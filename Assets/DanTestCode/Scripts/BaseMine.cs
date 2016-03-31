using UnityEngine;
using System.Collections;

public class BaseMine : MonoBehaviour {

	public Explosion expl;
	private int damage;
	// Used to keep track of time segments
	private float timer;
	// Used to trigger the explosion of the mine
	private bool trigger = false;

	public void Start() {
		timer = 65f;
		damage = UnityEngine.Random.Range(8, 12);
	}

	public void Update() {
		if (timer <= 0f) {
			if (trigger) { // The mine explodes
				((Explosion)Instantiate(expl, transform.position, transform.rotation)).GetComponent<Explosion>().damage = damage;
				Destroy(this.gameObject);
			} else { // Updates the damage of the mine overtime
				timer = 65f;
				damage += UnityEngine.Random.Range(0, 5);
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
		//Add similar condition for enemy
	}
}