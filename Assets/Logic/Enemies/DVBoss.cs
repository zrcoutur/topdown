﻿using UnityEngine;
using System.Collections;

public class DVBoss : Baseenemy
{
    private float Lightning;
    public EnemySlash slash;
    public override void attack()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        double rand1 = ((double)Random.Range(0, 100)) / (100.0);
        if (rand1 <= Lightning)
        {
            lightning();
        }
        else
        {
            var sl = (EnemySlash)Instantiate(slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
            sl.transform.parent = transform;
            sl.damage = damage;
        }

    }

    public override void TimeIncrease(float time) {
        Lightning *= (time / 300);

        var timeScale = 150f;

		if (Maxhealth < 650000) {
			health = health + (int)(3f * health * time / timeScale);
			Maxhealth = health;
		}

		if (speed < 3f) {
			speed = speed + (0.43f * speed * time / timeScale);
		}

		if (damage < 750) {
			damage = damage + (int)(0.9f * damage * time / timeScale);
		}
	}

    // Use this for initialization
    void Start()
    {
        isBoss = true;
		base.Maxhealth = 8000;
		base.health = Maxhealth;
        base.speed = 0.5f;
        base.rate = 2.5f;
        //base.rate = 4f;
        Lightning = 0.01f;
        damage = 20;
    }

    // Update is called once per frame
    void lightning()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);
        foreach (Collider2D coll in hitColliders)
        {
            if (coll.gameObject.tag == "Player")
            {
                int damage = (int)(coll.gameObject.GetComponent<Player>().stats.get_shield() * 1.1) + 1;
                coll.gameObject.GetComponent<Player>().GetHurt(damage);
                Debug.DrawLine(gameObject.transform.position, coll.gameObject.transform.position, Color.yellow, 2f);
                break;
            }
            if (coll.gameObject.tag == "Enemy" && !coll.gameObject.GetComponent<Baseenemy>().isBoss)
            {
                coll.gameObject.GetComponent<Baseenemy>().infected = true;
                Debug.DrawLine(gameObject.transform.position, coll.gameObject.transform.position, Color.yellow, 2f);
            }
        }
    }
    public override void Change()
    { }
}
