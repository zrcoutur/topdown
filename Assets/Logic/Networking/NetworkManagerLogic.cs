using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerLogic : NetworkManager {

	public void StartupHost() {
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame() {
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	void SetIPAddress() {
		string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort() {
		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded(int level) {
		if (level == 1) {
			StartCoroutine(SetupMenuSceneButtons());
		} 
	}

	IEnumerator SetupMenuSceneButtons() {
		yield return new WaitForSeconds (0.5f);
		GameObject.Find ("StartHostButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("StartHostButton").GetComponent<Button> ().onClick.AddListener (StartupHost);

		GameObject.Find ("JoinHostButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("JoinHostButton").GetComponent<Button> ().onClick.AddListener (JoinGame);
	}

	public void MainMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("mainMenu");
	}
}
