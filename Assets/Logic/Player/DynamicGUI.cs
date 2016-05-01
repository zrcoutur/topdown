using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DynamicGUI : MonoBehaviour {
	// Determines if the window is shown
	private bool show;
	// Window dimensions
	private Rect window_dimensions;
	// Dimensions of each of the contents of window
	private StatDisplay[] displays;

	private static GUIStyle lbl_grn_text;

	private Player p;

	public void Start() {
		show = false;
		// Get a reference to the Player associated with this window
		p = GetComponentInParent<Player>();
		window_dimensions = new Rect(100, 100, 100, 100);
		lbl_grn_text = null;
	}

	public void Update() {

		displays = new StatDisplay[6];
		// Initialize stat displays
		displays[0] = new StatDisplay("Health", p.stats.MAX_HEALTH);
		displays[1] = new StatDisplay("Shield", p.stats.MAX_SHIELD);
		displays[2] = new StatDisplay("Medpacks", p.stats.MEDPACKS);
		switchWeaponStats();

		// Toggle the display on and off
		if (Input.GetKeyDown(KeyCode.Tab)) {
			show = !show;
			Time_Count.toggle_pause();
		}
	}
		
	public void OnGUI() {
		
		if (lbl_grn_text == null) {
			// center text in labels
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			// center window title
			GUI.skin.window.alignment = TextAnchor.UpperCenter;
			// set green-italic text style
			lbl_grn_text = new GUIStyle(GUI.skin.label);
			lbl_grn_text.normal.textColor = Color.green;
			lbl_grn_text.fontStyle = FontStyle.Italic;
		}

		// Draws the window if show is true
		if (show) {
			var limbo = new Rect(window_dimensions.x, window_dimensions.y, window_dimensions.width, window_dimensions.height);
			// resize the window based on screen size
			window_dimensions = resizeWindow(window_dimensions);
			// reposition the stat displays if need be
			if (!Rect.Equals(limbo, window_dimensions)) {
				//Debug.Log(window_dimensions);
				updatePositions();
			}

			GUI.color = Color.black;
			GUI.contentColor = Color.white;
			GUI.Window(0, window_dimensions, drawContents, "Stats");
		}
	}

	/* Resizes the window based on the size of the screen */
	private Rect resizeWindow(Rect original) {
		return new Rect(13 * Screen.width / 20, Screen.height / 30, 3 * Screen.width / 10, 28 * Screen.height / 30);
	}

	/* Updates the positions of all the stat displays in the window */
	public void updatePositions() {
		for (int idx = 0; idx < displays.Length; ++idx) {
			Rect previous;

			if (idx == 0) { // position based on the window display
				previous = new Rect(0, 0, 0, 10);
			} else if (idx == 3) {
				previous = new Rect(0, 0, 0, displays[idx - 1].labels[3].y + displays[idx - 1].labels[3].height + 30);
			} else { // position a stat display based on the previous stat display
				previous = new Rect(0, 0, 0, displays[idx - 1].labels[3].y + displays[idx - 1].labels[3].height);
			}

			displays[idx].updateRects(previous);
		}
	}

	/* Creates the contents of the window */
	public void drawContents(int ID) {
		/* Draw each stat display */
		foreach (StatDisplay sd in displays) {
			drawStatDisplay(sd);
		}
		// Prints the name of the current weapon
		Rect weapon_lbl = StatDisplay.relativeRect(displays[2].labels[1], 2, -8, 35, 48, 22);
		GUI.Label(weapon_lbl, p.stats.weapon_by_type( p.stats.current_weapon() ).type.ToString());
	}

	/* Draws the fields of the given stat display */
	private void drawStatDisplay(StatDisplay display) {
		// display title
		GUI.Label(display.labels[0], display.name + " (" + display.stat.pointer_value() + ")");

		// Determines if the stat value is capped
		bool is_last = display.stat.next() == -1;
		// Determines if the player can afford the next upgrade
		Stat_Cost for_next = display.stat.next_cost();
		bool can_buy = for_next == null || ( for_next.scrap_cost <= p.stats.get_scrap() && for_next.ecore_cost <= p.stats.get_ecores() );

		GUI.enabled = !is_last && can_buy;

		if (GUI.enabled) {
			// Text color green indicates that a stat can be upgraded currently
			GUI.skin.button.normal.textColor = Color.green;
			GUI.skin.button.fontStyle = FontStyle.Bold;
		} else if (!is_last) {
			GUI.skin.button.normal.textColor = Color.red;
		}

		// Create button to increment the pointer
		if ( GUI.Button(display.buttons[0], "+") && GUI.enabled ) {
			int origin = 0;
			// Store the original value of the player's health
			if (display.stat.type == STAT_TYPE.health) {
				origin = (int)p.stats.MAX_HEALTH.current();
			}

			display.stat.increment();

			// Subtract cost from player stats
			if (for_next != null) {
				p.stats.change_scrap(-for_next.scrap_cost);
				p.stats.change_ecores(-for_next.ecore_cost);
			}

			// Indicate that the max values of either health or shield changed, so that sliders will update
			if (display.stat.type == STAT_TYPE.health) {
				// Restore health equal to the change in health
				p.stats.change_health( (int)p.stats.MAX_HEALTH.current() - origin );
				p.stats.HP_raised = true;
			} else if (display.stat.type == STAT_TYPE.shield) {
				// Fully restore shield
				p.stats.change_shield( (int)p.stats.MAX_SHIELD.current() );
				p.stats.Shield_raised = true;
			} else {
				// Updates the player's weapon if necessary
				p.updateWeapons();
			}
		}

		GUI.skin.button.normal.textColor = Color.white;
		GUI.skin.button.fontStyle = FontStyle.Normal;

		/* Do NOT uncomment!!
		 
		GUI.enabled = display.stat.pointer_value() > 0;
		// Decrement a stat value and return the cost of the upgrade
		if (GUI.Button(display.buttons[1], "-") && GUI.enabled) {
			display.stat.decrement();

			// Readd cost of the upgrade to player stats
			if (display.stat.next_cost() != null) {
				p.stats.change_scrap(display.stat.next_cost().scrap_cost);
				p.stats.change_ecores(display.stat.next_cost().ecore_cost);
			}

			// Indicate that the max values of either health or shield changed, so that sliders will update
			if (display.stat.type == STAT_TYPE.health) {
				p.stats.HP_raised = true;
			} else if (display.stat.type == STAT_TYPE.shield) {
				p.stats.Shield_raised = true;
			}
		}*/

		GUI.enabled = true;
		// show cost for next upgrade
		GUI.Label(display.labels[1], "cost (scrap | e. cores): " + ( (for_next == null) ? "-- | --" : (for_next.scrap_cost + " | " + for_next.ecore_cost) ) );
		// show current value
		GUI.Label(display.labels[2], "" + display.stat.current());
		// display arrows
		GUI.Label(display.labels[3], ">>");
		// show next value (or '--' if no such element exists)
		GUI.Label(display.labels[4], (is_last) ? "--" : "" + display.stat.next(), lbl_grn_text);
	}

	/* Switches the display of the current weapon stats to the next weapon in the list. */
	public void switchWeaponStats() {
		WeaponStats current = p.stats.weapon_by_type( p.stats.current_weapon() );

		displays[3] = new StatDisplay( "Damage", current.weapon_stat(STAT_TYPE.damage) );
		displays[4] = new StatDisplay( "Rate of Fire", current.weapon_stat(STAT_TYPE.rate_of_fire) );
		displays[5] = new StatDisplay( "Ammo Cost", current.weapon_stat(STAT_TYPE.ammo) );

		updatePositions();
	}

	/* Determines if the window is open. */
	public bool isOpen() { return show; }
}
