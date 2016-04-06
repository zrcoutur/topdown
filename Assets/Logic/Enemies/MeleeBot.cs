using UnityEngine;
using System.Collections;

public class MeleeBot : Baseenemy {
	public GameObject Slash;

	// Use this for initialization
	void Awake() {
		base.Maxhealth = 25;
		base.health = base.Maxhealth;
		base.speed = 6f;
		base.rate = 1.2f;
		base.rateVariance = 0f;
		base.range = 4.5f;
		base.damage = 5;
	}


	public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = 150f;

		if (Maxhealth < 32000) {
			health = health + (int)(0.8f * health * time / timeScale);
			Maxhealth = health;
		}

		if (speed < 32f) {
			speed = speed + (0.25f * speed * time / timeScale);
		}

		if (damage < 650) {
			damage = damage + (int)(0.55f * damage * time / timeScale);
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

	// Spider drones will leap at a neraby Player if they are within a certain range
	public override void Change() {
		
	}

}

