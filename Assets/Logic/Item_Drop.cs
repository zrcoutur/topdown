using System;
using UnityEngine;

public class Item_Drop : MonoBehaviour {

	public Shine shine;
	/* How long the item will stay in the game before disappearing */
	private float duration;

	public void Start() {
		// Initialize timer
		duration = 7f * (UnityEngine.Random.Range(65, 135) / 100f);
	}

	public void Update() {
		if (duration >= 0) { // Decrement timer
			duration -= Time.deltaTime;
		} else { // Remove item from the game
			// Shine effect
			Instantiate(shine, gameObject.transform.position, Quaternion.Euler (0, 0, 0));

			Destroy(this.gameObject);
		}
	}
}
