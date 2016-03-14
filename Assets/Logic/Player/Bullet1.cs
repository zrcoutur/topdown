using UnityEngine;
using System.Collections;

public class Bullet1 : PlayerAttack {

	//Poof effect
	public GameObject poof;
	//public GameObject bullet;
	// duration of the bullet (not in seconds)
	float duration = 1.0f;

	private int damage;

	// Use this for initialization
	void Start () {
		damage = 0;
		body = GetComponent<Rigidbody2D> ();

		float angle = 270.0f + Tools.Vector2ToAngle (body.velocity);

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		base.hitImpulse = body.velocity * 2.0f;

	}

	Rigidbody2D body;
	
	// Update is called once per frame
	void Update () {
		// Remove the bullet after a certain period of time
		if (duration >= 0.0f) {
			duration -= Time.deltaTime;
		} else {
			Destroy( GameObject.Find("Bullet1(Clone)") );
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if (col.tag == "Block") {
			Instantiate (poof, transform.position, transform.rotation);
			Destroy (gameObject);

		}

		if (col.tag == "Enemy") {
            col.GetComponentInParent<Baseenemy>().health -= damage;
            col.gameObject.SendMessage ("OnHit", body.velocity * 2.0f);
			Instantiate (poof, transform.position, transform.rotation);
			Destroy (gameObject);
		}

	}

	/* Set the damage of the bullet */
	public void set_damage(int dmg) { damage = dmg; }

	/* Return the bullet's damage */
	public int get_damage() { return damage; }
}
