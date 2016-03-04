using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * A class designed to update the maximum values for the Player's health and shield sliders.
 * 
 * @author Joshua Hooker
 * 29 February 2016
 */
public class Slider_Sync_Value : MonoBehaviour {

	private Slider health;
	private Slider shield;

	void Start() {
		// Find sliders in children objects
		Slider[] sliders = GetComponentsInChildren<Slider>();
		//Debug.Log(sliders.Length);

		// Expecting two sliders to exist: one for shield and the other for health
		if (sliders[0].name == "HealthSlider") {
			health = sliders[0];
			shield = sliders[1];
		} else {
			health = sliders[1];
			shield = sliders[0];
		}

		update_max_values();
	}

	void Update() {
		update_max_values();
	}

	/* Update the player's max health or max shield to the current maximum value */
	private void update_max_values() {
		// Sync health slider's max value to the Player's current max health
		if (Storage.HP_raised) {
			health.maxValue = Storage.MAX_HEALTH.current();
			Storage.HP_raised = false;
		}
		// Sync shield slider's max value to Player's current max shield
		if (Storage.Shield_raised) {
			shield.maxValue = Storage.MAX_SHIELD.current();
			Storage.Shield_raised = false;
		}
	}
}

