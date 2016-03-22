using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	// Object components
	Weapon wep;
	Rigidbody2D body;
	SpriteRenderer Srenderer;
	Animator anim;

	// Outside elements
	public Player_Stats stats;
	public readonly DynamicGUI upgradeWindow;
	public Weapon weapon;
	public Slash slash;
	public Shine shine;
	public Bullet1 bullet1;
	public CameraRunner cam;
	public Slider hpSlider;
	public Slider energySlider;
	public Slider shieldSlider;
	public Color[] colors;
	public DynamicGUI upgrade_window;
	public AudioClip X_Slash;
	public AudioClip X_Weapon_Swap;
	public AudioClip X_Bullet_Shoot;
	public AudioClip X_Shotgun_Shoot;
	public AudioClip X_Core_Get;
	public AudioClip X_Scrap_Get;
	public AudioClip X_Medpack_Get;
	public AudioClip X_Hurt;
	public AudioClip X_Die;
	public AudioClip X_Healed;
	public ScoreBoard score;

	// Parameters
	double atkCool;
	Regen_Counter ammo_regen;
	private static readonly float maxAmmo = 100f;
	float shieldRegenTime;
	float shieldRecoverTime;
	public float ammo;
	public float shieldMaxRegenTime = 2.5f;
	public float shieldMaxRecoverTime = 0.1f;

	// Timers, etc.
	float flash = 0;
	int toggle = 0;
	bool uponDeath = true;
	float spawnerCheck;
	float spawnerTime;

	// Keycodes
	KeyCode M_MoveLeft = KeyCode.A;
	KeyCode M_MoveRight = KeyCode.D;
	KeyCode M_MoveUp = KeyCode.W;
	KeyCode M_MoveDown = KeyCode.S;
	KeyCode M_Swap = KeyCode.E;
	KeyCode M_Shoot = KeyCode.Mouse0;
	KeyCode M_Strafe = KeyCode.LeftShift;

	// Returns the player's current health
	int GetHealth() {
		return stats.get_health();
	}

	// Use this for initialization
	void Start () {

		// Get components
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		Srenderer = GetComponent<SpriteRenderer>();

		// Set base params
		stats = new Player_Stats();
		ammo = 100f;
		ammo_regen = new Regen_Counter(0.45f, 0.25f);
		shieldRegenTime = shieldMaxRegenTime;
		shieldRecoverTime = shieldMaxRecoverTime;

		// Create weapon object and make it follow you
		wep = (Weapon) Instantiate( weapon, body.position, transform.rotation );
		wep.transform.parent = transform;

		score = new ScoreBoard();

		spawnerCheck = 0;
		spawnerTime = 10;
	}
	
	/*******************************************************************************
	 * 
	 * General player step behavior
	 * 
	 *******************************************************************************/
	void Update () {

		// Dead! Do nothing.
		if (stats.get_health() <= 0) {

			// Perform once
			if (uponDeath) {

				// Destroy your weapon if its not already destroyed
				if (wep != null)
				{
					Destroy(wep.gameObject);
				}
				

				// Stop turning
				body.freezeRotation = true;

				// Set dead animation
				anim.SetBool ("Dead", true);

				// Stop color
				Srenderer.color = colors [0];

				// Slow down movement
				body.drag = 100.0f;

				// Play death SFX
				CameraRunner.gAudio.PlayOneShot( X_Die );

				// Print out player scores
				score.display_scores();

				uponDeath = false;

				PlayerPrefs.SetInt ("FinalScore", (int) this.score.totalScore);
				UnityEngine.SceneManagement.SceneManager.LoadScene ("gameOver");
			}

			return;
		}

		// Flashing when hurt
		flash -= Time.deltaTime;

		if (flash >= 0)
		{
			toggle = 1 - toggle;
			Srenderer.color = colors[toggle];
		}
		else
			Srenderer.color = colors[0];

		/************************
		 * Shield Regen
		 ************************/

		shieldRegenTime -= Time.deltaTime;

		// Start regenning shield
		if (shieldRegenTime < 0) {

			// Time between 'ticks' of shield recovery
			shieldRecoverTime -= Time.deltaTime;

			// Regen a tick of shield if delay is over
			if ( shieldRecoverTime <= 0 && stats.get_shield() < stats.MAX_SHIELD.current() ) {
				
				stats.change_shield( (int)(shieldSlider.maxValue / 33f + 0.99f) );
				stats.Shield_raised = true;
				shieldRecoverTime += shieldMaxRecoverTime;
			
			}

			// Update slider
			shieldSlider.value = stats.get_shield();

		}

		/************************
		 * MOVEMENT
		 ************************/

		// Move Up
		if (Input.GetKey( M_MoveUp )) {
			body.AddForce (new Vector2 (0, 26));
		}

		// Move Down
		if (Input.GetKey( M_MoveDown )) {
			body.AddForce (new Vector2 (0, -26));
		}

		// Move Left
		if (Input.GetKey( M_MoveLeft )) {
			body.AddForce (new Vector2 (-26, 0));
		}

		// Move Right
		if (Input.GetKey( M_MoveRight )) {
			body.AddForce (new Vector2 (26, 0));
		}

		// Strafe Input
		body.freezeRotation = (Input.GetKey( M_Strafe ));

		/***********************
		 * ANIMATION
		 ***********************/

		var tDir = body.velocity;

		if (tDir.magnitude > 0.5f) {

			// Set walking flag
			anim.SetBool ("Walk", true);

		} else {

			// Turn off walking flag
			anim.SetBool ("Walk", false);

		}

		// Rotation

		// Get your current facing
		var currentAng = body.rotation;

		// Get the direction to the mouse
		var look = (Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position); // Vector representation
		var targetAng = 270.0f + Tools.Vector2ToAngle( look ); // Angle of that vector

		// Lerp between current facing and target facing
		body.MoveRotation (Mathf.MoveTowardsAngle (currentAng, targetAng, 20));

		/************************
		 * WEAPON SWAP
		 ************************/
		if (Input.GetKeyDown ( M_Swap )) {
			stats.cycle_weapons();
			GetComponentInChildren<DynamicGUI>().switchWeaponStats();

			atkCool = 0;

			// Play swap sound
			CameraRunner.gAudio.PlayOneShot( X_Weapon_Swap, 1.0f );
			// Change weapon sprite
			wep.updateWeapon = (int)stats.current_weapon();

		}


		/************************
		 * ATTACKING
		 ************************/

		atkCool -= Time.deltaTime;

		// Attack delay
		if (atkCool <= 0) {

			// Make weapon visible
			wep.GetComponent<Renderer> ().enabled = true;

			// Attack Input
			if (Input.GetKey ( M_Shoot )) {

				var pressed = Input.GetKeyDown( M_Shoot );

				PerformAttack ((int)stats.current_weapon(), pressed );
				ammo_regen.delay_regen();
			}

		}

		// passively recover ammo overtime
		int ammo = ammo_regen.increment();
		GainAmmo(ammo);
			
		// Press 'h' to use a medpack to restore half of your HP
		if ( Input.GetKeyDown(KeyCode.H) && stats.MEDPACKS.current() > 0 ) {
			GetHealed((int)stats.MAX_HEALTH.current() / 2);
			stats.MEDPACKS.decrement();
		}
		// Hold 'space' to gain ammo
		if ( Input.GetKey(KeyCode.Space) ) {
			GainAmmo(1);
		}

		//Activates spawners within a certain radius of the player every so often
		if (spawnerCheck >= spawnerTime)
		{
			
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 35, 1);

			//find nearby game objects that are spawners
			foreach (Collider2D col in hitColliders)
			{
				if (col.gameObject.name.Equals("baseSpawner(Clone)"))
				{
					col.gameObject.GetComponent<EnemySpawner>().setActive();
				}

			}
			spawnerCheck = 0;
		}
		else
		{
			spawnerCheck += Time.deltaTime;
		}



	}

	/*******************************************************************************
	 *
	 * Called whenever the player is inflicted any damage. Updates UI info, too.
	 *
	 *******************************************************************************/
	public void GetHurt( int damageTaken ) {

		// Lose shield first
		var underflow = stats.change_shield(-damageTaken);

		// If you take damage in excess of shield,
		// lose health then
		if (underflow < 0) {
			stats.change_health(underflow);
			hpSlider.value = stats.get_health();
		}

		// Flash when hurt
		flash = 0.4f;

		// Play Hurt SFX
		CameraRunner.gAudio.PlayOneShot( X_Hurt );

		// Reset shield regen window
		shieldRegenTime = shieldMaxRegenTime;

		// Update sliders
		hpSlider.value = stats.get_health();
		shieldSlider.value = stats.get_shield();
	}

	/*******************************************************************************
	 * 
	 * Called whenever the player is healed. Updates UI info, too.
	 * 
	 *******************************************************************************/
	public void GetHealed( int damageRecovered ) {

		// Cap health at maximum
		stats.change_health(damageRecovered);
		hpSlider.value = stats.get_health();

		// Heal SFX
		CameraRunner.gAudio.PlayOneShot( X_Healed );
	}

	/*******************************************************************************
	 *
	 * Expends ammo (i.e. energy) if you have enough. If you don't have enough,
	 * does nothing and returns false.
	 *
	 *******************************************************************************/
	bool UseAmmo( float cost ) {
		
		// Check if you have enough ammo
		if (ammo >= cost) {
			// Expend ammo
			ammo = Mathf.Max( 0, ammo - cost );

			// Update slider
			energySlider.value = ammo;

			return true;
		} 
		// TODO: 'No ammo' fx
		else {
			return false;
		}
	}

	/*******************************************************************************
	 *
	 * Called whenever you regain ammo. Updates UI info, too.
	 *
	 *******************************************************************************/
	void GainAmmo( float ammoGained ) {
		
		// Gain ammo up to maximum
		ammo = Mathf.Min( ammo + ammoGained, maxAmmo );

		// Update slider
		energySlider.value = ammo;
	}

	/*******************************************************************************
	 * 
	 * Performs an attack based on the weapon type provided and
	 * a flag that checks whether or not the button was just
	 * pressed (for 'sticky' attack styles)
	 * 
	 *******************************************************************************/
	void PerformAttack ( int weaponType, bool pressed ) {
		
		// You cannot fire when the upgrade window is open
		if (upgrade_window.isOpen()) { return; }


		switch (weaponType) {

		case (int)WEAPON_TYPE.sword:

			// Only slash on key-down
			if (!pressed)
				break;

			// Play Slash Sound
			CameraRunner.gAudio.PlayOneShot (X_Slash, 1.0f);

			// Make Slash Effect
			var sl = (Slash)Instantiate (slash, body.position, transform.rotation);
			sl.transform.parent = transform;
			score.sword_attacks++;
			sl.damage = damage_for_weapon ();

			// Shake camera
			cam.AddShake( 0.08f );

			// Momentum from swing
			body.AddForce ( Tools.AngleToVec2( (body.rotation * transform.forward).z + 90.0f, 120.0f ) );

			// Hide weapon
			wep.GetComponent<Renderer> ().enabled = false;

			// Cooldown
			atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

			break;

		case (int)WEAPON_TYPE.rifle:
			
			// Ammo Check
			if (!UseAmmo(stats.weapon_by_type(WEAPON_TYPE.rifle).weapon_stat(STAT_TYPE.ammo).current())) {
				break;
			}

			// Cooldown
			atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

			// Play Shoot Sound
			CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

			// Calculate creation position of bullet (from gun)
			var pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);

			// Create bullet
			var b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);

			b1.transform.parent = transform;
			score.bullets_fired++;

			b1.damage = damage_for_weapon();
			b1.set_duration(0.85f);

			// Mildly shake camera
			cam.AddShake( 0.06f );

			// Calculate bullet's velocity

			// Shot spread range.
			var spread = Random.Range( -3.0f, 3.0f );

			// Set final velocity based on travel angle
			b1.GetComponent<Rigidbody2D> ().velocity = Tools.AngleToVec2 ( (body.rotation * transform.forward).z + 90.0f + spread, 15.0f);

			break;

		case (int)WEAPON_TYPE.shotgun:

			// Ammo Check
			if ( !UseAmmo( stats.weapon_by_type(WEAPON_TYPE.shotgun).weapon_stat(STAT_TYPE.ammo).current() ) ) {
				break;
			}

			// Cooldown
			atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

			// Play Shoot Sound
			CameraRunner.gAudio.PlayOneShot( X_Shotgun_Shoot, 1.0f );

			// Fire five bullets in succession
			for (int bullet = 0; bullet <= 4; ++bullet) {
	
				// Calculate creation position of bullet (from gun)
				pos = body.position + Tools.AngleToVec2( (body.rotation * transform.forward).z + 70.0f, 1.0f );

				// Create bullet
				b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);
				b1.transform.parent = transform;
				score.bullets_fired++;
				b1.damage = damage_for_weapon();
				b1.set_duration(0.25f);

				// Calculate bullet's velocity

				// Shot spread range.
				spread = Random.Range( -15.0f, 15.0f );

				// Set final velocity based on travel angle
				b1.GetComponent<Rigidbody2D> ().velocity = Tools.AngleToVec2 ( (body.rotation * transform.forward).z + 90.0f + spread, 15.0f);
			}

			// Mildly shake camera
			cam.AddShake( 0.135f );

			break;
		}

		}

	/* Get current weapon damage */
	private int damage_for_weapon() {
		return (int)( stats.weapon_by_type(stats.current_weapon() ).weapon_stat(STAT_TYPE.damage).current() );
	}

	// Run into items
	public void OnTriggerEnter2D(Collider2D trigger) {
		GameObject obj = trigger.gameObject;
		// Med pack
		if (obj.tag == "med_pack") {
			// Shine effect
			Instantiate (shine, trigger.transform.position, Quaternion.Euler (0, 0, 0));
			// SFX
			CameraRunner.gAudio.PlayOneShot( X_Medpack_Get );

			Destroy(obj);
			int ret = stats.MEDPACKS.increment();
			// If you cannot hold anymore med_packs
			if (ret == 0) {
				stats.change_scrap(8);
				stats.change_ecores(1);
			}
		}
		// Energy Core
		else if (obj.tag == "core") {
			// Shine effect
			Instantiate (shine, trigger.transform.position, Quaternion.Euler (0, 0, 0));
			// SFX
			CameraRunner.gAudio.PlayOneShot( X_Core_Get );

			//Debug.Log("Cores: " + stats.get_ecores() + "\n");
			Destroy(obj);
			score.ecores_collected++;
			stats.change_ecores(1);

		// Scrap
		} else if (obj.tag == "scrap") {
			// Shine effect
			Instantiate (shine, trigger.transform.position, Quaternion.Euler (0, 0, 0));
			// SFX
			CameraRunner.gAudio.PlayOneShot( X_Scrap_Get );

			//Debug.Log("Scrap: " + stats.get_scrap() + "\n");
			Destroy(obj);
			score.scrap_collected++;
			stats.change_scrap(1);
		}
	}

	/* A simple class used to simulate the Player's ammo regenation. */
	private class Regen_Counter {
		// Time between each ammo gain (not necessarily in seconds!)
		private readonly float rate;
		// The percent increase of the rate counter overtime
		private readonly float rate_delta;
		// The percent increase (above 100%) of the change in time added to the counter
		private float rate_counter;
		// current point in time between ammo gains
		private float counter;
		// delays ammo gain when the Player is firing a gun
		private bool delayed;

		/* Creates a regen counter with the given rate and chance in rate. */
		public Regen_Counter(float r, float delta) {
			rate = r;
			rate_delta = delta;
			rate_counter = 1f;
			counter = 0f;
			delayed = false;
		}

		/* Incements the counter and rate counter. If the counter reaches
		 * the rate value, then 2 is returned, otherwise 0 is returned. */
		public int increment() {
			if (delayed) { // regen is delayed
				delayed = false;
				rate_counter = 1f;
			} else if (counter >= rate) { // return ammo gain
				counter = 0f;
				return 3;
			} else { // increment counter and rate counter
				counter += Time.deltaTime * rate_counter;
				rate_counter *= (1f + rate_delta);
			}

			return 0;
		}

		/* Set regen delay flag. */
		public void delay_regen() { delayed = true; }
	}
}
