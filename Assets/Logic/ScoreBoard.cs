using System;
using UnityEngine;

/**
 * A class designed to hold the player's progress in the game in some key areas.
 * 
 * author : Joshua Hooker
 * 18 March 2016
 */
public class ScoreBoard {
	// total number of enemies killed
	public ulong enemies_killed;
	// total number of bullets fired
	public ulong bullets_fired;
	// number of sword attaks
	public ulong sword_attacks;
	// the number of bullets that hit an enemy
	public ulong enemies_hit;
	// number of bullets that hit boxes
	public ulong boxes_hit;
	// total number of scrap collected
	public ulong scrap_collected;
	// total number of e. cores collected
	public ulong ecores_collected;
	// total number of doors opened
	public ulong doors_opened;
	//total score (points) the player has currently
	public ulong totalScore;
	// all time high score
	public ulong highscore;

	// Initialize values
	 void Start() {
		enemies_killed = 0;
		bullets_fired = 0;
		sword_attacks = 0;
		enemies_hit = 0;
		boxes_hit = 0;
		scrap_collected = 0;
		ecores_collected = 0;
		doors_opened = 0;
		totalScore = 0;
		highscore = (ulong)PlayerPrefs.GetInt("HighScore");;

	}


	/* Prints all values out to the commmand line. */
	public void display_scores() {
<<<<<<< HEAD

		if (totalScore > highscore )
		{
			PlayerPrefs.SetInt("HighScore", (int)totalScore);
			Debug.Log("New High Score");
		}


		if ((bullets_fired + sword_attacks) > 0)
		{
			Debug.Log("Accuracy = " + 100 * ((enemies_hit + boxes_hit) / (float)(bullets_fired + sword_attacks)) + "%");
		}
	
		Debug.Log("Enemies killed: " + enemies_killed +
=======
		Debug.Log("Accuracy = "+ 100*((float)enemies_hit/ (float)bullets_fired ) + "%");
		Debug.Log("Enemies killed: " + enemies_killed + 
>>>>>>> 1e59d6c3342627495bf69575f5ecb0ff3a8a457d
				  "\nBullets fired: " + bullets_fired +
				  "\nSword attacks: " + sword_attacks +
				  "\nEnemies hit: " + enemies_hit +
<<<<<<< HEAD
				  "\nScrap Collected: " + scrap_collected +
				  "\nE. Cores collected: " + ecores_collected);
		PlayerPrefs.Save();
=======
				  "\nScrap Collected: " + scrap_collected + 
				  "\nE. Cores collected: " + ecores_collected);
>>>>>>> 1e59d6c3342627495bf69575f5ecb0ff3a8a457d
	}
}
