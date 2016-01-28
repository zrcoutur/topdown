using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {

	double slashTimer;

	// Use this for initialization
	void Start () {
	
		slashTimer = 0.25;

	}
	
	// Update is called once per frame
	void Update () {
	
		slashTimer -= Time.deltaTime;

		if (slashTimer <= 0) {

			Destroy (gameObject);

		}


	}
}
