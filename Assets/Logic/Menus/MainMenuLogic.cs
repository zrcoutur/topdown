using UnityEngine;
using System.Collections;

public class MainMenuLogic : MonoBehaviour {

	public int finalScore;

	public GameObject menuItems;
	public GameObject controlPanel;

	public void NewGame() {
		GetComponent<AudioSource>().Play();
		PlayerPrefs.SetInt ("FinalScore", finalScore);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("test");
	}

	public void Controls() {
		GetComponent<AudioSource>().Play();
		this.menuItems.SetActive (false);
		this.controlPanel.SetActive (true);
	}

	public void ControlsClose() {
		GetComponent<AudioSource>().Play();
		this.controlPanel.SetActive (false);
		this.menuItems.SetActive (true);
	}

	public void Quit() {
		this.GetComponent<AudioSource>().Play();
		Application.Quit ();
	}
}
