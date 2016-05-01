using UnityEngine;
using System.Collections;

public class Bullet2 : PlayerAttack {

	//Poof effect
	public GameObject poof;

	public AudioClip X_Wall_Hit;
	public AudioClip X_Enemy_Hit;

	Rigidbody2D body;

	// number of enemies that can be pierced
	int life;

	//private int damage;

	// Use this for initialization
	void Start () {
		//	damage = 0;
		body = GetComponent<Rigidbody2D> ();

		life = 5;

		float angle = 270.0f + Tools.Vector2ToAngle (body.velocity);

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		base.hitImpulse = body.velocity * 6.0f;

	}

	// Update is called once per frame
	void Update () {

		if (!Time_Count.game_pause) {
			
			// Remove the bullet after a certain period of time
			if (duration >= 0.0f) {
				duration -= Time.deltaTime;
			} else {
				Destroy(GameObject.Find("Bullet2(Clone)"));
			}

			if (body.drag > 0)
				body.drag /= 2;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (col.gameObject.GetComponent<BreakableCrate>() == null && col.tag == "Block") {
			// Make poof
			Instantiate (poof, transform.position, transform.rotation);

			// Play wall hit sound effect
			CameraRunner.gAudio.PlayOneShot( X_Wall_Hit );

			// Destroy self
			Destroy (gameObject);

		}

		if (col.tag == "Enemy") {
			
			// Tell enemy it's been hit!
			col.gameObject.SendMessage("OnHit", (PlayerAttack)this);

			// Play hit sound effect
			CameraRunner.gAudio.PlayOneShot(X_Enemy_Hit);

			// Make poof
			Instantiate(poof, transform.position, transform.rotation);

			// Decrement life, speed and damage
			life -= 1;
			damage /= 2;
			body.drag = 2.0f;

			// Destroy self
			if (life <= 0)
				duration = 0f;
		} else if (col.gameObject.GetComponent<BreakableCrate>() != null) {
			// Decrement life, speed and damage
			life -= 1;
			damage /= 2;
			body.drag = 2.0f;

			// Destroy self
			if (life <= 0)
				duration = 0f;
		}

	}
}
