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
			dmg = new int[] { 15, 33, 68, 134, 248, 426, 683, 1035, 1499, 2090, 2825 };
			rof = new float[] { 2.85f, 3f, 3.2f, 3.55f, 3.95f, 4.5f };
			amo = new float[] { 105f, 83f, 78f, 70f, 60f, 43f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 11), new Stat_Cost(0, 19), new Stat_Cost(0, 27), new Stat_Cost(0, 35), new Stat_Cost(0, 44),
									  new Stat_Cost(0, 53), new Stat_Cost(0, 62), new Stat_Cost(8, 71), new Stat_Cost(12, 80), new Stat_Cost(18, 90) };
			rof_c = new Stat_Cost[] { new Stat_Cost(1, 0), new Stat_Cost(3, 0), new Stat_Cost(6, 0), new Stat_Cost(11, 24), new Stat_Cost(18, 58) };
			amo_c = new Stat_Cost[] { new Stat_Cost(6, 12), new Stat_Cost(8, 19), new Stat_Cost(11, 32), new Stat_Cost(16, 51), new Stat_Cost(24, 65) };
		} else if (type.CompareTo(WEAPON_TYPE.rifle) == 0) {
			dmg = new int[] { 6, 25, 57, 100, 155, 222, 300, 391, 493, 606, 733 };
			rof = new float[] { 4, 6, 9, 13, 18, 23 };
			amo = new float[] { 13f, 10f, 7f, 5f, 3f, 1f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 14), new Stat_Cost(0, 23), new Stat_Cost(0, 32), new Stat_Cost(0, 41), new Stat_Cost(0, 51),
									  new Stat_Cost(0, 60), new Stat_Cost(0, 70), new Stat_Cost(10, 80), new Stat_Cost(15, 90), new Stat_Cost(21, 100) };
			rof_c = new Stat_Cost[] { new Stat_Cost(3, 0), new Stat_Cost(5, 0), new Stat_Cost(8, 0), new Stat_Cost(13, 26), new Stat_Cost(22, 62) };
			amo_c = new Stat_Cost[] { new Stat_Cost(3, 6), new Stat_Cost(5, 14), new Stat_Cost(8, 28), new Stat_Cost(12, 47), new Stat_Cost(18, 65) };
		} else if (type.CompareTo(WEAPON_TYPE.shotgun) == 0) {
			dmg = new int[] { 3, 16, 39, 70, 111, 161, 219, 287, 264, 450, 545 };
			rof = new float[] { 2, 3, 4, 5, 7, 9 };
			amo = new float[] { 21f, 17f, 13f, 10f, 7f, 5f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 5), new Stat_Cost(0, 11), new Stat_Cost(0, 17), new Stat_Cost(0, 24), new Stat_Cost(0, 31),
									  new Stat_Cost(0, 43), new Stat_Cost(0, 46), new Stat_Cost(4, 53), new Stat_Cost(7, 61), new Stat_Cost(12, 70) };
			rof_c = new Stat_Cost[] { new Stat_Cost(3, 0), new Stat_Cost(7, 0), new Stat_Cost(13, 0), new Stat_Cost(21, 30), new Stat_Cost(30, 70) };
			amo_c = new Stat_Cost[] { new Stat_Cost(4, 8), new Stat_Cost(7, 16), new Stat_Cost(11, 29), new Stat_Cost(15, 43), new Stat_Cost(20, 70) };
		} else if (type.CompareTo(WEAPON_TYPE.grenade) == 0) {
			dmg = new int[] { 24, 65, 119, 183, 260, 347, 447, 557, 680, 813, 960 };
			rof = new float[] { 1.65f, 1.9f, 2.3f, 2.5f, 2.75f, 3.1f };
			amo = new float[] { 42f, 38f, 34f, 30f, 26f, 22f };

			dmg_c = new Stat_Cost[] { new Stat_Cost(0, 8), new Stat_Cost(0, 14), new Stat_Cost(0, 20), new Stat_Cost(0, 28), new Stat_Cost(0, 35),
									  new Stat_Cost(0, 43), new Stat_Cost(0, 52), new Stat_Cost(6, 61), new Stat_Cost(10, 70), new Stat_Cost(15, 80) };
			rof_c = new Stat_Cost[] { new Stat_Cost(5, 0), new Stat_Cost(8, 0), new Stat_Cost(13, 0), new Stat_Cost(19, 28), new Stat_Cost(26, 66) };
			amo_c = new Stat_Cost[] { new Stat_Cost(3, 7), new Stat_Cost(6, 21), new Stat_Cost(10, 37), new Stat_Cost(15, 55), new Stat_Cost(22, 75) };
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

