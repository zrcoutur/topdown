using UnityEngine;
using System.Collections;

/**
 * This class defines the behaviours of breakable crates in the game.
 * Each crate has a set amount of durability which can only be reduced by the Player's bullets and sword currently.
 * 
 * @author Joshua Hooker
 * 
 * 1 February 2016
 */
public class BreakableCrate : MonoBehaviour {
	// Box's HP bar
	public GameObject hp_bar;
	// droppabble items
	public GameObject energy_core;
	public GameObject scrap;
	// used to determine overlap amongst other Blocks
	public bool collision_tag;
	// initial durability of a crate
	private static readonly int MAX_DURABILITY = 21;
	// The amount of damage a crate can sustain until it breaks
	private float durability;

	// Use this for initialization
	void Start () {
		durability = MAX_DURABILITY;
		collision_tag = false;
	}
	
	// Update is called once per frame
	void Update () {
		/* the HP bar shrinks as the crate's durability decreases */
		Transform hp_len = hp_bar.transform;
		var x_scale = ( durability / MAX_DURABILITY );

		hp_len.localScale = new Vector3(x_scale, hp_len.localScale.y, hp_len.localScale.y);

		/* remove the crate when its durability reaches zero */
		if (durability <= 0) {
			remove_crate();
		}
	}
		
	public void OnTriggerEnter2D(Collider2D trigger) {
		var collider = trigger.gameObject;
		/* Reduce durability upon coming in contact with bullets and beam swords */
		if (collider != null) {
			/* bullets and swords deal differing damage */
			if (collider.tag == "bullet_1") {
				durability -= 1;
				//increment player stat
				collider.gameObject.transform.parent.GetComponent<Player>().score.boxes_hit++;
			} else if (collider.tag == "sword") {
				durability -= 8;
			}
		}
	}

	public void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.tag == "Block") {
			collision_tag = true;
		}
		else if (col.gameObject.tag == "Enemy")
		{
			durability -= 1;
		}

	}

	private void remove_crate() {
		float chance = UnityEngine.Random.value;

		// Possibly drop an e. core
		if (chance >= 0.03f && chance <= 0.37f) {
			var d = (GameObject)Instantiate(energy_core, transform.localPosition, Quaternion.identity);
			d.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200f, 200f), Random.Range(-200f, 200f)));
		} else if (chance > 0.37f) {
			// Drop between 0 and 10 scrap pieces
			int drops = UnityEngine.Random.Range(3, 8);

			for (int i = 0; i < drops; ++i) {
					var d = (GameObject)Instantiate(scrap, transform.localPosition, Quaternion.identity);
					d.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-350f, 350f), Random.Range(-350f, 350f)));
			}
		}

		Destroy(this.gameObject);
	}
}
