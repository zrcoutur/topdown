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
			dmg = new int[] { 5, 10, 16, 22, 30, 40, 52, 64, };
			rof = new int[] { 4 };
			amo = new int[] { 0 };
		} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
			dmg = new int[] { 2, 5, 10, 16, 24, 32 };
			rof = new int[] { 6, 9, 12, 16, 20, 25 };
			amo = new int[] { 7, 6, 5, 4, 3, 2 };
		} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
			dmg = new int[] { 5, 8, 13, 21, 27, 36 };
			rof = new int[] { 1, 2, 3, 5, 7, 10, 35 };
			amo = new int[] { 18, 15, 12, 9, 0 };
		} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
			dmg = new int[] { 15, 25, 36, 42, 58, 70 };
			rof = new int[] { 1, 3, 5 };
			amo = new int[] { 36, 32, 26, 20, 12 };
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
	public Stat weapon_stat(STAT_TYPE t) {
		switch (t) {
			case STAT_TYPE.damage:			return stats[0];
			case STAT_TYPE.rate_of_fire:	return stats[1];
			case STAT_TYPE.ammo:			return stats[2];
			default:						return null;
		}
	}
}

