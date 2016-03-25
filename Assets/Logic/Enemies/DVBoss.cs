using UnityEngine;
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
            var sl = (Slash)Instantiate(slash, body.position, transform.rotation);
            sl.transform.parent = transform;
            sl.damage = damage;
        }

    }

    public override void TimeIncrease(float time)
    {
        Lightning *= (time / 100);
        var timeScale = 105f;
        health = health + (int)(0.15f * health * time / timeScale);
        Maxhealth = health;
        speed = speed + (0.1f * speed * time / timeScale);
        damage = damage + (int)(0.1f * damage * time / timeScale);
    }

    // Use this for initialization
    void awake()
    {
        isBoss = true;
        base.health = 10000000;
        base.speed = 0.5f;
        base.Maxhealth = 10000000;
        base.rate = 2.5f;
        base.rate = 4f;
        Lightning = 0.01f;
        damage = 10;
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
