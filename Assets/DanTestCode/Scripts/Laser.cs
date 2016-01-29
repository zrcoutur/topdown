using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public int damage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
        //Detect entity being hit and deal damage to it
        Destroy(this.gameObject);
    }
}
