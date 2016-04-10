using UnityEngine;
using System.Collections;

/**
 * A class which maintains the attributes of simple landmine. Mines are only triggered by
 * the player and other explosions. Although, mines will damages both enemies and boxes
 * in addition to players.
 * 
 * last modified by : Joshua Hooker
 * 31 March 2016
 */
public class BaseMine : MonoBehaviour {
	// The mine's explosion
	public GameObject expl;
	// The damage of the mine
	private int damage;
	// Used to keep track of time segments
	private float timer;
	// Used to trigger the explosion of the mine
	private bool trigger = false;

	public void Start() {
		timer = 180f;
		damage = UnityEngine.Random.Range(12, 23);
	}

	public void Update() {
		if (timer <= 0f) {
			if (trigger) { // The mine explodes
				Explosion exl = ((GameObject)Instantiate(expl, transform.position, transform.rotation)).GetComponent<Explosion>();
				Vector3 scale = exl.transform.localScale;
				// reduce scale of the explosion
				exl.transform.localScale = new Vector3(1.75f * scale.x, 1.75f * scale.y, scale.z);
				exl.setDamage(damage);
				Destroy(this.gameObject);
			} else { // Updates the damage of the mine overtime
				timer = 180f;
				damage += UnityEngine.Random.Range(25, 50);
			}
		} else {
			timer -= Time.deltaTime;
		}
	}

	public void OnTriggerEnter2D(Collider2D trig) {
		// The mine has been triggered by something
		if (trig.gameObject.GetComponent<Player>() != null || trig.gameObject.GetComponent<Explosion>() != null) {
			// Starts flashing animation to indicate the mine is triggered
			this.GetComponent<Animator>().SetTrigger("mine_set");

			if (!trigger) {
				trigger = true;
				// sets delay time between 1/2 ~ 2 seconds
				timer = UnityEngine.Random.Range(10, 250) / 100f;
			} else { // if the mine is already detonated then the timer is reduced by an explosion
				timer -= 0.9f;
			}
		}
		//Add similar condition for enemy
	}
}