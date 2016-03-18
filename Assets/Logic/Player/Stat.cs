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
	private readonly float[] values;
	// The costs of each next respective stat upgrade in values
	private readonly Stat_Cost[] costs;
	private int pointer;

	/**
	 * Creates a stat of the given type, without any stat costs.
	 */
	public Stat (STAT_TYPE t, int[] vals) {
		type = t;
		values = new float[vals.Length];

		for (int idx = 0; idx < vals.Length; ++idx) {
			values[idx] = vals[idx];
		}

		pointer = 0;
		costs = null;
	}

	/**
	 * Creates a stat of the given type with the given costs;
	 * The sc array should have one less value than the vals array! 
	 */
	public Stat (STAT_TYPE t, int[] vals, Stat_Cost[] sc) {
		type = t;
		values = new float[vals.Length];

		for (int idx = 0; idx < vals.Length; ++idx) {
			values[idx] = vals[idx];
		}
		pointer = 0;
		costs = sc;
	}

	/**
	 * Creates a stat of the given type with the given costs;
	 * The sc array should have one less value than the vals array! 
	 */
	public Stat (STAT_TYPE t, float[] vals, Stat_Cost[] sc) {
		type = t;
		values = vals;
		pointer = 0;
		costs = sc;
	}

	/* Returns the element in values pointed at by current_value. */
	public float current() { return values[pointer]; }

	/* Returns the element in values after the element referenced by
	 * current_value if one exists; -1 is returned if the current_value
	 * points to the end of values. */
	public float next() {
		return (pointer >= values.Length - 1) ? -1 : values[pointer + 1];
	}

	/* Returns the cost of the upgrading to the next stat value;
	 * if there is no next stat, null is returned. */
	public Stat_Cost next_cost() {
		return (costs == null || pointer >= costs.Length) ? null : costs[pointer];
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

	/* Returns the number of pairs of costs for each value. */
	public int costs_length() { return (costs == null) ? 0 : costs.Length; }
 }
