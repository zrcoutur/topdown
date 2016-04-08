using UnityEngine;
using System.Collections;

public class MeleeBot : Baseenemy {
	public GameObject Slash;
	// holds the original values of the meleebots speed and attack cooldown
	private float o_speed, o_rate, o_range;
	// determines if the meleebot is currently dashing
	private bool dashing;
	// used to keep track of the delay between dashes
	private float dashDelay;

	// Use this for initialization
	void Awake() {
		base.Maxhealth = 25;
		base.health = base.Maxhealth;
		base.speed = 6f;
		o_speed = speed;
		base.rate = 1.2f;
		o_rate = rate;
		base.rateVariance = 0f;
		base.range = 4.5f;
		o_range = range;
		base.damage = 3;

		dashing = false;
		dashDelay = 0f;
	}


	public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = 150f;

		if (Maxhealth < 32000) {
			health = health + (int)(0.8f * health * time / timeScale);
			Maxhealth = health;
		}

		if (speed < 16f) {
			o_speed = speed + (0.2f * speed * time / timeScale);
		}

		if (damage < 550) {
			damage = damage + (int)(0.55f * damage * time / timeScale);
		}

		if (rate > 0.45f) {
			o_rate = o_rate + (0.05f * rate * time / timeScale);
		}
	}

	public override void attack()
	{
		Player p = nearest.gameObject.GetComponent<Player>();
		// Play Slash Sound
		CameraRunner.gAudio.PlayOneShot(p.X_Slash, 1.0f);

		// Make Slash Effect, Quaternions are dumb
		var sl = (GameObject)Instantiate(Slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
		sl.GetComponent<EnemySlash>().damage = base.damage;
		sl.transform.parent = transform;
		//sl.transform.localScale = gameObject.transform.localScale;
		// Shake camera
		p.cam.AddShake(0.1f);

		// Momentum from swing
		gameObject.GetComponent<Rigidbody2D>().AddForce(Tools.AngleToVec2((gameObject.GetComponent<Rigidbody2D>().rotation * transform.forward).z + 270.0f, 60.0f));
	}

	// Meleebot's will dash at a nearby player, increasing their speed and attack rate for a short period of time
	public override void Change() {
		// Check if a Player is nearby
		if (nearest != null && dashDelay <= 0f) {
			
			Vector2 force = Vector2.zero;
			float dist_near = Vector2.Distance(gameObject.transform.localPosition, nearest.gameObject.transform.localPosition);
			// If the Player is close enough, dash at them
			if (!dashing && Mathf.Abs(dist_near) <= 6.5f) {
				// while dashing speed is increaseing by 80%
				speed = 1.8f * o_speed;
				// while dashing rate of attack cooldown is reducced by 20%
				rate = 0.8f * o_rate;

				dashing = true;
				dashDelay = 10f;
			}

		} else if (dashDelay > 0f) {
			// Stop dashing after 1 second
			if (dashing && dashDelay <= 9f) {
				speed = o_speed;
				rate = o_rate;

				dashing = false;
			}

			// Leap cooldown
			dashDelay -= Time.deltaTime;
		}
	}

}

