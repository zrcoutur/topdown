using UnityEngine;
using System.Collections;

/**
 * This class defines the behavoirs of a simple grenade object that explodes after a period of time.
 * 
 * author : Joshua Hooker
 * 31 March 2016
 */
public class Grenade : PlayerAttack {
	// Explosion animation of the grenade
	public GameObject explosion;

	// Update is called once per frame
	public void Update() {

		if (!Time_Count.game_pause) {
			
			if (duration <= 0f) {
				Explosion exp = ((GameObject)Instantiate(explosion, transform.position, Quaternion.identity)).GetComponent<Explosion>();
				exp._parent = transform.parent;

				Vector3 scale = exp.transform.localScale;
				// reduce scale of the explosion
				exp.transform.localScale = new Vector3(1.15f * scale.x, 1.15f * scale.y, scale.z);
				// set explosion damage
				exp.setDamage(damage);
				Destroy(this.gameObject);
			} else {
				duration -= Time.deltaTime;
			}
		}
	}

	public void OnColisionEnter(Collider2D collider) {
		// reduce detonation time for theh grenade if it comes contact with an explosion
		if (collider.gameObject.GetComponent<Explosion>() != null) {
			duration -= 5f;
		}
	}
}
	