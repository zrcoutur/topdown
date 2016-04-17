using UnityEngine;
using System.Collections;

/**
 * This class defines the logic for the Boomarang slash sword upgrade, which is the speed focused upgrade
 * for the sword. This attack will spawn a small blades that will orbit the player, damaging nearby enemies
 * and blocking bullets.
 * 
 * author : Joshua Hooker
 * 17 April 2016
 */
public class Bslash : Slash {

	public GameObject blades;

	// Use this for initialization
	void Start() {
		base.hitImpulse = Tools.AngleToVec2 (Tools.QuaternionToAngle (transform.rotation) + 90.0f, 100.0f);
	}

	/* This method will spawn a small blade that will orbit the player.
	 * The method returns the blade that was created. */
	public GameObject spawn_blades() {
		Rigidbody2D body = GetComponent<Rigidbody2D>();

		// Spawn a boomarang blade
		var blade = (GameObject)Instantiate(blades, body.position, Quaternion.identity);
		blade.transform.parent = transform.parent;

		blade.GetComponent<Boomarang>().setDamage(damage / 2);

		return blade;
	}

}

