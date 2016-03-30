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

		base.Maxhealth = 25;
		base.health = base.Maxhealth;
        base.speed = 2.0f;
        base.rate = 1.2f;
		base.rateVariance = 0.1f;
        base.range = 10f;
        base.damage = 6;
    }


    public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = 105f;

		if (health < 16000) {
			health = health + (int)(0.75f * health * time / timeScale);
			Maxhealth = health;
		}

		if (speed < 12f) {
			speed = speed + (0.08f * speed * time / timeScale);
		}

		if (damage < 350) {
			damage = damage + (int)(0.65f * damage * time / timeScale);
		}
    }

    public override void attack()
    {
        // Play Shoot Sound
		CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

		// Make Bullet
		var b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
		b.damage = damage / 2;

		// Impart velocity to bullet
		b.GetComponent<Rigidbody2D> ().velocity = Tools.AngleToVec2 ((body.rotation * transform.forward).z - 90.0f, 8.0f);

	}

	public override void Change() {
		// Flyer drones slowdown near player's and speed up otherwise
		if (nearest != null) {
			if (Vector3.Distance(transform.position, nearest.position) <= range) {
				body.drag = 1f;
			} else {
				body.drag = 0f;
			}
		}
	}

}