using UnityEngine;
using System.Collections;

public class Bullet1 : MonoBehaviour {

	//Poof effect
	public GameObject poof;
    int damage;

	// Use this for initialization
	void Start () {

		body = GetComponent<Rigidbody2D> ();

		float angle = 270.0f + Tools.Vector2ToAngle (body.velocity);

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        damage = 2;

	}

	Rigidbody2D body;
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if (col.tag == "Block") {
			Instantiate (poof, transform.position, transform.rotation);
			Destroy (gameObject);

		}

		if (col.tag == "Enemy") {
			col.gameObject.SendMessage ("OnHit", body.velocity * 2.0f);
			Instantiate (poof, transform.position, transform.rotation);
            col.GetComponent<Baseenemy>().health -= damage;
			Destroy (gameObject);
		}

	}
}
