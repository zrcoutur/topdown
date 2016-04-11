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
	// Break sfx
	public AudioClip X_break;
	// Break poof
	public GameObject pf;
	// droppabble items
	public GameObject[] item_drops;
	// used to determine overlap amongst other Blocks
	public bool collision_tag;
	private int initial_durability;
	// The amount of damage a crate can sustain until it breaks
	private float durability;
	// Used to scale damage for the box
	private float timer;
	// damage done by an explosive crate
	private int damage;

	// Use this for initialization
	void Start () {
		initial_durability = UnityEngine.Random.Range(12, 28);
		durability = initial_durability;
		collision_tag = false;
		timer = 180f;
		damage = 125;
	}
	
	// Update is called once per frame
	void Update () {
		/* the HP bar shrinks as the crate's durability decreases */
		Transform hp_len = hp_bar.transform;
		var x_scale = ( durability / initial_durability );

		hp_len.localScale = new Vector3(x_scale, hp_len.localScale.y, hp_len.localScale.y);

		if (durability <= 0) {
			/* remove the crate when its durability reaches zero */
			// Play break sfx
			CameraRunner.gAudio.PlayOneShot(X_break, 0.5f);
			Instantiate(pf, transform.position, transform.rotation);
			remove_crate();
		} else if (timer <= 0f) {
			// scale damage of crate overtime
			timer = 210f;
			damage += Random.Range(15, 89);
		} else if (damage <= 12000) {
			timer -= Time.deltaTime;
		}
	}
		
	public void OnTriggerEnter2D(Collider2D trigger) {
		var collider = trigger.gameObject;
		/* Reduce durability upon coming in contact with bullets and beam swords */
		if (collider != null) {
			/* bullets and swords deal differing damage */
			if (collider.GetComponent<Bullet1>() != null || collider.GetComponent<EnemyBullet>() != null) {
				durability -= 1;
				//increment player stat
				if (collider.GetComponent<Bullet1>() != null) {
					collider.gameObject.transform.parent.GetComponent<Player>().score.boxes_hit++;
				}
			} else if (collider.GetComponent<Bullet2>() != null) {
				durability -= 5;
			} else if (collider.GetComponent<Bullet3>() != null) {
				durability -= 3;
			} else if (collider.GetComponent<Slash>() != null || collider.gameObject.GetComponent<EnemySlash>() != null) {
				durability -= 6;
			} else if (collider.GetComponent<Explosion>() != null) {
				durability -= 10;
			}
		}
	}

	public void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.tag == "Block") {
			collision_tag = true;
		}
	}

	private void remove_crate() {
		float chance = UnityEngine.Random.value;

		if (chance <= 0.05f) {
			// Create explodes
			Explosion exl = ((GameObject)Instantiate(item_drops[3], transform.position, Quaternion.identity)).GetComponent<Explosion>();
			Vector3 scale = exl.transform.localScale;
			// reduce scale of the explosion
			exl.transform.localScale = new Vector3(0.9f * scale.x, 0.9f * scale.y, scale.z);
			var dmg = (int)(damage);
			exl.setDamage(dmg);
		} else if (chance <= 0.15f) {
			// Drop a med_pack
			var d = (GameObject)Instantiate(item_drops[2], transform.localPosition, Quaternion.identity);
			d.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)));
		} else if (chance > 0.15f && chance <= 0.5f) {
			// Drop between 1 and 3 e. cores
			int drops = UnityEngine.Random.Range(1, 3);

			for (int i = 0; i < drops; ++i) {
				var d = (GameObject)Instantiate(item_drops[1], transform.localPosition, Quaternion.identity);
				d.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-80f, 80f), Random.Range(-80f, 80f)));
			}
		} else if (chance > 0.5f) {
			// Drop between 3 and 8 scrap pieces
			int drops = UnityEngine.Random.Range(3, 8);

			for (int i = 0; i < drops; ++i) {
				var d = (GameObject)Instantiate(item_drops[0], transform.localPosition, Quaternion.identity);
				d.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-80f, 80f), Random.Range(-80f, 80f)));
			}
		}

		Destroy(this.gameObject);
	}
}
