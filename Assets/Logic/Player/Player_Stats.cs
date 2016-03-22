using UnityEngine;
using System.Collections;

public class Player_Stats {
	// Related to health value
	public readonly Stat MAX_HEALTH;
	private int health;
	public bool HP_raised;
	// Related to shield value
	public readonly Stat MAX_SHIELD;
	private int shield;
	public bool Shield_raised;
	// Related to player's ability to heal themselves
	public Stat MEDPACKS;

	private readonly WeaponStats[] WEAPONS;
	/* Player's current weapon (see Enumerations.cs for respective integer-type pairs) */
	private WEAPON_TYPE held_weapon;
	public int[] owned_weapons = { 1,1,1,0};

	// Counters for consumable materials
	private int energyCores;
	private int scrap;

	public Player_Stats() {
		int[] val_temp = new int[10];
		Stat_Cost[] cost_temp = new Stat_Cost[9];
		// initialize health values
		for (int idx = 0; idx < val_temp.Length; ++idx) {
			val_temp[idx] = (idx * idx) + 2 * idx + 6;
		}
		// initialize stat costs
		for (int idx = 0; idx < cost_temp.Length; ++idx) {
			cost_temp[idx] = new Stat_Cost(0, (int)(1f * idx * idx * idx) + (int)(1.5f * idx * idx) + 6 * idx + 8);

		}

		MAX_HEALTH = new Stat(STAT_TYPE.health, val_temp, cost_temp);
		health = (int)MAX_HEALTH.current();
		HP_raised = true;

		val_temp = new int[14];
		cost_temp = new Stat_Cost[13];
		// initialize shield values
		for (int idx = 0; idx < val_temp.Length; ++idx) {
			val_temp[idx] = (int)(2.6f * idx * idx) + 3 * idx + 8;
		}

		// initialize stat costs
		for (int idx = 0; idx < cost_temp.Length; ++idx) {
			cost_temp[idx] = new Stat_Cost((int)(0.4f * idx * idx) + 6 * idx + 3, 0);
		}

		MAX_SHIELD = new Stat(STAT_TYPE.shield, val_temp, cost_temp);
		shield = (int)MAX_SHIELD.current();
		Shield_raised = true;

		WEAPONS = new WeaponStats[4];
		WEAPONS[0] = new WeaponStats(0);
		WEAPONS[1] = new WeaponStats(1);
		WEAPONS[2] = new WeaponStats(2);
		WEAPONS[3] = new WeaponStats(3);
		held_weapon = WEAPON_TYPE.sword;

		energyCores = 0;
		scrap = 0;

		MEDPACKS = new Stat(STAT_TYPE.other, new int[] { 0, 1, 2, 3 },
											 new Stat_Cost[] { new Stat_Cost(6, 45), new Stat_Cost(11, 98), new Stat_Cost(23, 189) } );

	}

	/* Changes health by the given value, but restores it to the range
	 * [0, MAX_HEALTH]; if the result lies outside the given range. */
	public void change_health(int value) {
		health += value;

		if (health > MAX_HEALTH.current()) {
			health = (int)MAX_HEALTH.current();
		} else if (health < 0) {
			health = 0;
		}
	}

	/* Returns current health value. */
	public int get_health() { return health; }

	/* Changes shield by the given value, but restores it to the range [0, MAX_SHIELD]; if the result lies outside the given range.
	 * This method returns the amount of difference between the shield value and zero, if the reuslting sheild value falls below
	 * zero, otherwise zero is returned. */
	public int change_shield(int value) {
		shield += value;
		var excess = 0;

		if (shield > MAX_SHIELD.current()) {
			shield = (int)MAX_SHIELD.current();
		} else if (shield < 0) {
			// returns any overflow damage after the shield is exhausted
			excess = shield;
			shield = 0;
		}

		return excess;
	}

	/* Returns current shield value. */
	public int get_shield() { return shield; }

	/* Returns the stats of the weapon with the given type.
	 * See Enumerations class for valid values for parameter t. */
	public WeaponStats weapon_by_type(WEAPON_TYPE t) {
		switch (t) {
			case WEAPON_TYPE.sword:		return WEAPONS[0];
			case WEAPON_TYPE.rifle:		return WEAPONS[1];
			case WEAPON_TYPE.shotgun:	return WEAPONS[2];
			case WEAPON_TYPE.grenade:	return WEAPONS[3];
			default: 					return null;	
		}
	}

	/* Cycles to the next weapon base on the integer value associated with a weapon type. */
	public void cycle_weapons() {
		
		//checks all weapon slots for the next weapon owned, then sets held weaon to that
		//weapons not owned yet will not be cycled 
		for (int i = 1; i < 4; i++)
		{
			if (owned_weapons[((byte)held_weapon + i) % 3] > 0)
			{
				// the grenade is currently not implemented, so it is skipped.
				held_weapon = (WEAPON_TYPE)(((byte)held_weapon + i) % (byte)WEAPON_TYPE.grenade);
				break;
			}
		}
		
	}

	/* Returns the integer representation of the Player's current weapon */
	public WEAPON_TYPE current_weapon() { return held_weapon; }

	/* Adds the given value to the current scrap count. Any addition that would
	 * result in a negative number will change the count value to zero, instead. */
	public void change_scrap(int value) {
		scrap = Mathf.Max( (scrap + value), 0 );
	}

	/* Returns current scrap count. */
	public int get_scrap() { return scrap; }

	/* Adds the given value to the current energy cores count. Any addition that would
	 * result in a negative number will change the count value to zero, instead. */
	public void change_ecores(int value) {
		energyCores = Mathf.Max( (energyCores + value), 0 );
	}

	/* Returns current energy core count. */
	public int get_ecores() { return energyCores; }

	/* Returns the number of weapon with stats. */
	public int num_of_weapons() { return WEAPONS.Length; }

}
