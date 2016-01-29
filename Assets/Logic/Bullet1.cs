using UnityEngine;
using System.Collections;

public class Bullet1 : MonoBehaviour {

	// Use this for initialization
	void Start () {

		body = GetComponent<Rigidbody2D> ();

		float angle = 270.0f + Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	}

	Rigidbody2D body;
	
	// Update is called once per frame
	void Update () {

	}
		
	void OnTriggerEnter2D(Collider2D col) {

		// Check for wall collision
		if (col.gameObject.tag == "Block") {
			
			Destroy (gameObject);

		}

	}
}
