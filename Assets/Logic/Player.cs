using UnityEngine;
using System.Collections;

enum weapons { BeamSword, PlasmaRifle, Shotgun, GrenadeLauncher };

public class Player : MonoBehaviour {

	Weapon wep;
	Rigidbody2D body;
	Animator anim;
	double atkCool;
	int heldWeapon; //1 is sword, 2 is pistol, 3 shotgun, 4 Grenade Launcher

    public Weapon weapon;
	public Slash slash;
	public Bullet1 bullet1;
	public CameraRunner cam;
    //public GameObject GrenadeLauncher;
     

	// Use this for initialization
	void Start () {

		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		heldWeapon = 0;

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
		if (Input.GetKey(KeyCode.UpArrow)) {
			body.AddForce (new Vector2 (0, 20));
		}

		// Move Down
		if (Input.GetKey(KeyCode.DownArrow)) {
			body.AddForce (new Vector2 (0, -20));
		}

		// Move Left
		if (Input.GetKey(KeyCode.LeftArrow)) {
			body.AddForce (new Vector2 (-20, 0));
		}

		// Move Right
		if (Input.GetKey(KeyCode.RightArrow)) {
			body.AddForce (new Vector2 (20, 0));
		}

		// Strafe Input
		body.freezeRotation = (Input.GetKey(KeyCode.LeftShift));

		var tDir = body.velocity;

        //if (Input.GetMouseButton(0))
       // {
       //    GameObject grenade= (GameObject)Instantiate(GrenadeLauncher,gameObject.transform.position,transform.rotation);
       //     grenade.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(transform.rotation.z), Mathf.Sin(transform.rotation.z)) * 20;
      //  }
        
        // Rotate to face if moving fast enough
		if (tDir.magnitude > 1f) {

			// Lerp between current facing and target facing
			var currentAng = body.rotation;
			var targetAng = 270.0f + Mathf.Atan2 (tDir.y, tDir.x) * Mathf.Rad2Deg;

			body.MoveRotation (Mathf.MoveTowardsAngle (currentAng, targetAng, 20));

			// Set walking flag
			anim.SetBool ("Walk", true);

		} else {

			// Turn off walking flag
			anim.SetBool ("Walk", false);

		}

		/************************
		 * WEAPON SWAP
		 ************************/
		if (Input.GetKeyDown (KeyCode.S)) {

			heldWeapon = 1 - heldWeapon;

			wep.GetComponent<Animator> ().SetInteger ("WeaponID", heldWeapon);

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
			if (Input.GetKey (KeyCode.Z)) {

				var pressed = Input.GetKeyDown( KeyCode.Z);

				PerformAttack (heldWeapon, pressed );

			}

		}
			
	}

	/*******************************************************************************
	 * 
	 * Performs an attack based on the weapon type provided and
	 * a flag that checks whether or not the button was just
	 * pressed (for 'sticky' attack styles)
	 * 
	 *******************************************************************************/
	void PerformAttack ( int weaponType, bool pressed ) {

		switch (weaponType) {

		case (int)weapons.BeamSword:

			// Only slash on key-down
			if (!pressed)
				break;

			// Make Slash Effect
			var sl = (Slash)Instantiate (slash, body.position, transform.rotation);
			sl.transform.parent = transform;

			// Shake camera
			cam.AddShake( 0.3f );

			// Momentum from swing
			body.AddForce ( AngleToVec2( (body.rotation * transform.forward).z + 90.0f, 120.0f ) );

			// Hide weapon
			wep.GetComponent<Renderer> ().enabled = false;

			// Cooldown
			atkCool = 0.45;

			break;

		case (int)weapons.PlasmaRifle:

			// Calculate creation position of bullet (from gun)
			var pos = body.position + AngleToVec2( (body.rotation * transform.forward).z + 70.0f, 1.0f );

			// Create bullet
			var b1 = (Bullet1)Instantiate (bullet1, pos, transform.rotation);

			// Mildly shake camera
			cam.AddShake( 0.2f );

			// Calculate bullet's velocity

			// Shot spread range.
			var spread = Random.Range( -5.0f, 5.0f );

			// Set final velocity based on travel angle
			b1.GetComponent<Rigidbody2D> ().velocity = AngleToVec2 ( (body.rotation * transform.forward).z + 90.0f + spread, 15.0f);
				
			// Cooldown
			atkCool = 0.1;

			break;

		}

	}

	/*******************************************************************************
	 * 
	 * Converts an angle to a Vector2 with a given magnitude.
	 * 
	 *******************************************************************************/
	Vector2 AngleToVec2( float angle, float magn ) {

		return new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad ) * magn, Mathf.Sin (angle * Mathf.Deg2Rad ) * magn);

	}

}
