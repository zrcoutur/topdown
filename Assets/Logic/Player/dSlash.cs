using UnityEngine;
using System.Collections;

/**
 * This class defines the logic for the non-focus sword upgrade; the 
 * attack rotates around the player deflecting bullets and dealing
 * damage to nearby enemies.
 */
public class dSlash : Slash {
	// The bullet instaniated when an enemy bullet is deflected
	public Bullet1 bullet_1;
	// Allows the slash to rotate around the player
	private bool rotate = false;

	// Use this for initialization
	public void Start() {
		slashTimer = 0.25f;
	}
	
	// Update is called once per frame
	public void Update() {
		// Remove the slash after a certain period of time
		if (slashTimer <= 0f) {
			Destroy(gameObject);
		} else {
			slashTimer -= Time.deltaTime;
		}

		if (rotate) {
			// Rotate the object
			float angle = Tools.QuaternionToAngle(gameObject.transform.localRotation);
			angle -= 24f;
			gameObject.transform.localRotation = Tools.AngleToQuaternion(angle);

			gameObject.transform.position = transform.parent.position;
		}
	}

	public void OnTriggerEnter2D( Collider2D col ) {

		if (col.tag == "Enemy") {

			//print (col.gameObject);
			col.gameObject.SendMessage("OnHit", (PlayerAttack)this);

		} else if (col.gameObject.GetComponent<EnemyBullet>() != null) {
			// reflect enemy bullet
			EnemyBullet eb = col.gameObject.GetComponent<EnemyBullet>();
			// Delfects an incoming bullet back at an enemy
			Bullet1 reverse = (Bullet1)Instantiate(bullet_1, eb.gameObject.transform.position, Quaternion.identity);
			// Sets transfomr's parent for indicating, which player deflected the bullet
			reverse.transform.parent = this.transform.parent;
			reverse.setDuration(eb.get_duration() + 0.5f);
			reverse.setDamage( col.gameObject.GetComponent<EnemyBullet>().damage );
			// Sets bullet trajectory
			reverse.GetComponent<Rigidbody2D>().velocity = -col.gameObject.GetComponent<Rigidbody2D>().velocity;

			Destroy(col.gameObject);
		}

	}

	/* Set the slash's timer and animaiton speed to allow for the rotation animation */
	public void setRotation() {
		slashTimer += 0.25f;
		gameObject.GetComponent<Animator>().speed = 0.5f;
		rotate = true;
	}
}

