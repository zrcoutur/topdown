﻿using UnityEngine;
using System.Collections;

public class MainMenuLogic : MonoBehaviour {

	public int finalScore;

	public void NewGame() {
		PlayerPrefs.SetInt ("FinalScore", finalScore);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("test");
	}
}
