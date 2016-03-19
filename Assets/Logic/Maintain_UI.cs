using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * A class designed to maintain the player's health, shield, and item counter displays.
 * 
 * @author Joshua Hooker
 * 15 March 2016
 */
public class Maintain_UI : MonoBehaviour {

	private Slider health;
	private Slider shield;

	private Text[] item_counters;

	public Player p;

	// Use this for initialization
	void Start () {
		p = FindObjectOfType<Player>();

		// Find sliders in children objects
		Slider[] sliders = GetComponentsInChildren<Slider>();

		// Expecting two sliders to exist: one for shield and the other for health
		if (sliders[0].name == "HealthSlider") {
			health = sliders[0];
			shield = sliders[1];
		} else {
			health = sliders[1];
			shield = sliders[0];
		}

		item_counters = gameObject.GetComponentsInChildren<Text>();

		update_ui();
	}
	
	// Update is called once per frame
	void Update () {
		update_ui();
	}

	/* Update the player's max health or max shield to the current maximum value */
	private void update_ui() {
		if (p.stats != null) {
			// Sync health slider's max value to the Player's current max health
			if (p.stats.HP_raised) {
				health.maxValue = p.stats.MAX_HEALTH.current();
				p.stats.HP_raised = false;
			}
			// Sync shield slider's max value to Player's current max shield
			if (p.stats.Shield_raised) {
				shield.maxValue = p.stats.MAX_SHIELD.current();
				p.stats.Shield_raised = false;
			}

			// Update scrap and e. core displays
			item_counters[0].text = "" + p.stats.get_scrap();
			item_counters[1].text = "" + p.stats.get_ecores();
			item_counters[2].text = "" + p.stats.MEDPACKS.current();
			item_counters[3].text = "" + p.score.totalScore;
			
		}
	}
}
