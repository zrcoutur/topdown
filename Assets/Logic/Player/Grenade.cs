using UnityEngine;
using System.Collections;

public class Grenade : PlayerAttack {
	// Explosion animation of the grenade
	public GameObject explosion;

	// Update is called once per frame
	public void Update() {
		if (duration <= 0f) {
			// TODO explode!
			Explosion exp = ((GameObject)Instantiate(explosion, transform.position, Quaternion.identity)).GetComponent<Explosion>();
			Vector3 scale = exp.transform.localScale;
			// reduce scale of the explosion
			exp.transform.localScale = new Vector3(0.65f * scale.x, 0.65f * scale.y, scale.z);
			exp.setDamage(damage);
			Destroy(this.gameObject);
		} else {
			duration -= Time.deltaTime;
		}
	}

	public void OnColisionEnter(Collider2D collider) {
		// explosions instantly trigger a grenade to explode
		if (collider.gameObject.GetComponent<Explosion>() != null) {
			duration = 0f;
		}
	}
}
	