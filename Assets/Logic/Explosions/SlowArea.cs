using UnityEngine;
using System.Collections;

/**
 * This class defines the behavoir of an area in the game that slows enemies
 * and damages them overtime. The area has a set duration for which it exists
 * in the game.
 * 
 * author : Joshua Hooker
 * 31 March 2016
 */
public class SlowArea : MonoBehaviour {
	// How long the area lasts for
	private float duration = float.MaxValue;
	
	// Update is called once per frame
	public void Update() {

		if (!Time_Count.game_pause) {
			
			// remove the slow area after a period of time
			if (duration <= 0f) {
				Destroy(this.gameObject);
			} else {
				duration -= Time.deltaTime;
			}
		
		}
	}

	// getter and setter for duration field
	public void setDuration(float length) { duration = length; }
	public float getDuration() { return duration; }
}

