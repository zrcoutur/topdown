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
		int[] dmg = null, rof = null;
		float[] amo = null;
		Stat_Cost[] dmg_c = null, rof_c = null, amo_c = null;

			if (type.CompareTo(WEAPON_TYPE.sword) == 0) {
				dmg = new int[] { 45, 85, 157, 291, 539, 997, 1844, 3412, 6311, 11676, 21601 };
				rof = new int[] { 4 };
				amo = new float[] { 0 };

				dmg_c = new Stat_Cost[10];
				for (int idx = 0; idx < dmg_c.Length; ++idx) {
					dmg_c [idx] = new Stat_Cost((int)(1.5f * idx * idx) + 9 * idx + 4, (idx * idx * idx) + (int)(1.5f * idx * idx) + 15 * idx + 13);
				}

				rof_c = new Stat_Cost[0];
				amo_c = new Stat_Cost[0];
			} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
				dmg = new int[] { 8, 13, 19, 27, 38, 56, 79, 125, 233, 378, 521 };
				rof = new int[] { 4, 6, 9, 13, 18, 23 };
				amo = new float[] { 13f, 10f, 7f, 5f, 3f, 1f };

				dmg_c = new Stat_Cost[] { new Stat_Cost(0, 3), new Stat_Cost(0, 11), new Stat_Cost(0, 23), new Stat_Cost(0, 39), new Stat_Cost(0, 60),
										  new Stat_Cost(0, 89), new Stat_Cost(0, 123), new Stat_Cost(0, 172), new Stat_Cost(12, 256), new Stat_Cost(28, 344) };
				rof_c = new Stat_Cost[] { new Stat_Cost(0, 5), new Stat_Cost(5, 16), new Stat_Cost(18, 43), new Stat_Cost(31, 113),	new Stat_Cost(73, 290) };
				amo_c = new Stat_Cost[] { new Stat_Cost(5, 0), new Stat_Cost(11, 0), new Stat_Cost(23, 0), new Stat_Cost(39, 0), new Stat_Cost(64, 156) };
			} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
				dmg = new int[] { 5, 12, 23, 39, 61, 111, 152, 208, 267, 331, 413 };
				rof = new int[] { 2, 3, 4, 5, 7, 9 };
				amo = new float[] { 21f, 17f, 13f, 10f, 7f, 5f };

				dmg_c = new Stat_Cost[] { new Stat_Cost(0, 9), new Stat_Cost(0, 17), new Stat_Cost(0, 35), new Stat_Cost(0, 56), new Stat_Cost(0, 93),
										  new Stat_Cost(33, 155), new Stat_Cost(72, 367), new Stat_Cost(126, 721), new Stat_Cost(0, 0) };
				rof_c = new Stat_Cost[] { new Stat_Cost(1, 8), new Stat_Cost(3, 23), new Stat_Cost(7, 56), new Stat_Cost(19, 143), new Stat_Cost(41, 406) };
				amo_c = new Stat_Cost[] { new Stat_Cost(9, 0), new Stat_Cost(21, 0), new Stat_Cost(43, 0), new Stat_Cost(78, 213), new Stat_Cost(145, 345) };
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

