using System;
using UnityEngine;

public class Item_Drop : MonoBehaviour {

	public Shine shine;
	// List of players
	private Player[] players;
	/* How long the item will stay in the game before disappearing */
	private float duration;

	public void Start() {
		// Find all players
		players = FindObjectsOfType<Player>();
		// Initialize timer
		duration = 5f * (UnityEngine.Random.Range(80, 160) / 100f);
	}

	public void Update() {
		/* Finds the closest player and moves the item nearer to that player. */
		for (int idx = 0; idx < players.Length; ++idx) {
			float dist_x = transform.localPosition.x - players[idx].transform.localPosition.x;
			float dist_y = transform.localPosition.y - players[idx].transform.localPosition.y;

			if (System.Math.Abs(dist_x) <= 2.5f && System.Math.Abs(dist_y) <= 2.5f) {
				GetComponent<Rigidbody2D>().AddForce( new Vector2(-15f * dist_x, -15f * dist_y) );
			}
		}

		if (duration >= 0) { // Decrement timer
			duration -= Time.deltaTime;
		} else { // Remove item from the game
			// Shine effect
			Instantiate(shine, gameObject.transform.position, Quaternion.Euler (0, 0, 0));
			Destroy(this.gameObject);
		}
	}
}
