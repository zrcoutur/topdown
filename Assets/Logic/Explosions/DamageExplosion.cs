using UnityEngine;
using System.Collections;

/**
 * A simple expolsion that deals damage to anything within range of the explosion radius.
 * 
 * last modified by : Joshua Hooker
 * 31 March 2016
 */
public class DamageExplosion : Explosion {

	// Use this for initialization
	void Start() {
		// Play explosion SFX
		CameraRunner.gAudio.PlayOneShot( X_Explode );
	}
	
	// Update is called once per frame
	void Update () {

		if (!Time_Count.game_pause) {
			
			if (animDone <= 0) {
				Destroy(this.gameObject);
			} else {
				animDone -= Time.deltaTime;
			}
		}
	}
}

