using UnityEngine;
using System.Collections;

public class DVBoss : Baseenemy {

	// Use this for initialization
	void awake () {
        base.health = 10000000;
        base.speed =0.5f;
        base.Maxhealth = 10000000;
        base.rate = 2.5f;
        base.rate = 4f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
