﻿using UnityEngine;
using System.Collections;

public class SpiderDrone : Baseenemy
{
    public GameObject Slash;
	private float leapDelay = 9f;
	private float leapHalt = 0f;
    

    // Use this for initialization
    void Awake() {
		base.Maxhealth = 16;
		base.health = base.Maxhealth;
		base.speed = 3.2f;
        base.rate = -1f;
		base.rateVariance = 0f;
        base.range = 0f;
        base.damage = 3;
		base.pointValue = 15;
    }


    public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = time / 180f;

		Maxhealth = System.Math.Min(3500, 16 + (int)(8f * timeScale * timeScale + 12f * timeScale) );
		health = Maxhealth;

		speed = Mathf.Min(16f, 3.2f + (0.64f * timeScale));
		damage = System.Math.Min(115, 3 + (int)(0.13f * timeScale * timeScale + 3f * timeScale) );
    }

    public override void attack()
    {
        Player p = nearest.gameObject.GetComponent<Player>();
        // Play Slash Sound
        //p.Paudio.PlayOneShot(p.X_Slash, 1.0f);

        // Make Slash Effect, Quaternions are dumb
        //var sl = (GameObject)Instantiate(Slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
        //sl.GetComponent<EnemySlash>().damage = base.damage;
        //sl.transform.parent = transform;
        //sl.transform.localScale = gameObject.transform.localScale;
        // Shake camera
        p.cam.AddShake(0.1f);

        // Momentum from swing
        gameObject.GetComponent<Rigidbody2D>().AddForce(Tools.AngleToVec2((gameObject.GetComponent<Rigidbody2D>().rotation * transform.forward).z + 270.0f, 60.0f));
    }

	// Spider drones will leap at a neraby Player if they are within a certain range
    public override void Change() {
		// Check if a Player is nearby
		if (nearest != null && leapDelay <= 0f) {
			float chance = UnityEngine.Random.value;
			// Spider Drones do not always leap
			if (chance <= 0.1f) {
				Vector2 force = Vector2.zero;
				float dist_near = Vector2.Distance(gameObject.transform.localPosition, nearest.gameObject.transform.localPosition);

				if (Mathf.Abs(dist_near) <= 2.5f) {
					body.velocity = Vector2.zero;
					// Leap backwards
					force = -900f * (nearest.gameObject.transform.localPosition - gameObject.transform.localPosition);
					leapDelay = 2f + UnityEngine.Random.Range(-1f, 1f);
				} else if (Mathf.Abs(dist_near) <= 7.5f) {
					body.velocity = Vector2.zero;
					// Leap at the closest Player
					force = 300f * (nearest.gameObject.transform.localPosition - gameObject.transform.localPosition);
					leapDelay = 7f + UnityEngine.Random.Range(-2f, 2f);
					leapHalt = 1f;
				}

				GetComponent<Rigidbody2D>().AddForce(force);
			}
		} else if (leapDelay > 0f) {
			// Stop the Spider drone mid-leap
			if (leapHalt <= 0f) {
				body.velocity = Vector2.zero;
				leapHalt = float.MaxValue;
			} else {
				leapHalt -= Time.deltaTime;
			}
			
			// Leap cooldown
			leapDelay -= Time.deltaTime;
		}
	}

}