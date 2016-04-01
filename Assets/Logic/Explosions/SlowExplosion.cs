using UnityEngine;
using System.Collections;

/**
 * This class defines the behavoir of an explosion that creates a slow area where
 * the explosion occured. The Speed focused upgraded grenade launcher shoots
 * grenades that cause there kinds of explosions.
 * 
 * authoer : Joshua Hooker
 * 31 March 2016
 */
public class SlowExplosion : Explosion {

	public GameObject slowArea;

	// Use this for initialization
	void Start() {
		// Play explosion SFX
		CameraRunner.gAudio.PlayOneShot( X_Explode );
	}

	// Update is called once per frame
	void Update () {
		if (animDone <= 0) {
			// Create an area that slows enemies down and damages them overtime
			SlowArea slepl = ((GameObject)Instantiate(slowArea, transform.position, Quaternion.identity)).GetComponent<SlowArea>();
			slepl.setDuration(6f);
			Destroy(this.gameObject);
		} else {
			animDone -= Time.deltaTime;
		}
	}
}

