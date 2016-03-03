using UnityEngine;
using System.Collections;

public class SpiderDrone : Baseenemy
{
    public GameObject Slash;
    

    // Use this for initialization
    void Awake()
    {

        base.Maxhealth = 5;
        base.health = 5;
        base.speed = 1.5f;
        base.rate = 2.5f;
        base.range = 40f;
        base.damage = 10;
    }


    public override void TimeIncrease(float time)
    {
        Maxhealth *= (int)(time / 45);
        speed *= (int)(time / 45);
        damage *= (int)(time / 45);
    }

    public override void attack()
    {
        Debug.Log("Attack Right");
        Player p = nearest.gameObject.GetComponent<Player>();
        // Play Slash Sound
        //p.Paudio.PlayOneShot(p.X_Slash, 1.0f);

        // Make Slash Effect, Quaternions are dumb
        var sl = (GameObject)Instantiate(Slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
        sl.GetComponent<EnemySlash>().damage = base.damage;
        sl.transform.parent = transform;
        //sl.transform.localScale = gameObject.transform.localScale;
        // Shake camera
        p.cam.AddShake(0.1f);

        // Momentum from swing
        gameObject.GetComponent<Rigidbody2D>().AddForce(Tools.AngleToVec2((gameObject.GetComponent<Rigidbody2D>().rotation * transform.forward).z + 90.0f, 120.0f));
    }
}