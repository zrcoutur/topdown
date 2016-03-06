using System;

// Assign arbitrary values to each weapon type
public enum WEAPON_TYPE: byte {
	sword = 0,
	rifle = 1,
	shotgun = 2,
	grenade = 3
};

// Assign arbitrary values to each stat type
public enum STAT_TYPE: byte {
	health = 0,
	shield = 1,
	energy = 2,
	damage = 3,
	rate_of_fire = 4,
	ammo = 5
};

public enum COST_TYPE : byte {
	scrap = 0,
	cores = 1
};

