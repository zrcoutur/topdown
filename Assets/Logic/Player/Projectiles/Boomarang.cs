using UnityEngine;
using System.Collections;

/**
 * This class represents the logic for a blade that will orbit a player, dealing
 * damage to enemies and boxes nearby as well as blocking any enemy bullets they
 * come in contact with. There is a set cap to the number of blades that can spawn
 * for a player and is maintained by the blade objects themselves. Each blade has
 * a set durability that decreases every time that it blocks a bullet, hits an enemy
 * or box. Once a blades durabiltiy reaches zero, it is removed from the game.
 * 
 * author : Joshua Hooker
 * 16 Apirl 2016
 */
public class Boomarang : PlayerAttack {
	// Keeps track of the total number of blades that exist
	protected static int bladeCount;
	public static int totalBlades { get { return bladeCount; } }
	// Where the boomarang is in relation to its orbital radius
	private byte state;
	// How many hits (i.e. enemies, boxes, and bullets) the blade will last through
	private int durability;

	static Boomarang() {
		bladeCount = 0;
	}

	// Use this for initialization
	public void Start() {
		durability = 3;
		++bladeCount;
		// A blade starts close to the Player
		state = 0;
	}
	
	// Update is called once per frame
	public void Update() {
		// Remove the blade after its durability runs out
		if (durability <= 0f) { Destroy(gameObject); }

		// Rotate the object
		float angle = Tools.QuaternionToAngle(gameObject.transform.rotation);
		angle -= 26f;
		gameObject.transform.rotation = Tools.AngleToQuaternion(angle);

		// Oribit the Player at a set distance
		float dist = Vector2.Distance(transform.parent.position, transform.position);
		Vector2 dir = (transform.parent.position - transform.position).normalized;

		if (dist <= 0.2f) {
			// State 0 implies that the boomarang is near the player but outside its orbital radius
			if (state != 0) {
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				state = 0;
			}

			// Move back to rotation position
			GetComponent<Rigidbody2D>().AddForce( Tools.AngleToVec2(Tools.Vector2ToAngle(dir), -0.015f) );
		} else if (dist > 0.2f && dist <= 1.5f) {
			// State 1 implies that the boomarang is within its orbital radius
			if (state != 1) {
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				state = 1;
			}

			// Oscillate around the Player
			GetComponent<Rigidbody2D>().AddForce( Tools.AngleToVec2(Tools.Vector2ToAngle(dir) + 90f, 0.02f) );
		} else {
			// State 2 implies that the boomarang is away from the player and outside its orbital radius
			if (state != 2) {
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				state = 2;
			}

			// Move back to rotation position
			GetComponent<Rigidbody2D>().AddForce( Tools.AngleToVec2(Tools.Vector2ToAngle(dir), 0.025f) );
		}
	}

	/* The blades can block bullets and hurt nearby enemies. */
	public void OnTriggerEnter2D(Collider2D collider) {
		// Update the blade's durability counter upon hitting an enemy or blocking a bullet.
		if (collider.tag == "Enemy") {
			
			collider.gameObject.SendMessage("OnHit", (PlayerAttack)this);
			--durability;
		} else if (collider.gameObject.GetComponent<EnemyBullet>() != null) {

			Destroy(collider.gameObject);
			--durability;
		}
	}

	/* Update the blade counter when a blade is destroyed. */
	public void OnDestroy() {
		--bladeCount;
	}
}

