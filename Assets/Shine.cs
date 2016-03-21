using UnityEngine;
using System.Collections;

public class Shine : Animatable {

	// Use this for initialization
	void Start () {
		int randomAngle = UnityEngine.Random.Range(-180, 180);
		this.transform.Rotate(Vector3.forward * randomAngle);
		base.timer = 0.3f;

	}

}
