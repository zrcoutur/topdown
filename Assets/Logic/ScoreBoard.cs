using System;
using UnityEngine;
using UnityEngine.UI;

/**
 * A class designed to hold the player's progress in the game in some key areas.
 * 
 * author : Joshua Hooker
 * 18 March 2016
 */
public class ScoreBoard : MonoBehaviour {
	// total number of enemies killed
	public ulong enemies_killed;
	// total number of bullets fired
	public ulong bullets_fired;
	// the number of bullets that hit an enemy
	public ulong enemies_hit;
	// total number of scrap collected
	public ulong scrap_collected;
	// total number of e. cores collected
	public ulong ecores_collected;
	//total score (points) the player has currently
	public ulong totalScore;


	// Initialize values
	 void Start() {
		enemies_killed = 0;
		bullets_fired = 0;
		enemies_hit = 0;
		scrap_collected = 0;
		ecores_collected = 0;
		totalScore = 0;
	}


	/* Prints all values out to the commmand line. */
	public void display_scores() {
		Debug.Log("Accuracy = "+ 100*((float)enemies_hit/ (float)bullets_fired ) + "%");
		Debug.Log("Enemies killed: " + enemies_killed + 
				  "\nBullets fired: " + bullets_fired +
				  "\nEnemies hit: " + enemies_hit +
				  "\nScrap Collected: " + scrap_collected + 
				  "\nE. Cores collected: " + ecores_collected);
	}
}
