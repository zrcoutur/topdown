using UnityEngine;
using System.Collections;

public class CameraRunner : MonoBehaviour {

	public GameObject follow;

	// Global camera variable
	public static AudioSource gAudio;

	float shake;

	// Allows others to shake the camera
	public void AddShake( float s ) {
		shake += s;
	}

	// Use this for initialization
	void Start () {

		shake = 0;

		gAudio = this.GetComponent<AudioSource>();
	
	}

	// Update is called once per frame
	void Update () {

		if (!Time_Count.game_pause) {
			
			var shk_x = 0f;
			var shk_y = 0f;

			if (shake > 0) {

				shake -= 0.1f;
				shk_x = Random.Range (-shake, shake);
				shk_y = Random.Range (-shake, shake);

			}

			transform.position = new Vector3 (
				follow.transform.position.x + shk_x,
				follow.transform.position.y + shk_y,
				transform.position.z
			);
		
		}
	}
}
