using UnityEngine;
using System.Collections;

public class Slash : PlayerAttack {

	public bool can_deflect = false;
	public Bullet1 bullet_1;
	double slashTimer;

	// Use this for initialization
	void Start () {
	
		slashTimer = 0.25;
		base.hitImpulse = Tools.AngleToVec2 (Tools.QuaternionToAngle (transform.rotation) + 90.0f, 300.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
		slashTimer -= Time.deltaTime;

		if (slashTimer <= 0) {

			Destroy (gameObject);

		}


	}

	void OnTriggerEnter2D( Collider2D col ) {

		if (col.tag == "Enemy") {

			//print (col.gameObject);
			col.gameObject.SendMessage("OnHit", (PlayerAttack)this);

		} else if (can_deflect && col.gameObject.GetComponent<EnemyBullet>() != null) {
			// Delfects an incoming bullet back at an enemy
			Bullet1 reverse = (Bullet1)Instantiate(bullet_1, col.gameObject.transform.position, Quaternion.identity);
			// Sets transfomr's parent for indicating, which player deflected the bullet
			reverse.transform.parent = this.transform.parent;
			reverse.set_duration(1f);
			reverse.damage = 2 * col.gameObject.GetComponent<EnemyBullet>().damage;
			// Sets bullet trajectory
			reverse.GetComponent<Rigidbody2D>().velocity = -col.gameObject.GetComponent<Rigidbody2D>().velocity;
		}

	}

}
