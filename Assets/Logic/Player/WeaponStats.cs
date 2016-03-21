﻿using System;

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
		int[] dmg = null, rof = null, amo = null;
		Stat_Cost[] dmg_c = null, rof_c = null, amo_c = null;

		if (type.CompareTo(WEAPON_TYPE.sword) == 0) {
			dmg = new int[] { 16, 23, 32, 46, 59, 73, 96, 132, 187, 273 };
			rof = new int[] { 4 };
			amo = new int[] { 0 };

			dmg_c = new Stat_Cost[] { new Stat_Cost(1, 5), new Stat_Cost(1, 11), new Stat_Cost(2, 19), new Stat_Cost(3, 30), new Stat_Cost(5, 43), new Stat_Cost(7, 59), new Stat_Cost(9, 79), new Stat_Cost(13, 103), new Stat_Cost(21, 167) };
			rof_c = new Stat_Cost[0];
			amo_c = new Stat_Cost[0];
		} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
			dmg = new int[] { 8, 11, 17, 26, 38, 55, 76, 99, 113 };
			rof = new int[] { 6, 9, 12, 16, 20, 25, 36 };
			amo = new int[] { 7, 6, 5, 4, 3, 2 };

			dmg_c = new Stat_Cost[] { new Stat_Cost(1, 3), new Stat_Cost(2, 11), new Stat_Cost(3, 23), new Stat_Cost(5, 39), new Stat_Cost(8, 60), new Stat_Cost(12, 89), new Stat_Cost(16, 122), new Stat_Cost(26, 189) };
			rof_c = new Stat_Cost[] { new Stat_Cost(1, 6), new Stat_Cost(2, 18), new Stat_Cost(4, 43), new Stat_Cost(9, 81), new Stat_Cost(15, 127), new Stat_Cost(29, 206) };
			amo_c = new Stat_Cost[] { new Stat_Cost(1, 9), new Stat_Cost(3, 23), new Stat_Cost(6, 44), new Stat_Cost(11, 97), new Stat_Cost(17, 173) };
		} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
			dmg = new int[] { 5, 8, 13, 22, 34, 52, 73 };
			rof = new int[] { 1, 2, 3, 5, 7, 10, 16, 23 };
			amo = new int[] { 18, 15, 12, 9, 7 };

			dmg_c = new Stat_Cost[] { new Stat_Cost(2, 9), new Stat_Cost(5, 17), new Stat_Cost(9, 35), new Stat_Cost(13, 56), new Stat_Cost(19, 93), new Stat_Cost(24, 170) };
			rof_c = new Stat_Cost[] { new Stat_Cost(1, 7), new Stat_Cost(2, 18), new Stat_Cost(4, 39), new Stat_Cost(6, 57), new Stat_Cost(9, 78), new Stat_Cost(13, 123), new Stat_Cost(21, 216) };
			amo_c = new Stat_Cost[] { new Stat_Cost(3, 13), new Stat_Cost(7, 29), new Stat_Cost(11, 56), new Stat_Cost(16, 104) };
		} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
			dmg = new int[] { 15, 25, 36, 42, 58, 70 };
			rof = new int[] { 1, 3, 5 };
			amo = new int[] { 36, 32, 26, 20, 12 };

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

