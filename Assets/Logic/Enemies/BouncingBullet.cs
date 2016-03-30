﻿using UnityEngine;
using System.Collections;

public class BouncingBullet : Bullet1 {

    private bool Enemy;
    private bool Man;
    private int number;
    void Awake()
    {
        Enemy = false;
        Man = true;
        number = 1;
    }
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	    if (number >= 10)
        {
            Destroy(this);
        }
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Slash")
        {
            gameObject.transform.rotation = col.gameObject.transform.rotation;
            number++;
        }
        if (col.tag == "Block")
        {
            // Make poof
            Instantiate(poof, transform.position, transform.rotation);

            // Play wall hit sound effect
            CameraRunner.gAudio.PlayOneShot(X_Wall_Hit);

            // Destroy self
            Destroy(gameObject);

        }

        if (col.tag == "Enemy")
        {
            if (col.gameObject.name.Substring(0, 2) == "DV")
            {
                this.gameObject.transform.rotation = Tools.AngleToQuaternion(Tools.QuaternionToAngle(this.gameObject.transform.rotation) + 180);
                number++;
            }
            // Tell enemy it's been hit!
            col.gameObject.SendMessage("OnHit", (PlayerAttack)this);

            // Play hit sound effect
            CameraRunner.gAudio.PlayOneShot(X_Enemy_Hit);

            // Make poof
            Instantiate(poof, transform.position, transform.rotation);

            // Destroy self
            Destroy(gameObject);
        }

    }
}
