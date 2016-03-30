using UnityEngine;
using System.Collections;

public class Shine : Animatable {

	// Use this for initialization
	void Start () {
		int randomAngle = UnityEngine.Random.Range(0, 5);
		this.transform.Rotate(Vector3.forward * randomAngle * 60);
		base.timer = 0.3f;

	}

}
