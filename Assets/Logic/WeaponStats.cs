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
		int[] dmg = null, rof = null, amo = null;

		if (type.CompareTo(WEAPON_TYPE.sword) == 0) {
			dmg = new int[] { 15, 19, 23, 29, 36, 41, 47, 52, 56 };
			rof = new int[] { 20 };
			amo = new int[] { 0 };
		} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
			dmg = new int[] { 2, 4, 6, 8, 10, 12, 14, 16 };
			rof = new int[] { 25, 29, 34, 40, 45, 51, 58, 64 };
			amo = new int[] { 10, 8, 7, 6, 5, 3 };
		} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
			dmg = new int[] { 8, 12, 17, 21, 27, 32, 39, 45 };
			rof = new int[] { 8, 12, 15, 19, 23, 26, 29 };
			amo = new int[] { 24, 22, 19, 16, 13, 9 };
		} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
			dmg = new int[] { 20, 25, 31, 37, 43, 49, 54 };
			rof = new int[] { 2, 4, 6, 9, 12, 16, 19 };
			amo = new int[] { 35, 32, 30, 27, 23, 20 };
		}

		/* stats[0] = damage
		 * stats[1] = rate of fire
		 * stats[2] = ammo consumption */
		stats[0] = new Stat(STAT_TYPE.damage, dmg);
		stats[1] = new Stat(STAT_TYPE.rate_of_fire, rof);
		stats[2] = new Stat(STAT_TYPE.ammo, amo);
	}

	/* Returns the weapon stat corresponding to the given type.
	 * See Enumerations class for valid values for parameter t. */
	public Stat stat_by_type(STAT_TYPE t) {
		switch (t) {
			case STAT_TYPE.damage:			return stats[0];
			case STAT_TYPE.rate_of_fire:	return stats[1];
			case STAT_TYPE.ammo:			return stats[2];
			default:						return null;
		}
	}
}

