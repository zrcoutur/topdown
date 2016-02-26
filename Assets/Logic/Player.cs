using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum weapons { BeamSword, PlasmaRifle, Shotgun, GrenadeLauncher };

public class Player : MonoBehaviour {

	//public DynamicGUI upgradeWindow;
	AudioSource Paudio;
	Weapon wep;
	Rigidbody2D body;
	Animator anim;
	double atkCool;
	int heldWeapon; //0 is sword, 1 is rifle, 2 shotgun, 3 Grenade Launcher
	// Used to recover ammo
	private float ammo_recovery_rate;
	private float ammo_counter;

	public Weapon weapon;
	public Slash slash;
	public Bullet1 bullet1;
	public CameraRunner cam;
	public Slider hpSlider;
	public Slider energySlider;

    private int maxAmmo;
    public int health;
    public int ammo;
    public int shield;
    public int energyCores;
    public int scrap;

	public AudioClip X_Slash;
	public AudioClip X_Weapon_Swap;
	public AudioClip X_Bullet_Shoot;
    //public GameObject GrenadeLauncher;

	// Keycodes
	KeyCode M_MoveLeft = KeyCode.A;
	KeyCode M_MoveRight = KeyCode.D;
	KeyCode M_MoveUp = KeyCode.W;
	KeyCode M_MoveDown = KeyCode.S;
	KeyCode M_Swap = KeyCode.E;
	KeyCode M_Shoot = KeyCode.Mouse0;
	KeyCode M_Strafe = KeyCode.LeftShift;

	int GetHealth() {
		return health;
	}

	// Use this for initialization
	void Start () {

		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		Paudio = GetComponent<AudioSource> ();

		heldWeapon = 0;
		maxAmmo = 100;
		ammo = 100;
		ammo_recovery_rate = 0.2f;
		ammo_counter = ammo_recovery_rate;
		health = Storage.MAX_HEALTH.current();

		// Create weapon object and make it follow you
		wep = (Weapon) Instantiate( weapon, body.position, transform.rotation );
		wep.transform.parent = transform;

	}
	
	/*******************************************************************************
	 * 
	 * General player step behavior
	 * 
	 *******************************************************************************/
	void Update () {

		/************************
		 * MOVEMENT
		 ************************/

		// Move Up
		if (Input.GetKey( M_MoveUp )) {
			body.AddForce (new Vector2 (0, 20));
		}

		// Move Down
		if (Input.GetKey( M_MoveDown )) {
			body.AddForce (new Vector2 (0, -20));
		}

		// Move Left
		if (Input.GetKey( M_MoveLeft )) {
			body.AddForce (new Vector2 (-20, 0));
		}

		// Move Right
		if (Input.GetKey( M_MoveRight )) {
			body.AddForce (new Vector2 (20, 0));
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
			heldWeapon = (heldWeapon + 1) % 3;

			// Play swap sound
			Paudio.PlayOneShot( X_Weapon_Swap, 1.0f );
			// shotgun has no unique arm style yet
			wep.GetComponent<Animator>().SetInteger ("WeaponID", (heldWeapon == 2) ? 1 : heldWeapon);

		}


		/************************
		 * ATTACKING
		 ************************/

		atkCool -= Time.deltaTime;

		// Attack check
		if (atkCool <= 0) {

			// Make weapon visible
			wep.GetComponent<Renderer> ().enabled = true;

			// Attack Check
			if (Input.GetKey ( M_Shoot )) {

				var pressed = Input.GetKeyDown( M_Shoot );

				PerformAttack (heldWeapon, pressed );

			}

		}
		// passively recover ammo overtime
		if (ammo_counter >= ammo_recovery_rate) {
			ammo_counter = 0.0f;
			GainAmmo(3);
		} else {
			ammo_counter += Time.deltaTime;
		}
			
	}

	/*******************************************************************************
	 *
	 * Called whenever the player is inflicted any damage. Updates UI info, too.
	 *
	 *******************************************************************************/
	public void GetHurt( int damageTaken ) {
		health -= damageTaken;
		hpSlider.value = health;
	}

	/*******************************************************************************
	 * 
	 * Called whenever the player is healed. Updates UI info, too.
	 * 
	 *******************************************************************************/
	public void GetHealed( int damageRecovered ) {

		// Cap health at maximum
		health = Mathf.Min( health + damageRecovered, Storage.MAX_HEALTH.current() );
		hpSlider.value = health;
		// TODO: heal sfx/effect?
	}

	/*******************************************************************************
	 *
	 * Expends ammo (i.e. energy) if you have enough. If you don't have enough,
	 * does nothing and returns false.
	 *
	 *******************************************************************************/
	bool UseAmmo( int cost ) {
		
		// Check if you have enough ammo
		if (ammo >= cost) {
			ammo = Mathf.Max( 0, ammo - cost );
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
	void GainAmmo( int ammoGained ) {
		ammo = Mathf.Min( ammo + ammoGained, maxAmmo );
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
//		if (upgradeWindow.isOpen()) { return; }

		switch (weaponType) {

		case (int)WEAPON_TYPE.sword:

			// Only slash on key-down
			if (!pressed)
				break;

			// Play Slash Sound
			Paudio.PlayOneShot( X_Slash, 1.0f );

			// Make Slash Effect
			var sl = (Slash)Instantiate (slash, body.position, transform.rotation);
			sl.transform.parent = transform;

			// Shake camera
			cam.AddShake( 0.3f );

			// Momentum from swing
			body.AddForce ( Tools.AngleToVec2( (body.rotation * transform.forward).z + 90.0f, 120.0f ) );

			// Hide weapon
			wep.GetComponent<Renderer> ().enabled = false;

			// Cooldown
			atkCool = 2.0f / Storage.weapon_by_type((WEAPON_TYPE)heldWeapon).stat_by_type(STAT_TYPE.rate_of_fire).current();

			break;

		case (int)WEAPON_TYPE.rifle:
			
			// Cooldown
			atkCool = 2.0f / Storage.weapon_by_type((WEAPON_TYPE)heldWeapon).stat_by_type(STAT_TYPE.rate_of_fire).current();

			// Ammo Check
			if ( !UseAmmo( Storage.weapon_by_type(WEAPON_TYPE.rifle).stat_by_type(STAT_TYPE.ammo).current() ) ) {
				break;
			}

			// Play Shoot Sound
			Paudio.PlayOneShot( X_Bullet_Shoot, 1.0f );

			// Calculate creation position of bullet (from gun)
			var pos = body.position + Tools.AngleToVec2( (body.rotation * transform.forward).z + 70.0f, 1.0f );

			// Create bullet
			var b1 = (Bullet1)Instantiate (bullet1, pos, transform.rotation);

			// Mildly shake camera
			cam.AddShake( 0.05f );

			// Calculate bullet's velocity

			// Shot spread range.
			var spread = Random.Range( -3.0f, 3.0f );

			// Set final velocity based on travel angle
			b1.GetComponent<Rigidbody2D> ().velocity = Tools.AngleToVec2 ( (body.rotation * transform.forward).z + 90.0f + spread, 15.0f);

			break;

		case (int)WEAPON_TYPE.shotgun:

			// Cooldown
			atkCool = 2.0f / Storage.weapon_by_type((WEAPON_TYPE)heldWeapon).stat_by_type(STAT_TYPE.rate_of_fire).current();

			// Ammo Check
			if ( !UseAmmo( Storage.weapon_by_type(WEAPON_TYPE.rifle).stat_by_type(STAT_TYPE.ammo).current() ) ) {
				break;
			}
			// Fire five bullets in succession
			for (int bullet = 0; bullet <= 4; ++bullet) {
				// Play Shoot Sound
				Paudio.PlayOneShot( X_Bullet_Shoot, 1.0f );
	
				// Calculate creation position of bullet (from gun)
				pos = body.position + Tools.AngleToVec2( (body.rotation * transform.forward).z + 70.0f, 1.0f );

				// Create bullet
				b1 = (Bullet1)Instantiate(bullet1, pos, transform.rotation);

				// Mildly shake camera
				cam.AddShake( 0.08f );

				// Calculate bullet's velocity

				// Shot spread range.
				spread = Random.Range( -15.0f, 15.0f );

				// Set final velocity based on travel angle
				b1.GetComponent<Rigidbody2D> ().velocity = Tools.AngleToVec2 ( (body.rotation * transform.forward).z + 90.0f + spread, 15.0f);
			}

			break;
		}

	}
}
