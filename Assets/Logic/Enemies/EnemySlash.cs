using UnityEngine;
using System.Collections;

public class EnemySlash : MonoBehaviour {

    double slashTimer;
    public int damage;

    // Use this for initialization
    void Start()
    {

        slashTimer = 0.25;

    }

    // Update is called once per frame
    void Update()
    {

        slashTimer -= Time.deltaTime;

        if (slashTimer <= 0)
        {

            Destroy(gameObject);

        }


    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().GetHurt(damage);
            col.gameObject.SendMessage("OnHit", Tools.AngleToVec2(Tools.QuaternionToAngle(transform.rotation) + 90.0f, 300.0f));
        }

    }
}
