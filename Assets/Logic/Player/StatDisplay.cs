using UnityEngine;
using System;

/**
 * This class is designed to hold the dimensions of all the rectangles
 * used to display the name, incremental button, current value, and
 * next value for some stat in a pop-up window.
 * 
 * @author Joshua Hooker
 * 23 February 2016
 */
public class StatDisplay {

	public readonly string name;
	public readonly Stat stat;
	public Rect[] labels;
	public Rect button;

	/**
	 * Creates a stat display for the given stat, with the given name.
	 */
	public StatDisplay (string n, Stat s) {
		name = n;
		stat = s;
		// initialize the dimensions of the label fields and button field
		labels = new Rect[5];
		button = new Rect(0, 0, 0, 0);
	}

	/* Changes the positions of all the rectangles based on the rectangle given. */
	public void updateRects(Rect origin) {
		//Debug.Log(origin + "\n");

		// Stat name
		labels[0] = relativeRect(origin, 2, 15, 25, 80, 22);
		// Increment button
		button = relativeRect(labels[0], 0, 10, 2, 18, 18);
		// next stat cost
		labels[1] = relativeRect(labels[0], 2, 0, 0, 200, 22);
		// current stat value
		labels[2] = relativeRect(labels[1], 2, 38, 0, 36, 22);
		// transition symbol
		labels[3] = relativeRect(labels[2], 0, 5, 0, 16, 22);
		// next stat value
		labels[4] = relativeRect(labels[3], 0, 5, 0, 36, 22);
	}

	/* Returns a new rectangle with x, y, width, and height values relative to the given rectangle.
	 * The value for corner defines which corner of the rectangle will be used for the relative
	 * origin of the returned rectangle:
	 *  	0 -> top-right, 1 -> bottom-right, 2 -> bottom-left, and 3 -> top-left.
	 *  	Any other value given for corner will result in the throwing of an error. */
	public static Rect relativeRect(Rect origin, int corner, int off_x, int off_y, int width, int height) {
		// endpoints of the origin rectangle
		float prev_x, prev_y;
		// pick a corner to offset from
		switch (corner) {
		case 0:
			prev_x = origin.x + origin.width;
			prev_y = origin.y;
			break;
		case 1:
			prev_x = origin.x + origin.width;
			prev_y = origin.y + origin.height;
			break;
		case 2:
			prev_x = origin.x;
			prev_y = origin.y + origin.height;
			break;
		case 3:
			prev_x = origin.x;
			prev_y = origin.y;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}

		return new Rect(prev_x + off_x, prev_y + off_y, width, height);
	}
}
