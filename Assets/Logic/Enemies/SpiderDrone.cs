﻿using UnityEngine;
using System.Collections;

public class SpiderDrone : Baseenemy
{
    public GameObject Slash;
    

    // Use this for initialization
    void Awake()
    {
        isBoss = false;
		base.Maxhealth = 35;
		base.health = base.Maxhealth;
        base.speed = 1.5f;
        base.rate = -1f;
		base.rateVariance = 0f;
        base.range = 0f;
        base.damage = 5;
    }


    public override void TimeIncrease(float time)
    {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = 105f;
		health = health + (int)(0.5f * health * time / timeScale);
		Maxhealth = health;
		speed = speed + (0.1f * speed * time / timeScale);
		damage = damage + (int)(0.2f * damage * time / timeScale);
    }

    public override void attack()
    {
        //Player p = nearest.gameObject.GetComponent<Player>();
        // Play Slash Sound
        //p.Paudio.PlayOneShot(p.X_Slash, 1.0f);

        // Make Slash Effect, Quaternions are dumb
        //var sl = (GameObject)Instantiate(Slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
        //sl.GetComponent<EnemySlash>().damage = base.damage;
        //sl.transform.parent = transform;
        //sl.transform.localScale = gameObject.transform.localScale;
        // Shake camera
        //p.cam.AddShake(0.1f);

        // Momentum from swing
       // gameObject.GetComponent<Rigidbody2D>().AddForce(Tools.AngleToVec2((gameObject.GetComponent<Rigidbody2D>().rotation * transform.forward).z + 270.0f, 60.0f));
    }
}