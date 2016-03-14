using UnityEngine;
using System.Collections;

public class GrowBoss : SpiderDrone
{
    Vector3 size;
    void Awake()
    {
        Maxhealth = 100;
        health = 100;
        size = transform.lossyScale;
        speed = .5f;
   }

    // Update is called once per frame
    void Update()
    {
        {
            if (health < 0)
            {
                Instantiate(poof, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
            }
            base.flash -= Time.deltaTime;

            if (flash >= 0)
            {
                toggle = 1 - toggle;
                base.Srenderer.color = colors[toggle];
            }
            else
                Srenderer.color = colors[0];

            SearchDelay -= Time.deltaTime;

            if (SearchDelay <= 0)
            {

                SearchDelay = 1.0f;

                nearest = Tools.findNearest(transform.position, "Player");
            }

            if (nearest != null)
            {

                // Calculate angle to target
                Vector2 dir = nearest.position - transform.position;
                float currentAngle = Tools.QuaternionToAngle(transform.rotation);
                float targetAngle = Tools.Vector2ToAngle(dir) + 90.0f;

                if (Mathf.Sqrt(Mathf.Pow(dir.x, 2f) + Mathf.Pow(dir.y, 2f)) <= range)
                {
                    if (timer <= 0)
                    {
                        attack();
                        timer += rate;
                    }
                    timer -= Time.deltaTime;
                }

                // Rotate to face target
                transform.rotation = Tools.AngleToQuaternion(Mathf.MoveTowardsAngle(currentAngle, targetAngle, 3.0f));

                // Move towards target
                body.AddForce(Tools.AngleToVec2(currentAngle - 90.0f, speed));


            }
        }
        transform.localScale = new Vector3(size.x * (Maxhealth / health), size.y * (Maxhealth / health));
    }
}