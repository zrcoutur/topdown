using UnityEngine;
using System.Collections;
using UnityEditor;

public class Bullet3 : PlayerAttack {

	//Poof effect
	public GameObject poof;

	public AudioClip X_Wall_Hit;
	public AudioClip X_Enemy_Hit;

	Rigidbody2D body;

	public float outset;
	float rot = 0;
	float startup = 0.125f;
	float damageApprox;
	int damageMax;

	//private int damage;

	// Use this for initialization
	void Start () {

		//	damage = 0;
		body = GetComponent<Rigidbody2D> ();

		rot = -outset;

		damageApprox = damage;
		damageMax = damage*2;

		base.hitImpulse = body.velocity * 4.0f;

	}

	// Update is called once per frame
	void Update ()
	{

		// Conflux stuff
		if (startup < 1.0f) {
			startup *= 120.0f * Time.deltaTime;
			damageApprox *= 1.0f + (30.0f * Time.deltaTime);
			damage = Mathf.Min((int)damageApprox, damageMax);
		}

		body.AddForce (Tools.AngleToVec2 (Tools.Vector2ToAngle(body.velocity)+Mathf.Clamp(rot*5.0f,-80.0f,80.0f), startup*(40.0f+Mathf.Abs(rot-outset))));
		if (rot > outset+3.0f) {
			rot -= Time.deltaTime * 300.0f;
		} else if (rot < outset-3.0f) {
			rot += Time.deltaTime * 300.0f;
		}

		// Remove the bullet after a certain period of time
		if (duration >= 0.0f) {
			duration -= Time.deltaTime;
		} else {
			Destroy( GameObject.Find("Bullet3(Clone)") );
		}
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Block") {
			// Make poof
			Instantiate (poof, transform.position, transform.rotation);

			// Play wall hit sound effect
			CameraRunner.gAudio.PlayOneShot( X_Wall_Hit );

			// Destroy self
			Destroy (gameObject);

		}

		if (col.tag == "Enemy") {
			// Tell enemy it's been hit!
			col.gameObject.SendMessage ("OnHit", (PlayerAttack)this);

			// Play hit sound effect
			CameraRunner.gAudio.PlayOneShot( X_Enemy_Hit );

			// Make poof
			Instantiate (poof, transform.position, transform.rotation);

			// Destroy
			duration = 0f;
		}

	}

	/* Sets the duration of the bullet. */
	public void set_duration(float d) { duration = d; }

}
