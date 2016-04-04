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

	// Use this for initialization
	public void Start () {
		// Center the time display
		GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	public void Update () {
		GetComponent<Text>().text = ((int)Time.fixedTime).ToString();
	}

	private string displayCurrentTime() {
		// TODO display time in hh:mm:ss
		return null;
	}
}
