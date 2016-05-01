using System;
using UnityEngine;

/**
 * This classd defines the behavoir of any items that enemies drop and players pick up. Each item
 * has a set duration time at which point the item disappears if it is not collected by a player.
 * Items are also drawn to player's within a certain range of the item. Items ignore collisions
 * with any other objects such as enemies, explosions, bullets, etc.
 * 
 * author : Joshua Hooker
 */
public class Item_Drop : MonoBehaviour {

	public Shine shine;
	// Sound played when grabbed
	public AudioClip getSound;
	// List of players
	private Player[] players;
	// Used to accelerate the item overtime
	private float time_near;
	/* How long the item will stay in the game before disappearing */
	private float duration;

	public void Start() {
		// Find all players
		players = FindObjectsOfType<Player>();
		time_near = 0f;
		// Initialize timer
		duration = 8f * (UnityEngine.Random.Range(75, 250) / 100f);
	}

	public void Update() {

		if (!Time_Count.game_pause) {
			
			Player closest = null;

			/* Finds the closest player and moves the item nearer to that player. */
			for (int idx = 0; idx < players.Length; ++idx) {
				float dist_x = transform.localPosition.x - players[idx].transform.localPosition.x;
				float dist_y = transform.localPosition.y - players[idx].transform.localPosition.y;
				// draw item closer to the player
				if (System.Math.Abs(dist_x) <= 3f && System.Math.Abs(dist_y) <= 3f) {
					closest = players[idx];
					break;
				}
			}

			// Accelerate item to nearby player
			if (closest != null) {
				float dist_x = transform.localPosition.x - closest.transform.localPosition.x;
				float dist_y = transform.localPosition.y - closest.transform.localPosition.y;

				time_near += Time.deltaTime;
				GetComponent<Rigidbody2D>().AddForce(40f * Mathf.Pow(time_near + 1, 2.0f) * (new Vector2(-dist_x, -dist_y)));
			} else {
				//  Reset acceleration
				time_near = 0f;
			}

			if (duration >= 0) { // Decrement timer
				duration -= Time.deltaTime;
			} else { // Remove item from the game
				// Shine effect
				Instantiate(shine, transform.position, Quaternion.Euler(0, 0, 0));
				Destroy(this.gameObject);
			}
		}
	}

	/* Sets the duration of this item to the given value. */
	public void set_duration(float value) { duration = value; }
}
