using UnityEngine;
using System.Collections;

public class HitSpark : Animatable {

	// Use this for initialization
	void Start () {
		int randomAngle = UnityEngine.Random.Range(-20, 20);
		this.transform.Rotate(Vector3.forward * randomAngle);
		base.timer = 0.3f;

	}

}
