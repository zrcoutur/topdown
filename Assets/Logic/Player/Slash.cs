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

		} else if (col.gameObject.GetComponent<EnemyBullet>() != null) {
			// reflect enemy bullet
			if (can_deflect) {
				EnemyBullet eb = col.gameObject.GetComponent<EnemyBullet>();
				// Delfects an incoming bullet back at an enemy
				Bullet1 reverse = (Bullet1)Instantiate(bullet_1, eb.gameObject.transform.position, Quaternion.identity);
				// Sets transfomr's parent for indicating, which player deflected the bullet
				reverse.transform.parent = this.transform.parent;
				reverse.setDuration(eb.get_duration() + 0.5f);
				reverse.setDamage( col.gameObject.GetComponent<EnemyBullet>().damage );
				// Sets bullet trajectory
				reverse.GetComponent<Rigidbody2D>().velocity = -col.gameObject.GetComponent<Rigidbody2D>().velocity;
			}

			Destroy(col.gameObject);
		}

	}

}
