using UnityEngine;
using System.Collections;

public class SpiderDrone : Baseenemy
{
    
	

	// Use this for initialization
	void Awake() {

        base.Maxhealth = 5;
        base.health = 5;
        base.speed = 1.5f;
        base.rate = 2.5f;
        base.rate = 4f;
    }
        

    void TimeIncrease(float time)
    {
        Maxhealth *= (int)(time / 45);
        speed *= (int)(time / 45);
    }
}