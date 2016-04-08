using UnityEngine;
using System.Collections;
using System.Threading;

public class EnemyBullet : MonoBehaviour {

	//Poof effect
	public GameObject poof;

	public AudioClip X_Wall_Hit;
	public AudioClip X_Enemy_Hit;

	public int damage;

	// duration of the bullet (not in seconds)
	float duration = 2f;

	// Use this for initialization
	void Start () {

		body = GetComponent<Rigidbody2D> ();

		float angle = 270.0f + Tools.Vector2ToAngle (body.velocity);

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	}

	Rigidbody2D body;
	
	// Update is called once per frame
	void Update () {

		// Remove the bullet after a certain period of time
		if (duration >= 0.0f) {
			duration -= Time.deltaTime;
		} else {
			Destroy(gameObject);
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

		if (col.tag == "Player") {
			// Tell enemy it's been hit!
			col.gameObject.SendMessage ("GetHurt", damage);

			// Make poof
			Instantiate (poof, transform.position, transform.rotation);

			// Destroy self
			Destroy (gameObject);
		}
	}

	/* Sets the duration of the bullet. */
	public void set_duration(float d) { duration = d; }

	/* Returns the remaininf duration of the bullet. */
	public float get_duration() { return duration; }
}
