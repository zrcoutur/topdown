﻿using UnityEngine;
using System.Collections;

public class Slash : PlayerAttack {

	double slashTimer;

	// Use this for initialization
	void Start () {
	
		slashTimer = 0.25;
		base.hitImpulse = Tools.AngleToVec2 (Tools.QuaternionToAngle (transform.rotation) + 90.0f, 300.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
		slashTimer -= Time.deltaTime;

		if (slashTimer <= 0) {

			Destroy (gameObject);

		}


	}

	void OnTriggerEnter2D( Collider2D col ) {

		if (col.tag == "Enemy") {

			//print (col.gameObject);
			col.gameObject.SendMessage ("OnHit", (PlayerAttack)this);

		}

	}

}
