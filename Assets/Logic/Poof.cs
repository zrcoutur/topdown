using UnityEngine;
using System.Collections;

public class Poof : Animatable {

	// Use this for initialization
	void Start () {

		transform.Rotate (new Vector3 (0, 0, Random.Range (0, 360)));
		base.timer = 0.4f;
	
	}

}
