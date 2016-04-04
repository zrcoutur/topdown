using UnityEngine;
using System.Collections;

public class SpiderDrone : Baseenemy
{
    public GameObject Slash;
	private float leapDelay = 5f;
    

    // Use this for initialization
    void Awake() {
		base.Maxhealth = 35;
		base.health = base.Maxhealth;
		base.speed = 3.2f;
        base.rate = -1f;
		base.rateVariance = 0f;
        base.range = 0f;
        base.damage = 8;
    }


    public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = 150f;

		if (Maxhealth < 40000) {
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
			float dist_near = Vector2.Distance(gameObject.transform.localPosition, nearest.gameObject.transform.localPosition);

			// Leap at the closest Player
			if (Mathf.Abs(dist_near) <= 5f) {
				Debug.Log("Leap!");
				leapDelay = 10f; // create a delay between leaps
				GetComponent<Rigidbody2D>().AddForce( 400f * (nearest.gameObject.transform.localPosition - gameObject.transform.localPosition) );
			}

		} else if (leapDelay > 0f) { // leap cooldown
			leapDelay -= Time.deltaTime;
		}
	}

}