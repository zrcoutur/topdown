using UnityEngine;
using System.Collections;

public class Animatable : MonoBehaviour {

	public float timer;

	// Use this for initialization
	void Start () {

		timer = 0.4f;

	}

	// Update is called once per frame
	void Update () {

		if (!Time_Count.game_pause) {
			
			timer -= Time.deltaTime;

			if (timer <= 0) {
				Destroy(gameObject);
			}
		}
	}
}
