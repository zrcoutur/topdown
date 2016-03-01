using UnityEngine;
using System.Collections;

public static class Storage {

	public static readonly Stat MAX_HEALTH;
	public static readonly Stat MAX_SHIELD;
	private static readonly WeaponStats[] WEAPONS;
	// Used to communicate between the DynamicGUI and the Player's HP slider
	public static bool HP_raised;
	public static bool Shield_raised;

	static Storage () {
		MAX_HEALTH = new Stat(STAT_TYPE.health, new int[] { 25, 46, 67, 98, 121, 167, 225 });
		MAX_SHIELD = new Stat(STAT_TYPE.shield, new int[] { 10, 12, 15, 18, 22, 27, 33, 41, 51, 63, 78, 97, 121, 151, 188, 235, 293, 366, 457, 571, 713, 891 });
		HP_raised = false;
		Shield_raised = false;

		WEAPONS = new WeaponStats[4];
		WEAPONS[0] = new WeaponStats(0);
		WEAPONS[1] = new WeaponStats(1);
		WEAPONS[2] = new WeaponStats(2);
		WEAPONS[3] = new WeaponStats(3);
	}

	/* Returns the stats of the weapon with the given type.
	 * See Enumerations class for valid values for parameter t. */
	public static WeaponStats weapon_by_type(int t) {
		switch ((WEAPON_TYPE)t) {
			case WEAPON_TYPE.sword:		return WEAPONS[0];
			case WEAPON_TYPE.rifle:		return WEAPONS[1];
			case WEAPON_TYPE.shotgun:	return WEAPONS[2];
			case WEAPON_TYPE.grenade:	return WEAPONS[3];
			default: 					return null;	
		}
	}

	/* Returns the number of weapon with stats. */
	public static int num_of_weapons() { return WEAPONS.Length; }
}
