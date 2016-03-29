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
		timer = 1.05f;
		damage = UnityEngine.Random.Range(6, 10);
	}

	public void Update() {
		if (timer <= 0f) {
			if (trigger) { // The mine explodes
				((Explosion)Instantiate(expl, transform.position, transform.rotation)).GetComponent<Explosion>().damage = damage;
				Destroy(this.gameObject);
			} else { // Updates the damage of the mine overtime
				timer = 1.05f;
				damage += UnityEngine.Random.Range(0, 8);
			}
		} else {
			timer -= Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		// The mine has been triggered by something
		if (!trigger && coll.GetComponent<Player>() != null) {
			trigger = true;
			// Starts flashing animation to indicate the mine is triggered
			this.GetComponent<Animator>().SetTrigger("mine_set");
			// sets delay time between 1/2 ~ 2 seconds
			timer = UnityEngine.Random.Range(50, 200) / 100f;
		}
		//Add similar condition for enemy
	}
}