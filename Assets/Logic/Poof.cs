using UnityEngine;
using System.Collections;

public class Poof : Animatable {

	// Poof sfx
	public AudioClip X_poof;

	// Use this for initialization
	void Start () {

		transform.Rotate (new Vector3 (0, 0, Random.Range (0, 360)));
		base.timer = 0.4f;

		CameraRunner.gAudio.PlayOneShot (X_poof);
	
	}

}
