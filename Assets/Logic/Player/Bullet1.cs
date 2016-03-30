using UnityEngine;
using System.Collections;
using System.Threading;

public class Bullet1 : PlayerAttack {

	//Poof effect
	public GameObject poof;

	public AudioClip X_Wall_Hit;
	public AudioClip X_Enemy_Hit;

	// duration of the bullet (not in seconds)
	float duration = 1.0f;

	public int twin = 0;
	public int osc = 0;
	float otime = 0;

	//private int damage;

	// Use this for initialization
	void Start () {
	//	damage = 0;
		body = GetComponent<Rigidbody2D> ();

		float angle = 270.0f + Tools.Vector2ToAngle (body.velocity);

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		base.hitImpulse = body.velocity * 2.0f;

	}

	Rigidbody2D body;
	
	// Update is called once per frame
	void Update () {

		//Twin intertwining
		if (twin == 1) {
			otime += Time.deltaTime;
			body.AddForce(Tools.AngleToVec2(Tools.Vector2ToAngle(body.velocity)+90.0f,Mathf.Cos(otime*15.0f)*50.0f*osc));
		}

		// Remove the bullet after a certain period of time
		if (duration >= 0.0f) {
			duration -= Time.deltaTime;
		} else {
			Destroy( GameObject.Find("Bullet1(Clone)") );
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

			// Destroy self
			Destroy (gameObject);
		}

	}

	/* Sets the duration of the bullet. */
	public void set_duration(float d) { duration = d; }

}
