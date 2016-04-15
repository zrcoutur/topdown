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
	public Weapon weapon;
	public Slash slash;
	public Slash slash2;
	public Shine shine;
	public Bullet1 bullet1;
	public Bullet2 bullet2;
	public Bullet3 bullet3;
	public Grenade grenade;
	public Grenade s_grenade;
	public RPG rpg;
	public CameraRunner cam;
	public Slider hpSlider;
	public Slider energySlider;
	public Slider shieldSlider;
	public Color[] colors;
	//public DynamicGUI upgrade_window;
	public AudioClip X_Slash;
	public AudioClip X_Weapon_Swap;
	public AudioClip X_Bullet_Shoot;
	public AudioClip X_Rifle_Shoot;
	public AudioClip X_Shotgun_Shoot;
	public AudioClip X_MegaShotgun_Shoot;
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
	private float death_timer = 5f;

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
		ammo_regen = new Regen_Counter(0.3f, 0.2f);
		shieldRegenTime = shieldMaxRegenTime;
		shieldRecoverTime = shieldMaxRecoverTime;

		// Create weapon object and make it follow you
		wep = (Weapon) Instantiate( weapon, body.position, transform.rotation );
		wep.stats = stats;
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
			death_timer -= Time.deltaTime;

			// Perform once
			if (uponDeath) {

				// Destroy your weapon if its not already destroyed
				if (wep != null) {
					Destroy(wep.gameObject);
				}
				

				// Stop turning
				body.freezeRotation = true;

				// Set dead animation
				anim.SetBool("Dead", true);

				// Stop color
				Srenderer.color = colors [0];

				// Slow down movement
				body.drag = 100.0f;

				// Play death SFX
				CameraRunner.gAudio.PlayOneShot(X_Die);

				// Print out player scores
				score.display_scores();

				uponDeath = false;

			} else if (death_timer <= 0f) {
				PlayerPrefs.SetInt("FinalScore", (int)this.score.totalScore);
				UnityEngine.SceneManagement.SceneManager.LoadScene("gameOver");
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
				
				stats.change_shield( (int)(shieldSlider.maxValue / 50f + 1f) );
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
			body.AddForce (new Vector2 (0, 22));
		}

		// Move Down
		if (Input.GetKey( M_MoveDown )) {
			body.AddForce (new Vector2 (0, -22));
		}

		// Move Left
		if (Input.GetKey( M_MoveLeft )) {
			body.AddForce (new Vector2 (-22, 0));
		}

		// Move Right
		if (Input.GetKey( M_MoveRight )) {
			body.AddForce (new Vector2 (22, 0));
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
			// Play swap sound
			CameraRunner.gAudio.PlayOneShot( X_Weapon_Swap, 1.0f );

			GetComponentInChildren<DynamicGUI>().switchWeaponStats();
			// Change weapon sprite
			wep.updateWeapon();

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
	void PerformAttack (int weaponType, bool pressed) {
		
		// You cannot fire when the upgrade window is open
		if (GetComponentInChildren<DynamicGUI>().isOpen()) {
			return;
		}


		switch (weaponType) {

		case (int)WEAPON_TYPE.sword:

			// Only slash on key-down
			if (!pressed)
				break;

			// Determine weapon subtype
			switch (stats.weapon_by_type(WEAPON_TYPE.sword).upgrade_state()) {

			// Standard Beam Sword
			case 0:

				// Play Slash Sound
				CameraRunner.gAudio.PlayOneShot(X_Slash, 1.0f);

				// Make Slash Effect
				var sl = (Slash)Instantiate(slash, body.position, transform.rotation);
				sl.transform.parent = transform;
				score.sword_attacks++;
				sl.setDamage(damage_for_weapon());

				// Shake camera
				cam.AddShake(0.08f);

				// Momentum from swing
				body.AddForce(Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, 120.0f));

				// Hide weapon
				wep.GetComponent<Renderer>().enabled = false;

				// Cooldown
				atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

				break;

			// Omega Beam Sword
			case 1:

				// Play Slash Sound
				CameraRunner.gAudio.PlayOneShot(X_Slash, 1.0f);

				// Make Slash Effect
				var sl2 = (Slash)Instantiate(slash2, body.position, transform.rotation);
				sl2.can_deflect = true;
				sl2.transform.parent = transform;
				score.sword_attacks++;
				sl2.setDamage(damage_for_weapon());

				// Shake camera
				cam.AddShake(0.12f);

				// Momentum from swing
				body.AddForce(Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, 200.0f));

				// Hide weapon
				wep.GetComponent<Renderer>().enabled = false;

				// Cooldown
				atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

				break;

			}

			break;

		case (int)WEAPON_TYPE.rifle:

			// Determine weapon subtype
			switch (stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state()) {

			// Plasma Rifle
			case 0:

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

				b1.setDamage(damage_for_weapon());
				b1.setDuration(UnityEngine.Random.Range(75, 115) / 100f);

				// Mildly shake camera
				cam.AddShake(0.06f);

				// Calculate bullet's velocity

				// Shot spread range.
				var spread = Random.Range(-3.0f, 3.0f);

				// Set final velocity based on travel angle
				b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f + spread, 15.0f);

				break;

			// Lancer Rifle
			case 1:

				// Ammo Check
				if (!UseAmmo(4.0f * stats.weapon_by_type(WEAPON_TYPE.rifle).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}

				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current() * 0.35f);

				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_Rifle_Shoot, 1.0f);

				// Calculate creation position of bullet (from gun)
				pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);

				// Create bullet
				var b2 = (Bullet2)Instantiate(bullet2, pos, transform.rotation);

				b2.transform.parent = transform;
				score.bullets_fired++;

				b2.setDamage((int)(3.5f * damage_for_weapon()));
				b2.setDuration(UnityEngine.Random.Range(125, 265) / 100f);

				// Mildly shake camera
				cam.AddShake(0.08f);

				// Calculate bullet's velocity

				// Set final velocity based on travel angle
				b2.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, 20.0f);

				break;
			
			// Plasma Gatling			
			case 2:
				// Ammo Check
				if (!UseAmmo(0.25f * stats.weapon_by_type(WEAPON_TYPE.rifle).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}

				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current() * 2.0f);

				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 0.4f);

				// Calculate creation position of bullet (from gun)
				pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);

				// Create bullet
				b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);

				b1.transform.parent = transform;
				score.bullets_fired++;

				b1.setDamage((int)(0.66f * damage_for_weapon()));
				b1.setDuration(UnityEngine.Random.Range(65, 105) / 100f);

				// Mildly shake camera
				cam.AddShake(0.04f);

				// Calculate bullet's velocity

				// Shot spread range.
				spread = Random.Range(-12.0f, 12.0f);

				// Set final velocity based on travel angle
				b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + spread + 90.0f, 18.0f);

				break;

			// Twin Cannon
			case 3:

				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(WEAPON_TYPE.rifle).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}

				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current());

				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

				// Calculate creation position of bullet (from gun)
				pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);

				// Create bullet
				b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);
				var b0 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);

				b1.transform.parent = transform;
				b0.transform.parent = transform;
				score.bullets_fired += 2;

				var dmg = damage_for_weapon();
				b1.setDamage(dmg);
				b0.setDamage(dmg);

				var twinDur = UnityEngine.Random.Range(95, 125) / 100f;
				b1.setDuration(twinDur);
				b0.setDuration(twinDur);

				// Mildly shake camera
				cam.AddShake(0.09f);

				// Calculate bullet's velocity

				// Set final velocity based on travel angle
				b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, 14.0f);
				b0.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, 14.0f);

				// Coloration
				//b0.GetComponent<SpriteRenderer>().color = new Vector4(0.5f,0.7f,1f,1f);
				//b1.GetComponent<SpriteRenderer>().color = new Vector4(0.5f,0.7f,1f,1f);

				b0.twin = 1;
				b0.osc = 1;

				b1.twin = 1;
				b1.osc = -1;

				break;


			}

			break;

		case (int)WEAPON_TYPE.shotgun:

			// Determine weapon subtype
			switch (stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state()) {

			// Shotgun
			case 0:
				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(WEAPON_TYPE.shotgun).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}
	
				// Cooldown
				atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();
	
				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_Shotgun_Shoot, 1.0f);
	
				// Fire five bullets in succession
				for (int bullet = 0; bullet <= 4; ++bullet) {
		
					// Calculate creation position of bullet (from gun)
					var pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);
	
					// Create bullet
					var b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);
					b1.transform.parent = transform;
					score.bullets_fired++;
					b1.setDamage(damage_for_weapon());
					b1.setDuration(0.16f);
	
					// Calculate bullet's velocity
	
					// Shot spread range.
					var spread = Random.Range(-15.0f, 15.0f);
	
					// Set final velocity based on travel angle
					b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f + spread, 13.0f);
				}
	
				// Mildly shake camera
				cam.AddShake(0.135f);
	
				break;
	
			// Armageddon
			case 1:
				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(WEAPON_TYPE.shotgun).weapon_stat(STAT_TYPE.ammo).current() * 2.0f)) {
					break;
				}
	
				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current() * 0.5f);
	
				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_MegaShotgun_Shoot, 1.0f);
	
				// Fire TWELVE bullets in succession
				for (int bullet = 0; bullet <= 12; ++bullet) {
		
					// Calculate creation position of bullet (from gun)
					var pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);
	
					// Create bullet
					var b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);
					b1.transform.parent = transform;
					score.bullets_fired++;
					b1.setDamage((int)(1.6f * damage_for_weapon()));
					b1.setDuration(0.14f);
	
					// Calculate bullet's velocity
	
					// Shot spread range.
					var spread = Random.Range(-30.0f, 30.0f);
	
					// Set final velocity based on travel angle
					b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f + spread, 12f);
				}
	
				// Moderately shake camera
				cam.AddShake(0.435f);
	
				break;

			// Auto shotgun
			case 2:
				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(WEAPON_TYPE.shotgun).weapon_stat(STAT_TYPE.ammo).current() * 0.5f)) {
					break;
				}
	
				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current() * 1.2f);
	
				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_Shotgun_Shoot, 1.0f);
	
				// Fire six bullets in succession
				for (int bullet = 0; bullet <= 6; ++bullet) {
		
					// Calculate creation position of bullet (from gun)
					var pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);
	
					// Create bullet
					var b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);
					b1.transform.parent = transform;
					score.bullets_fired++;
					b1.setDamage((int)(0.7f * damage_for_weapon()));
					b1.setDuration(0.2f);
	
					// Calculate bullet's velocity
	
					// Shot spread range.
					var spread = Random.Range(-12.0f, 12.0f);
	
					// Set final velocity based on travel angle
					b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f + spread, 15.0f);
				}

				// Mildly shake camera
				cam.AddShake(0.2f);
				break;

			// Conflux Shot
			case 3:

				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(WEAPON_TYPE.shotgun).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}
	
				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current());
	
				// Play Shoot Sound
				CameraRunner.gAudio.PlayOneShot(X_Shotgun_Shoot, 1.0f);
	
				// Fire five bullets in succession
				for (int bullet = 0; bullet <= 4; ++bullet) {
		
					// Calculate creation position of bullet (from gun)
					var pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1.0f);
	
					// Create bullet
					var b3 = (Bullet3)Instantiate(bullet3, pos, transform.rotation);
					b3.transform.parent = transform;
					score.bullets_fired++;
					b3.setDamage(damage_for_weapon()); // Note that damage is increased by the shot behavior.
					b3.setDuration(0.25f);
					b3.outset = -60.0f + 30.0f * bullet;
	
					// Calculate bullet's velocity
	
					// Shot spread range.
	
					// Set final velocity based on travel angle
					b3.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 30.0f + bullet * 30.0f, 13.0f);
				}

				// Mildly shake camera
				cam.AddShake(0.335f);

				break;
			}

			break;
		// Grenade Launcher
		case (int)WEAPON_TYPE.grenade:
			Grenade gnd;
			float gnd_spread;
			Vector2 gnd_pos;

			switch (stats.weapon_by_type(stats.current_weapon()).upgrade_state()) {

			// Grenade Launcher
			case 0:
				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}

				// Cooldown
				atkCool = 1.8f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

				// Play Shoot Sound
				// TODO replace with grenade shoot sound!
				CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

				// Calculate creation position of grenade (from gun)
				gnd_pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 0.5f);

				// Create grenade
				gnd = (Grenade)Instantiate(grenade, gnd_pos, transform.rotation);

				gnd.transform.parent = transform;

				gnd.setDamage(damage_for_weapon());
				gnd.setDuration(1.1f);

				// Calculate bullet's velocity

				// Set final velocity based on travel angle
				gnd.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, Random.Range(7.5f, 9.5f));

				// Mildly shake camera
				cam.AddShake(0.2f);
				
				break;
			
			// RPG
			case 1:

				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.ammo).current() * 1.2f)) {
					break;
				}

				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current() * 0.9f);

				// Play Shoot Sound
				// TODO replace with grenade shoot sound!
				CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

				// Calculate creation position of grenade (from gun)
				gnd_pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 1f);

				// Create grenade
				var rpg = (RPG)Instantiate(this.rpg, gnd_pos, transform.rotation);

				rpg.transform.parent = transform;

				rpg.setDamage( (int)(2.5f * damage_for_weapon()) );
				rpg.setDuration(5f);
				rpg.setAcceleration(6f);
				// Calculate bullet's velocity

				// Set final velocity based on travel angle
				rpg.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + 90.0f, 4f);

				// Mildly shake camera
				cam.AddShake(0.2f);

				break;
			
			// Cluster Cannon
			case 2:
				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.ammo).current() * 0.85f)) {
					break;
				}

				// Cooldown
				atkCool = 2.0f / (stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current() * 1.15f);
				// spawn multiple clusters
				for (int idx = 0; idx < 5; ++idx) {
					// Calculate creation position of grenade (from gun)
					gnd_pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 0.5f);

					// Create grenade
					gnd = (Grenade)Instantiate(grenade, gnd_pos, transform.rotation);
					gnd.transform.parent = transform;

					// reduce the size of the grenade sprite
					Vector3 gnd_scale = gnd.transform.localScale;
					gnd.transform.localScale = new Vector3(0.8f * gnd_scale.x, 0.8f * gnd_scale.y, gnd_scale.z);

					gnd.setDamage( (int)(0.7f * damage_for_weapon()) );
					gnd.setDuration(0.8f);

					// Calculate bullet's velocity
					// Shot spread range.
					gnd_spread = Random.Range(-35f, 35f);

					// Set final velocity based on travel angle
					gnd.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + gnd_spread + 90.0f, Random.Range(6f, 9f));
				}

				// Mildly shake camera
				cam.AddShake(0.1f);

				break;

			// Slow Grenades
			case 3:
				// Ammo Check
				if (!UseAmmo(stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.ammo).current())) {
					break;
				}

				// Cooldown
				atkCool = 2.0f / stats.weapon_by_type(stats.current_weapon()).weapon_stat(STAT_TYPE.rate_of_fire).current();

				// Play Shoot Sound
				// TODO replace with grenade shoot sound!
				CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

				// Calculate creation position of grenade (from gun)
				gnd_pos = body.position + Tools.AngleToVec2((body.rotation * transform.forward).z + 70.0f, 0.5f);

				// Create grenade
				gnd = (Grenade)Instantiate(s_grenade, gnd_pos, transform.rotation);

				gnd.transform.parent = transform;

				gnd.setDamage(damage_for_weapon());
				gnd.setDuration(0.9f);

				// Calculate bullet's velocity
				// Shot spread range.
				gnd_spread = Random.Range(-10f, 10f);

				// Set final velocity based on travel angle
				gnd.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z + gnd_spread + 90.0f, Random.Range(9f, 13f));

				// Mildly shake camera
				cam.AddShake(0.15f);

				break;
			
			}

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
			// SFX
			CameraRunner.gAudio.PlayOneShot(X_Medpack_Get);

			// Shine effect
			Instantiate(shine, transform.position, Quaternion.Euler(0, 0, 0));
			Destroy(trigger.gameObject);

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
			// SFX
			CameraRunner.gAudio.PlayOneShot(X_Core_Get);

			// Shine effect
			Instantiate(shine, transform.position, Quaternion.Euler(0, 0, 0));
			Destroy(trigger.gameObject);

			//Debug.Log("Cores: " + stats.get_ecores() + "\n");
			Destroy(obj);
			score.ecores_collected++;
			stats.change_ecores(1);

			// Scrap
		} else if (obj.tag == "scrap") {
			// SFX
			CameraRunner.gAudio.PlayOneShot(X_Scrap_Get);

			// Shine effect
			Instantiate(shine, transform.position, Quaternion.Euler(0, 0, 0));
			Destroy(trigger.gameObject);

			//Debug.Log("Scrap: " + stats.get_scrap() + "\n");
			Destroy(obj);
			score.scrap_collected++;
			stats.change_scrap(1);
		} else if (obj.tag == "weapon_pack") {

			stats.weapon_by_type((WEAPON_TYPE)(obj.GetComponent<WeaponUpgrade>().weapon)).setUgrade(1);
			Destroy(obj);
		} else if (trigger.gameObject.GetComponent<Explosion>() != null) {
			// The player suffers 10% damage from explosions
			GetHurt( (int)(0.1f * trigger.gameObject.GetComponent<Explosion>().getDamage()) );
		}
	}

	/* Checks the curent values of each weapon stat and upgrades any weapon that meets a
	 * specific criteria:
	 * 
	 * The sum of a weapon's levels for damage, rate of fire, and ammo consumption must
	 * be greater than or equal to 7 (or 6 in the case of the sword).
	 * For the damage upgrade, the damage level must be greater than or equal to 4 and greater than the rate of fire level,
	 * for the speed upgrade the rate of fire level must be greater than or equal to 3 and greater than or equal to the damage level,
	 * for any other case, the non focus upgrade is reached after a total of 7 levels. */
	public void updateWeapons() {
		/* Loop through all weapons. */
		for (WEAPON_TYPE idx = WEAPON_TYPE.sword; idx <= WEAPON_TYPE.grenade; ++idx) {
			WeaponStats weapon = stats.weapon_by_type(idx);
			// Verify that weapon has not already been upgraded
			if (weapon.upgrade_state() == 0) {
				if (idx == WEAPON_TYPE.sword) {
					// Only the sword's damage can be upgraded
					if (weapon.weapon_stat(STAT_TYPE.damage).pointer_value() >= 6) {
						weapon.setUgrade(1);
					}
				} else {
					// determine each of the weapon's stat's current level
					int dmg_lvl = weapon.weapon_stat(STAT_TYPE.damage).pointer_value();
					int rof_lvl = weapon.weapon_stat(STAT_TYPE.rate_of_fire).pointer_value();
					int ac_lvl = weapon.weapon_stat(STAT_TYPE.ammo).pointer_value();
					// the sum of all three levels must greater than 5
					if ((dmg_lvl + rof_lvl + ac_lvl) >= 7) {

						if (dmg_lvl >= 4) { // damage focus upgrade
							weapon.setUgrade(1);
						} else if (rof_lvl >= 3 && rof_lvl >= dmg_lvl) { // speed focus upgrade
							weapon.setUgrade(2);
						} else { // no focus upgrade
							weapon.setUgrade(3);
						}
					}
				}
			}
		}

		wep.updateWeapon();
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
		private float delayed = 0.0f;

		/* Creates a regen counter with the given rate and chance in rate. */
		public Regen_Counter(float r, float delta) {
			rate = r;
			rate_delta = delta;
			rate_counter = 1f;
			counter = 0f;
			delayed = 0.2f;
		}

		/* Incements the counter and rate counter. If the counter reaches
		 * the rate value, then 2 is returned, otherwise 0 is returned. */
		public int increment() {
			if (delayed >= 0) { // regen is delayed
				delayed -= Time.deltaTime;
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
		public void delay_regen() { delayed = 0.2f; }
	}
}
