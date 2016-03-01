using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shield_Slider_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Slider>().maxValue = Storage.MAX_SHIELD.current();
	}

	// Update is called once per frame
	void Update () {
		if (Storage.Shield_raised) {
			GetComponent<Slider>().maxValue = Storage.MAX_SHIELD.current();
			Storage.Shield_raised = false;
		}
	}
}
