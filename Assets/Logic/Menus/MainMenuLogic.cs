using UnityEngine;
using System.Collections;

public class MainMenuLogic : MonoBehaviour {

	public int finalScore;

	public GameObject menuItems;
	public GameObject controlPanel;

	public void NewGame() {
		PlayerPrefs.SetInt ("FinalScore", finalScore);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("test");
	}

	public void Controls() {
		this.menuItems.SetActive (false);
		this.controlPanel.SetActive (true);
	}

	public void ControlsClose() {
		this.controlPanel.SetActive (false);
		this.menuItems.SetActive (true);
	}

	public void Quit() {
		Application.Quit ();
	}
}
