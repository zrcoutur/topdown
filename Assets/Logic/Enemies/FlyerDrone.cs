using UnityEngine;
using System.Collections;

public class FlyerDrone : Baseenemy
{
    //public GameObject Slash;
	public EnemyBullet bullet;
	public AudioClip X_Bullet_Shoot;
    

    // Use this for initialization
    void Awake()
    {

		base.Maxhealth = 20;
		base.health = base.Maxhealth;
        base.speed = 4f;
        base.rate = 2f;
		base.rateVariance = 0.1f;
        base.range = 8f;
        base.damage = 6;
    }


    public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = 150f;

		if (Maxhealth < 24000) {
			health = health + (int)(0.65f * health * time / timeScale);
			Maxhealth = health;
		}

		if (speed < 20f) {
			speed = speed + (0.15f * speed * time / timeScale);
		}

		if (rate > 0.35f) {
			rate = rate - (0.05f * rate * time / timeScale);
		}

		if (damage < 600) {
			damage = damage + (int)(0.45f * damage * time / timeScale);
		}
    }

    public override void attack()
    {
        // Play Shoot Sound
		CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

		float chance = UnityEngine.Random.value;

		if (chance <= 0.5f) {
			// Fire a single bullet at the player
			var b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation) + 180));
			b.damage = damage / 2;
			b.set_duration(2f);
			// Impart velocity to bullet
			b.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z - 90.0f, 12.0f);
		} else {
			// Fire two bullets at an angle from the direction that the drone is facing
			var b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation) + 180));
			b.damage = damage / 2;
			b.set_duration(2f);
			// Impart velocity to bullet
			b.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z - 70.0f, 12.0f);

			b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation) + 180));
			b.damage = damage / 2;
			b.set_duration(2f);
			// Impart velocity to bullet
			b.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z - 110.0f, 12.0f);
		}
	}

	public override void Change() {
		// Flyer drones slowdown near player's and speed up otherwise
		if (nearest != null) {
			if (Vector3.Distance(transform.position, nearest.position) <= (4 * range / 5)) {
				body.drag = 2.5f;
			} else {
				body.drag = 0.5f;
			}
		}
	}

}