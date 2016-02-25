using UnityEngine;
using System;
using System.Collections;

public class DynamicGUI : MonoBehaviour {
	// Determines if the window is shown
	private bool show;
	private Rect window_dimensions;
	private StatDisplay[] displays;
	// Determines which weapon stats to show
	private WEAPON_TYPE weapon;

	private static GUIStyle lbl_grn_text;

	public void Start() {
		show = false;

		window_dimensions = new Rect(100, 100, 100, 100);
		displays = new StatDisplay[5];
		// Initialize stat displays
		displays[0] = new StatDisplay("Health", Storage.MAX_HEALTH);
		displays[1] = new StatDisplay("Shield", Storage.MAX_SHIELD);
		weapon = WEAPON_TYPE.grenade;
		switchWeaponStats();

		lbl_grn_text = null;
	}

	public void Update() {
		// Toggle the display on and off
		if (Input.GetKeyDown (KeyCode.Tab)) { show = !show; }
	}
		
	public void OnGUI() {
		// set green-italic text style
		if (lbl_grn_text == null) {
			// center text in labels
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			// center window title
			GUI.skin.window.alignment = TextAnchor.UpperCenter;

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
				updatePositions();
			}

			GUI.Window(0, window_dimensions, drawContents, "Stats");
		}
	}

	/* Resizes the window based on the size of the screen */
	private Rect resizeWindow(Rect original) {
		return new Rect(Screen.width / 10, Screen.height / 20, Screen.width / 5, 18 * Screen.height / 20);
	}

	/* Updates the positions of all the stat displays in the window */
	public void updatePositions() {
		for (int idx = 0; idx < displays.Length; ++idx) {
			Rect previous;

			if (idx == 0) { // position based on the window display
				previous = new Rect(window_dimensions.x, window_dimensions.y, 0, 0);
			} else 	if (idx == 2) {
				previous = new Rect(window_dimensions.x, window_dimensions.y, 0, displays[idx - 1].labels[3].y + displays[idx - 1].labels[3].height + 37);
			} else { // position a stat display based on the previous stat display
				previous = new Rect(window_dimensions.x, window_dimensions.y, 0, displays[idx - 1].labels[3].y + displays[idx - 1].labels[3].height);
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

		Rect weapon_lbl = StatDisplay.relativeRect(displays[1].labels[1], 2, 0, 30, 48, 22);
		GUI.Label(weapon_lbl, Storage.weapon_by_type(weapon).type.ToString());

		// Button for switching between the stats of each weapon
		if ( GUI.Button( StatDisplay.relativeRect( weapon_lbl, 0, 10, 0, 48, 22), "switch") ) {
			switchWeaponStats();
		}
	}

	/* Draws the fields of the given stat display */
	private void drawStatDisplay(StatDisplay display) {
		// display title
		GUI.Label(display.labels[0], display.name);

		// disable the button once pointer reaches the last stat value
		bool is_last = display.stat.next() == -1;

		GUI.enabled = !is_last;
		// create button to increment the pointer
		if ( GUI.Button(display.button, "+") && !is_last ) {
			display.stat.increment();
		}
		GUI.enabled = true;
		
		// show current value
		GUI.Label(display.labels[1], "" + display.stat.current());
		// display arrows
		GUI.Label(display.labels[2], ">>");
		// show next value (or '--' if no such element exists)
		GUI.Label(display.labels[3], (is_last) ? "--" : "" + display.stat.next(), lbl_grn_text);
	}

	/* Switches the display of the current weapon stats to the next weapon in the list. */
	private void switchWeaponStats() {
		weapon = (WEAPON_TYPE) ( ((byte)weapon + 1) % 4 );

		displays[2] = new StatDisplay( "Damage", Storage.weapon_by_type(weapon).stat_by_type(STAT_TYPE.damage) );
		displays[3] = new StatDisplay( "Rate of Fire", Storage.weapon_by_type(weapon).stat_by_type(STAT_TYPE.rate_of_fire) );
		displays[4] = new StatDisplay( "Ammo Cost", Storage.weapon_by_type(weapon).stat_by_type(STAT_TYPE.ammo) );
		updatePositions();
	}

	/* Determines if the window is open. */
	public bool isOpen() { return show; }
}
