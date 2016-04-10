using UnityEngine;
using System.Collections;

public class GrowBoss : Baseenemy
{
    private float healRate;
    private float healTime;
	private int max_dmg;
	private float max_speed;
    
	int healAmount;
    Vector3 size;

    void Awake() {
        isBoss = true;
        Maxhealth = 1600;
        health = 1600;
		damage = 43;
		max_dmg = damage;
        size = transform.lossyScale;
        speed = 300f;
		max_speed = speed;
        healRate = 3f;
        healTime = healRate;
		healAmount = 1;
		base.pointValue = 100;
    }

    // Update is called once per frame
    public override void Change() {

        float newSize = Mathf.Min((((float)Maxhealth) / health), 10);
        transform.localScale = new Vector3(size.x * newSize, size.y * newSize);
        // damage and speed scale with health reduction
		damage = (int)( max_dmg * (1f - (3f * (float)health / Maxhealth) / 4f) );
		speed = max_speed * (1f - (9f * (float)health / Maxhealth) / 10f);

		if (healTime <= 0) {
			
			if (health < (Maxhealth / 2)) {
                health += healAmount;
            }
            healTime += healRate;
        }
        healTime -= Time.deltaTime;
    }

    public override void TimeIncrease(float time) {
        // How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
        var timeScale = 270;

		if (Maxhealth < 2000000) {
			Maxhealth = health + (int)(4f * health * time / timeScale);
			health = Maxhealth;
		}

		if (speed < 400f) {
			max_speed = max_speed + (0.06f * speed * time / timeScale);
		}
        
		if (damage < 800) {
			max_dmg = max_dmg + (int)(1.6f * damage * time / timeScale);
		}

		//healRate *= (3 / 4 * Mathf.Pow(2,time));
       	//healAmount += (int)(time / timeScale);
    }

    public override void attack() { }
}