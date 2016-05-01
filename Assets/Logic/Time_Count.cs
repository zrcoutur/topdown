using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class is designed to keep track of how long the player has lasted 
 * in the game and is used by the time display in the game.
 * 
 * author : Joshua Hooker
 * 2 April 2016
 */
public class Time_Count : MonoBehaviour {
	// Total time since the beginning of the round
	public float time;
	// Indicates that the game is paused
	private static bool pause;
	public static bool game_pause { get { return pause; } }

	static Time_Count() {
		pause = false;
	}

	// Use this for initialization
	public void Start () {
		time = 0f;
		// Center the time display
		GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	public void Update () {
		
		if (!pause) {
			// Only count time outside of game being paused
			GetComponent<Text>().text = ((int)time).ToString();
			time += Time.deltaTime;
		}
	}

	/**
	 * Toggle the game pause mode. If the game is currently paused
	 * then the game will resume otherwise the game will be paused.
	 */
	public static void toggle_pause() {
		// Toggle pause mode with 'p'
		pause = !pause;
		// Slow down time if the game is paused
		Time.timeScale = (pause) ? 0.00001f : 1f;
	}

	private string displayCurrentTime() {
		// TODO display time in hh:mm:ss
		return null;
	}
}
