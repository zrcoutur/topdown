using System;

/**
 * This class is designed to define the stat values of any weapon.
 * 
 * @author Joshua Hooker
 * 23 February 2016
 */
public class WeaponStats {
	// the type of weapon
	public readonly WEAPON_TYPE type;
	// the list of values for the weapons damage, rate of fire, and ammo consumption
	private readonly Stat[] stats;


	/**
	 * Creates a weapon with the given type
	 */
	public WeaponStats(byte t) {
		type = (WEAPON_TYPE)t;
		// currently there are only three stats
		stats = new Stat[3];
		initializeStats();
	}

	/* Initializes all the stats of the weapon based on its type. */
	private void initializeStats() {
		int[] dmg = null, rof = null;
		float[] amo = null;
		Stat_Cost[] dmg_c = null, rof_c = null, amo_c = null;

			if (type.CompareTo(WEAPON_TYPE.sword) == 0) {
				dmg = new int[] { 16, 30, 51, 86, 113, 189, 273, 391, 562, 733, 1003, 1531, 3456 };
				rof = new int[] { 4 };
				amo = new float[] { 0 };

				dmg_c = new Stat_Cost[12];

				for (int idx = 0; idx < dmg_c.Length; ++idx) {
					dmg_c [idx] = new Stat_Cost((2 * idx * idx) + 6 * idx + 2, (int)(2.6f * idx * idx * idx) + (int)(0.8f * idx * idx) + 8 * idx + 3);
				}

				rof_c = new Stat_Cost[0];
				amo_c = new Stat_Cost[0];
			} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
				dmg = new int[] { 8, 11, 17, 26, 38, 55, 76, 99, 113, 206, 343, 639 };
				rof = new int[] { 7, 10, 15, 23, 32, 43 };
				amo = new float[] { 7f, 6f, 5f, 4f, 3f, 2f, 1f, 0.66f };

				dmg_c = new Stat_Cost[] { new Stat_Cost(0, 3), new Stat_Cost(0, 11), new Stat_Cost(0, 23), new Stat_Cost(0, 39), new Stat_Cost(0, 60),
										  new Stat_Cost(0, 89), new Stat_Cost(0, 123), new Stat_Cost(0, 172), new Stat_Cost(12, 256), new Stat_Cost(28, 344),
										  new Stat_Cost(47, 495) };
				rof_c = new Stat_Cost[] { new Stat_Cost(0, 5), new Stat_Cost(5, 16), new Stat_Cost(18, 43), new Stat_Cost(31, 113),	new Stat_Cost(73, 290) };
				amo_c = new Stat_Cost[] { new Stat_Cost(5, 0), new Stat_Cost(11, 0), new Stat_Cost(23, 0), new Stat_Cost(39, 0),
										  new Stat_Cost(64, 156), new Stat_Cost(99, 378), new Stat_Cost(166, 671) };
			} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
				dmg = new int[] { 5, 8, 13, 22, 34, 52, 73, 109, 213 };
				rof = new int[] { 2, 3, 5, 8, 12, 16, 22 };
				amo = new float[] { 18f, 15f, 12f, 9f, 7f, 5f, 3.33f };

				dmg_c = new Stat_Cost[] { new Stat_Cost(0, 9), new Stat_Cost(0, 17), new Stat_Cost(0, 35), new Stat_Cost(0, 56),
										  new Stat_Cost(0, 93), new Stat_Cost(33, 155), new Stat_Cost(72, 367),	new Stat_Cost(126, 721) };
				rof_c = new Stat_Cost[] { new Stat_Cost(1, 8), new Stat_Cost(3, 23), new Stat_Cost(7, 56), new Stat_Cost(11, 113),
										  new Stat_Cost(21, 206), new Stat_Cost(36, 389) };
				amo_c = new Stat_Cost[] { new Stat_Cost(9, 0), new Stat_Cost(21, 0), new Stat_Cost(43, 0), new Stat_Cost(78, 213),
										  new Stat_Cost(145, 345), new Stat_Cost(231, 989) };
			} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
				dmg = new int[] { 15, 25, 36, 42, 58, 70 };
				rof = new int[] { 1, 3, 5 };
				amo = new float[] { 36, 32, 26, 20, 12 };

				dmg_c = new Stat_Cost[0];
				rof_c = new Stat_Cost[0];
				amo_c = new Stat_Cost[0];
			}

		/* stats[0] = damage
		 * stats[1] = rate of fire
		 * stats[2] = ammo consumption */
		stats[0] = new Stat(STAT_TYPE.damage, dmg, dmg_c);
		stats[1] = new Stat(STAT_TYPE.rate_of_fire, rof, rof_c);
		stats[2] = new Stat(STAT_TYPE.ammo, amo, amo_c);
	}

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

