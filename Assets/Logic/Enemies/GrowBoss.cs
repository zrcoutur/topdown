using UnityEngine;
using System.Collections;

public class GrowBoss : Baseenemy
{
    private float healRate;
    private float healTime;
    int healAmount;
    Vector3 size;
    void Awake()
    {
        isBoss = true;
        Maxhealth = 100;
        health = 100;
        size = transform.lossyScale;
        speed = .5f;
        healRate = 5f;
        healTime = healRate;
    }

    // Update is called once per frame
    public override void Change()
    {
        float newSize = Mathf.Min((((float)Maxhealth) / health), 10);
        transform.localScale = new Vector3(size.x * newSize, size.y * newSize);
        if (healTime <= 0)
        {
            if (health < 200)
            {
                health += healAmount;
            }
            healTime += healRate;
        }
        healTime -= Time.deltaTime;
    }
    public override void TimeIncrease(float time)
    {
        // How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
        var timeScale = 105f;
        health = health + (int)(0.15f * health * time / timeScale);
        Maxhealth = health;
        speed = speed + (0.1f * speed * time / timeScale);
        damage = damage + (int)(0.1f * damage * time / timeScale);
        healRate *= (3 / 4 * Mathf.Pow(2,time));
        healAmount += (int)(time / timeScale);
    }
    public override void attack() { }

}