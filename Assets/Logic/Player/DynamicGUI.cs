﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DynamicGUI : MonoBehaviour {
	// Determines if the window is shown
	private bool show;

	private bool initialized;

	private Rect window_dimensions;
	private StatDisplay[] displays;
	
	private static GUIStyle lbl_grn_text;

	public Player p;

	public void Start() {
		show = false;

		window_dimensions = new Rect(100, 100, 100, 100);
		lbl_grn_text = null;
	}

	public void Update() {
		if (!initialized) {
			displays = new StatDisplay[5];
			// Initialize stat displays
			displays[0] = new StatDisplay("Health", p.stats.MAX_HEALTH);
			displays[1] = new StatDisplay("Shield", p.stats.MAX_SHIELD);
			switchWeaponStats();

			initialized = true;
		}

		// Toggle the display on and off
		if (Input.GetKeyDown (KeyCode.Tab)) {
			show = !show;
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
				previous = new Rect(0, 0, 0, 0);
			} else 	if (idx == 2) {
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

		Rect weapon_lbl = StatDisplay.relativeRect(displays[1].labels[1], 2, -8, 35, 48, 22);
		GUI.Label(weapon_lbl, p.stats.weapon_by_type( p.stats.current_weapon() ).type.ToString());

		// Button for switching between the stats of each weapon
		/*if ( GUI.Button( StatDisplay.relativeRect( weapon_lbl, 0, 10, 0, 48, 22), "switch") ) {
			switchWeaponStats();
		}*/
	}

	/* Draws the fields of the given stat display */
	private void drawStatDisplay(StatDisplay display) {
		// display title
		GUI.Label(display.labels[0], display.name);

		// Determins if the stat value is capped
		bool is_last = display.stat.next() == -1;
		// Determines if the player can afford the next upgrade
		Stat_Cost for_next = display.stat.next_cost();
		bool can_buy = for_next == null || ( for_next.scrap_cost <= p.stats.get_scrap() && for_next.ecore_cost <= p.stats.get_ecores() );

		GUI.enabled = !is_last && can_buy;
		// Create button to increment the pointer
		if ( GUI.Button(display.button, "+") && GUI.enabled ) {
			display.stat.increment();

			// Subtract cost from player stats
			if (for_next != null) {
				p.stats.change_scrap(-for_next.scrap_cost);
				p.stats.change_ecores(-for_next.ecore_cost);
			}

			// Indicate that the max values of either health or shield changed, so that sliders will update
			if (display.stat.type == STAT_TYPE.health) {
				p.stats.HP_raised = true;
			} else if (display.stat.type == STAT_TYPE.shield) {
				p.stats.Shield_raised = true;
			}
		}
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

		displays[2] = new StatDisplay( "Damage", current.weapon_stat(STAT_TYPE.damage) );
		displays[3] = new StatDisplay( "Rate of Fire", current.weapon_stat(STAT_TYPE.rate_of_fire) );
		displays[4] = new StatDisplay( "Ammo Cost", current.weapon_stat(STAT_TYPE.ammo) );

		updatePositions();
	}

	/* Determines if the window is open. */
	public bool isOpen() { return show; }
}
