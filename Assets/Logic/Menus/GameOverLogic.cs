using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverLogic : MonoBehaviour {

	public Text finalscore_text;

	void Start () {
		finalscore_text.text = "Final Score: " + PlayerPrefs.GetInt ("FinalScore").ToString ();
	}
	
	public void Restart() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("test");
	}

	public void MainMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("mainMenu");
	}
}
