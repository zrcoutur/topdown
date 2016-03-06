using System;

/**
 * This class is designed to store the range of values of a particular stat.
 * 
 * @author Joshua Hooker
 * 23 February 2016
 */
public class Stat {
	// the type of stat
	public readonly STAT_TYPE type;
	// A non-empty sequence of values for the stat
	private readonly int[] values;
	private int pointer;

	/**
	 * Creates a stat of the given type
	 */
	public Stat (STAT_TYPE t, int[] vals) {
		type = t;
		values = vals;
		pointer = 0;
	}

	/* Returns the element in values pointed at by current_value. */
	public int current() { return values[pointer]; }

	/* Returns the element in values after the element referenced by
	 * current_value if one exists; -1 is returned if the current_value
	 * points to the end of values. */
	public int next() {
		return (pointer == values.Length - 1) ? -1 : values[pointer + 1];
	}

	/* If there is a next value, the pointer is incremented, else nothing changes.
	 * Returns 1 if the pointer is incremented and 0 if the pointer remains the same. */
	public int increment() {
		
		if (next() != -1) {
			++pointer;
			return 1;
		}

		return 0;
	}

	/* Returns the number of values for this stat. */
	public int values_length() { return values.Length; }
}
