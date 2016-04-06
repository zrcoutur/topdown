using UnityEngine;
using System.Collections;

public class GrowBoss : Baseenemy
{
    private float healRate;
    private float healTime;
    int healAmount;
    Vector3 size;

    void Awake() {
        isBoss = true;
        Maxhealth = 900;
        health = 900;
		damage = 36;
        size = transform.lossyScale;
        speed = 0.45f;
        healRate = 3f;
        healTime = healRate;
		healAmount = 1;
    }

    // Update is called once per frame
    public override void Change() {

        float newSize = Mathf.Min((((float)Maxhealth) / health), 10);
        transform.localScale = new Vector3(size.x * newSize, size.y * newSize);
        if (healTime <= 0)
        {
			if (health < (Maxhealth / 2)) {
                health += healAmount;
            }
            healTime += healRate;
        }
        healTime -= Time.deltaTime;
    }

    public override void TimeIncrease(float time) {
        // How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
        var timeScale = 600f;

		if (Maxhealth < 2000000) {
			health = health + (int)(4f * health * time / timeScale);
			Maxhealth = health;
		}

		if (speed < 2.5f) {
			speed = speed + (0.4f * speed * time / timeScale);
		}
        
		if (damage < 400) {
			damage = damage + (int)(1.6f * damage * time / timeScale);
		}

		//healRate *= (3 / 4 * Mathf.Pow(2,time));
       	//healAmount += (int)(time / timeScale);
    }

    public override void attack() { }
}