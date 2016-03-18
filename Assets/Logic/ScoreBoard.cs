using System;
using UnityEngine;

/**
 * A class designed to hold the player's progress in the game in some key areas.
 * 
 * author : Joshua Hooker
 * 18 March 2016
 */
public static class ScoreBoard {
	// total number of enemies killed
	public static ulong enemies_killed;
	// total number of bullets fired
	public static ulong bullets_fired;
	// the number of bullets that hit an enemy
	public static ulong enemies_hit;
	// total number of scrap collected
	public static ulong scrap_collected;
	// total number of e. cores collected
	public static ulong ecores_collected;

	// Initialize values
	static ScoreBoard() {
		enemies_killed = 0;
		bullets_fired = 0;
		enemies_hit = 0;
		scrap_collected = 0;
		ecores_collected = 0;
	}

	/* Prints all values out to the commmand line. */
	public static void display_scores() {
		Debug.Log("Enemies killed: " + enemies_killed + 
				  "\nBullets fired: " + bullets_fired +
				  "\nEnemies hit: " + enemies_hit +
				  "\nScrap Collected: " + scrap_collected + 
				  "\nE. Cores collected: " + ecores_collected);
	}
}
