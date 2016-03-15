using System;
using UnityEngine;

public class Item_Drop : MonoBehaviour {
	/* How long the item will stay in the game before disappearing */
	private float duration;

	public void Start() {
		// Initialize timer
		duration = 4.5f;
	}

	public void Update() {
		if (duration >= 0) { // Decrement timer
			duration -= Time.deltaTime;
		} else { // Remove item from the game
			Destroy(this.gameObject);
		}
	}
}
