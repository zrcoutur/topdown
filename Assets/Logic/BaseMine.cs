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
		timer = 105f;
		damage = UnityEngine.Random.Range(5, 9);
	}

	public void Update() {
		if (timer <= 0f) {
			if (trigger) { // The mine explodes
				Explosion exl = ((GameObject)Instantiate(expl, transform.position, transform.rotation)).GetComponent<Explosion>();
				exl.setDamage(damage);
				Destroy(this.gameObject);
			} else { // Updates the damage of the mine overtime
				timer = 105f;
				damage += UnityEngine.Random.Range(10, 35);
			}
		} else {
			timer -= Time.deltaTime;
		}
	}

	public void OnTriggerEnter2D(Collider2D trig) {
		// The mine has been triggered by something
		if (!trigger && trig.gameObject.GetComponent<Player>() != null) {
			trigger = true;
			// Starts flashing animation to indicate the mine is triggered
			this.GetComponent<Animator>().SetTrigger("mine_set");
			// sets delay time between 1/2 ~ 2 seconds
			timer = UnityEngine.Random.Range(10, 250) / 100f;
		}

		if (trig.gameObject.GetComponent<Explosion>() != null) {
			Debug.Log("Explosion");
			// immediately denote this mine if it is within the range of another explosion
			trigger = true;
			this.GetComponent<Animator>().SetTrigger("mine_set");
			timer = 0f;
		}
		//Add similar condition for enemy
	}
}