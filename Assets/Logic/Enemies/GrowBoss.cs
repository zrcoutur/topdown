using UnityEngine;
using System.Collections;

public class GrowBoss : Baseenemy
{
    Vector3 size;
    void Awake()
    {
        isBoss = true;
        Maxhealth = 100;
        health = 100;
        size = transform.lossyScale;
        speed = .5f;
    }

    // Update is called once per frame
    public override void Change()
    {
        transform.localScale = new Vector3(size.x * (((float)Maxhealth) / health), size.y * (((float)Maxhealth) / health));
    }
    public override void TimeIncrease(float time)
    {
        // How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
        var timeScale = 105f;
        health = health + (int)(0.15f * health * time / timeScale);
        Maxhealth = health;
        speed = speed + (0.1f * speed * time / timeScale);
        damage = damage + (int)(0.1f * damage * time / timeScale);
    }
    public override void attack() { }

}