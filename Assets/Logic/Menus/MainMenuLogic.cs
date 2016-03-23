using UnityEngine;
using System.Collections;

public class MainMenuLogic : MonoBehaviour {

	public int finalScore;

	public void NewGame() {
		PlayerPrefs.SetInt ("FinalScore", finalScore);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("test");
	}

	public void Multiplayer() {
		
	}

	public void Quit() {
		Application.Quit ();
	}
}
