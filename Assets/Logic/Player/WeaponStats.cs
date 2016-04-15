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
			dmg = new int[] { 43, 216, 489, 935, 1868, 3297, 5330, 8075, 11640, 16133, 21662 };
			rof = new float[] { 4 };
			amo = new float[] { 0 };

			dmg_c = new Stat_Cost[10];
			for (int idx = 0; idx < dmg_c.Length; ++idx) {
				dmg_c [idx] = new Stat_Cost((int)(0.35f * idx * idx) + 5 * idx + 2, (int)(0.3f * idx * idx * idx * idx + 0.75f * idx * idx) + 13 * idx + 11);
			}

			rof_c = new Stat_Cost[0];
			amo_c = new Stat_Cost[0];
		} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
			dmg = new int[] { 18, 57, 144, 279, 462, 693, 972, 1288, 1674, 2097, 2568 };
			rof = new float[] { 4, 6, 9, 13, 18, 23 };
			amo = new float[] { 13f, 10f, 7f, 5f, 3f, 1f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 7), new Stat_Cost(0, 19), new Stat_Cost(0, 32), new Stat_Cost(0, 47), new Stat_Cost(0, 62),
									  new Stat_Cost(0, 78), new Stat_Cost(0, 95), new Stat_Cost(13, 113), new Stat_Cost(18, 131), new Stat_Cost(25, 151) };
			rof_c = new Stat_Cost[] { new Stat_Cost(3, 0), new Stat_Cost(9, 0), new Stat_Cost(16, 0), new Stat_Cost(23, 54),	new Stat_Cost(36, 98) };
			amo_c = new Stat_Cost[] { new Stat_Cost(5, 9), new Stat_Cost(12, 23), new Stat_Cost(19, 54), new Stat_Cost(28, 96), new Stat_Cost(41, 126) };
		} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
			dmg = new int[] { 16, 54, 106, 170, 248, 338, 442, 558, 688, 830, 986 };
			rof = new float[] { 2, 3, 4, 5, 7, 9 };
			amo = new float[] { 21f, 17f, 13f, 10f, 7f, 5f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 9), new Stat_Cost(0, 25), new Stat_Cost(0, 43), new Stat_Cost(0, 62), new Stat_Cost(0, 83),
									  new Stat_Cost(0, 105), new Stat_Cost(0, 128), new Stat_Cost(16, 152), new Stat_Cost(22, 178), new Stat_Cost(31, 205) };
			rof_c = new Stat_Cost[] { new Stat_Cost(5, 0), new Stat_Cost(14, 0), new Stat_Cost(21, 0), new Stat_Cost(30, 78), new Stat_Cost(42, 125) };
			amo_c = new Stat_Cost[] { new Stat_Cost(6, 15), new Stat_Cost(15, 31), new Stat_Cost(23, 78), new Stat_Cost(37, 113), new Stat_Cost(54, 159) };
		} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
			dmg = new int[] { 64, 163, 318, 521, 772, 1071, 1418, 1813, 2256, 3286 };
			rof = new float[] { 1.65f, 1.9f, 2.3f, 2.5f, 2.75f, 3.1f };
			amo = new float[] { 42f, 38f, 34f, 30f, 26f, 22f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 11), new Stat_Cost(0, 32), new Stat_Cost(0, 55), new Stat_Cost(0, 79), new Stat_Cost(0, 105),
								 	  new Stat_Cost(0, 132), new Stat_Cost(0, 160), new Stat_Cost(21, 189), new Stat_Cost(34, 220), new Stat_Cost(42, 252) };
			rof_c = new Stat_Cost[] { new Stat_Cost(8, 0), new Stat_Cost(20, 0), new Stat_Cost(32, 0), new Stat_Cost(42, 109), new Stat_Cost(55, 163) };
			amo_c = new Stat_Cost[] { new Stat_Cost(10, 23), new Stat_Cost(22, 42), new Stat_Cost(37, 77), new Stat_Cost(53, 129), new Stat_Cost(68, 173) };
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

