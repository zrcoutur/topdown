using UnityEngine;
using System.Collections;

public class MeleeBot : Baseenemy {
	public GameObject Slash;
	// holds the original values of the meleebots speed and attack cooldown
	private float o_speed, o_rate;
	// determines if the meleebot is currently dashing
	private bool dashing;
	// used to keep track of the delay between dashes
	private float dashDelay;

	// Use this for initialization
	void Awake() {
		base.Maxhealth = 25;
		base.health = base.Maxhealth;
		base.speed = 4.5f;
		o_speed = speed;
		base.rate = 1.2f;
		o_rate = rate;
		base.rateVariance = 0f;
		base.range = 4.5f;
		base.damage = 3;
		base.pointValue = 30;

		dashing = false;
		dashDelay = 0f;
	}


	public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = time / 180f;

		Maxhealth = System.Math.Min(32000, Maxhealth + (int)(3.2f * Maxhealth * Mathf.Pow(timeScale, 2f)));
		health = Maxhealth;

		o_speed = Mathf.Min(15f, o_speed + (0.085f * o_speed * timeScale));
		o_rate = Mathf.Max(0.45f, o_rate - (0.03f * o_rate * timeScale));
		damage = System.Math.Min(280, damage + (int)(0.235f * damage * Mathf.Pow(timeScale, 2f)));
	}

	public override void attack()
	{
		Player p = nearest.gameObject.GetComponent<Player>();
		// Play Slash Sound
		CameraRunner.gAudio.PlayOneShot(p.X_Slash, 1.0f);

		// Make Slash Effect, Quaternions are dumb
		var sl = (GameObject)Instantiate(Slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
		// Meleebot's slash deals more damahe then simply running into the player.
		sl.GetComponent<EnemySlash>().damage = (int)(1.6f * base.damage);
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

