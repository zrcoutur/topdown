using UnityEngine;
using System.Collections;

public class energy_core_script : MonoBehaviour {

	private int core_value;

	// Use this for initialization
	void Start () {
		core_value = UnityEngine.Random.Range(0, 5);
	}

	public int retrieve_core() {
		Destroy(this);
		return core_value;
	}
}
