using System;

/**
 * This class is designed to define and maintain the stat values of a sepcific weapon. Each weapon has a type, a state,
 * damage, rate of fire, and ammo cost. The types of weapons are: sword, rifle, shotgun, and grenade launcher.
 * 
 * author : Joshua Hooker
 * 23 February 2016
 */
public class WeaponStats {
	// the type of weapon
	public readonly WEAPON_TYPE type;
	/* This value is used to determine which upgrade the weapon currently has if any.
	 * 0 -> no upgrade
	 * 1 -> damage focus upgrade
	 * 2 -> rate of fire focused upgrade
	 * 3 -> no focus upgrade */
	private int state;
	// the list of values for the weapons damage, rate of fire, and ammo consumption
	private readonly Stat[] stats;

	/**
	 * Creates a weapon with the given type
	 */
	public WeaponStats(byte t) {
		type = (WEAPON_TYPE)t;
		state = 0;
		// currently there are only three stats
		stats = new Stat[3];
		initializeStats();
	}

	/* Initializes all the stats of the weapon based on its type. */
	private void initializeStats() {
		int[] dmg = null;
		float[] rof = null, amo = null;
		Stat_Cost[] dmg_c = null, rof_c = null, amo_c = null;

		// Initializes stats and stat costs based on the weapon type
		if (type.CompareTo(WEAPON_TYPE.sword) == 0) {
			dmg = new int[] { 32, 113, 324, 734, 1190, 1934, 2567, 3412, 6311, 11676, 21601 };
			rof = new float[] { 4 };
			amo = new float[] { 0 };

			dmg_c = new Stat_Cost[10];
			for (int idx = 0; idx < dmg_c.Length; ++idx) {
				dmg_c [idx] = new Stat_Cost((int)(1.5f * idx * idx) + 9 * idx + 4, (idx * idx * idx) + (int)(1.5f * idx * idx) + 15 * idx + 13);
			}

			rof_c = new Stat_Cost[0];
			amo_c = new Stat_Cost[0];
		} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
			dmg = new int[] { 12, 25, 43, 98, 137, 191, 254, 326, 413, 523, 789 };
			rof = new float[] { 4, 6, 9, 13, 18, 23 };
			amo = new float[] { 13f, 10f, 7f, 5f, 3f, 1f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 5), new Stat_Cost(0, 12), new Stat_Cost(0, 23), new Stat_Cost(0, 39), new Stat_Cost(0, 70),
									  new Stat_Cost(0, 109), new Stat_Cost(0, 154), new Stat_Cost(11, 213), new Stat_Cost(24, 287), new Stat_Cost(54, 412) };
			rof_c = new Stat_Cost[] { new Stat_Cost(3, 0), new Stat_Cost(9, 0), new Stat_Cost(18, 0), new Stat_Cost(34, 86),	new Stat_Cost(55, 234) };
			amo_c = new Stat_Cost[] { new Stat_Cost(2, 11), new Stat_Cost(8, 31), new Stat_Cost(19, 67), new Stat_Cost(35, 123), new Stat_Cost(58, 330) };
		} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
			dmg = new int[] { 6, 11, 23, 45, 79, 121, 174, 235, 318, 409, 563 };
			rof = new float[] { 2, 3, 4, 5, 7, 9 };
			amo = new float[] { 21f, 17f, 13f, 10f, 7f, 5f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 8), new Stat_Cost(0, 18), new Stat_Cost(0, 34), new Stat_Cost(0, 58), new Stat_Cost(0, 96),
									  new Stat_Cost(0, 153), new Stat_Cost(0, 241), new Stat_Cost(16, 362), new Stat_Cost(33, 512), new Stat_Cost(64, 745) };
			rof_c = new Stat_Cost[] { new Stat_Cost(5, 0), new Stat_Cost(13, 0), new Stat_Cost(28, 0), new Stat_Cost(43, 113), new Stat_Cost(63, 289) };
			amo_c = new Stat_Cost[] { new Stat_Cost(4, 16), new Stat_Cost(11, 39), new Stat_Cost(23, 81), new Stat_Cost(38, 153), new Stat_Cost(62, 304) };
		} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
			dmg = new int[] { 50, 68, 93, 131, 186, 273, 391, 523, 671, 912, 1356 };
			rof = new float[] { 1.2f, 1.3f, 1.5f, 1.85f, 2.1f, 2.5f };
			amo = new float[] { 56f, 52f, 48f, 45f, 42f, 39f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 11), new Stat_Cost(0, 24), new Stat_Cost(0, 41), new Stat_Cost(0, 67), new Stat_Cost(0, 113),
								 	  new Stat_Cost(0, 173), new Stat_Cost(0, 278), new Stat_Cost(22, 393), new Stat_Cost(41, 567), new Stat_Cost(73, 836) };
			rof_c = new Stat_Cost[] { new Stat_Cost(5, 0), new Stat_Cost(16, 0), new Stat_Cost(27, 0), new Stat_Cost(43, 109), new Stat_Cost(68, 341) };
			amo_c = new Stat_Cost[] { new Stat_Cost(8, 23), new Stat_Cost(19, 39), new Stat_Cost(31, 111), new Stat_Cost(56, 234), new Stat_Cost(83, 413) };
		}

		/* stats[0] = damage
		 * stats[1] = rate of fire
		 * stats[2] = ammo consumption */
		stats[0] = new Stat(STAT_TYPE.damage, dmg, dmg_c);
		stats[1] = new Stat(STAT_TYPE.rate_of_fire, rof, rof_c);
		stats[2] = new Stat(STAT_TYPE.ammo, amo, amo_c);
	}

	/* If the given value is a valid upgrade value and the upgrade
	 * is equal to 0 (initial state), then upgrade is set to the
	 * given value.*/
	public void setUgrade(int upgrade) {
		if (state == 0 && upgrade >= 0 && upgrade <= 3) {
			state = upgrade;
		}
	}

	/* Returns the current state of the weapon. */
	public int upgrade_state() { return state; }

	/* Returns the weapon stat corresponding to the given type.
	 * See Enumerations class for valid values for parameter t. */
	public Stat weapon_stat(STAT_TYPE t) {
		switch (t) {
			case STAT_TYPE.damage:			return stats[0];
			case STAT_TYPE.rate_of_fire:	return stats[1];
			case STAT_TYPE.ammo:			return stats[2];
			default:						return null;
		}
	}
}

