using UnityEngine;
using System.Collections;

public class DVBoss : Baseenemy {
    private float Lightning;
    public EnemySlash slash;

    public override void attack() {
		
        Random.seed = System.DateTime.Now.Millisecond;
        double rand1 = ((double)Random.Range(0, 100)) / (100.0);

        if (rand1 <= Lightning) {
            //lightning();
        } else {
			
            var sl = (EnemySlash)Instantiate(slash, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation)+180));
            sl.transform.parent = transform;
			sl.GetComponent<SpriteRenderer>().color = Color.red;
			sl.damage = (int)(2f * damage);
        }

    }

    public override void TimeIncrease(float time) {
        Lightning *= (time / 300);

        var timeScale = time / 360f;

		Maxhealth = System.Math.Min(99900, 800 + (int)(400f * timeScale * timeScale + 600f * timeScale) );
		health = Maxhealth;

		speed = Mathf.Min(15f, 5f + (0.7f * timeScale));
		damage = System.Math.Min(130, 27 + (int)(0.125f * timeScale * timeScale + 5f * timeScale) );
	}

    // Use this for initialization
    void Awake()
    {
        isBoss = true;
		base.Maxhealth = 800;
		base.health = Maxhealth;
        base.speed = 5f;
        base.rate = 2.75f;
        Lightning = 0.01f;
        damage = 27;
		base.pointValue = 55;
    }

    // Update is called once per frame
    void lightning() {
		
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);
        foreach (Collider2D coll in hitColliders)
        {
            if (coll.gameObject.tag == "Player") {
				
                int damage = (int)(coll.gameObject.GetComponent<Player>().stats.get_shield() * 1.1) + 1;
                coll.gameObject.GetComponent<Player>().GetHurt(damage);
                Debug.DrawLine(gameObject.transform.position, coll.gameObject.transform.position, Color.yellow, 2f);
                break;
            } else if (coll.gameObject.tag == "Enemy" && !coll.gameObject.GetComponent<Baseenemy>().isBoss) {
                
				coll.gameObject.GetComponent<Baseenemy>().infected = true;
                Debug.DrawLine(gameObject.transform.position, coll.gameObject.transform.position, Color.yellow, 2f);
            }
        }
    }

    public override void Change()
    { }
}
