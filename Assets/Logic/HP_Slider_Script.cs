using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HP_Slider_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Slider>().maxValue = Storage.MAX_HEALTH.current();
	}
	
	// Update is called once per frame
	void Update () {
		if (Storage.HP_raised) {
			GetComponent<Slider>().maxValue = Storage.MAX_HEALTH.current();
			Storage.HP_raised = false;
		}
	}
}
