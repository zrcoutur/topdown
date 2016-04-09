using UnityEngine;
using System.Collections;

/**
 * This class defines the behavoir of an RPG projectile, which is fired by the Player's
 * damage-focused grenade launcher upgrade. RPGs are similiar to grenades, expect that
 * an RPG will explode on contact and RPGs have acceleration.
 * 
 * author : Joshua Hooker
 * 1 April 2016
 */
public class RPG : Grenade {
	// The RPG's acceleration in the x-y plane.
	private float acceleration;
	
	// Update is called once per frame
	public new void Update () {
		// Explode after a set period of time
		if (duration <= 0f) {
			explode();
		} else {
			Vector2 origin = GetComponent<Rigidbody2D>().velocity;
			// gain velocity overtime
			GetComponent<Rigidbody2D>().velocity += acceleration * Time.deltaTime * origin;
			duration -= Time.deltaTime;
		}
	}

	public void OnCollisionEnter2D(Collision2D trigger) {
		if (trigger.gameObject.tag != "med_pack" && trigger.gameObject.tag != "scrap" && trigger.gameObject.tag != "core" && trigger.gameObject.tag != "mine") {
			// explode on contact
			explode();
		}
	}

	/* Creates an explosion for the RPG and removes the RPG from the game */
	private void explode() {
		Explosion exp = ((GameObject)Instantiate(explosion, transform.position, Quaternion.identity)).GetComponent<Explosion>();
		Vector3 scale = exp.transform.localScale;
		// reduce scale of the explosion
		exp.transform.localScale = new Vector3(1.5f * scale.x, 1.5f * scale.y, scale.z);
		// set explosion damage
		exp.setDamage(damage);
		Destroy(this.gameObject);
	}

	// getter and setters for the RPG's acceleration
	public void setAcceleration(float acc) { acceleration = acc; }
	public float getAcceleration() { return acceleration; }
}
