﻿using UnityEngine;
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

	private readonly WeaponStats[] WEAPONS;
	/* Player's current weapon (see Enumerations.cs for respective integer-type pairs) */
	private WEAPON_TYPE held_weapon;

	// Counters for consumable materials
	private int energyCores;
	private int scrap;

	public Player_Stats() {
		int[] val_temp = new int[11];
		Stat_Cost[] cost_temp = new Stat_Cost[10];
		// initialize health values
		for (int idx = 0; idx < val_temp.Length; ++idx) {
			val_temp[idx] = (2 * idx * idx) + 5;
		}
		// initialize stat costs
		for (int idx = 0; idx < cost_temp.Length; ++idx) {
			cost_temp[idx] = new Stat_Cost(2 * idx + 1, (int)(0.45f * idx * idx) + 5 * idx + 6);

		}

		MAX_HEALTH = new Stat(STAT_TYPE.health, val_temp, cost_temp);
		health = MAX_HEALTH.current();
		HP_raised = true;

		val_temp = new int[11];
		cost_temp = new Stat_Cost[10];
		// initialize shield values
		for (int idx = 0; idx < val_temp.Length; ++idx) {
			val_temp[idx] = (int)(0.6f * idx * idx) + 3 * idx + 8;
		}

		// initialize stat costs
		for (int idx = 0; idx < cost_temp.Length; ++idx) {
			cost_temp[idx] = new Stat_Cost((int)(0.13f * idx * idx) + idx + 2, (int)(0.35f * idx * idx) + 3 * idx + 3);
		}

		MAX_SHIELD = new Stat(STAT_TYPE.shield, val_temp, cost_temp);
		shield = MAX_SHIELD.current();
		Shield_raised = true;

		WEAPONS = new WeaponStats[4];
		WEAPONS[0] = new WeaponStats(0);
		WEAPONS[1] = new WeaponStats(1);
		WEAPONS[2] = new WeaponStats(2);
		WEAPONS[3] = new WeaponStats(3);
		held_weapon = WEAPON_TYPE.sword;

		energyCores = 0;
		scrap = 0;
	}

	/* Changes health by the given value, but restores it to the range
	 * [0, MAX_HEALTH]; if the result lies outside the given range. */
	public void change_health(int value) {
		health += value;

		if (health > MAX_HEALTH.current()) {
			health = MAX_HEALTH.current();
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
			shield = MAX_SHIELD.current();
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
		// the grenade is currently not implemented, so it is skipped.
		held_weapon = (WEAPON_TYPE)( ( (byte)held_weapon + 1 ) % (byte)WEAPON_TYPE.grenade );
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
