using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    public Bullet1 bullet;
    public Color[] colors;
    protected SpriteRenderer Srenderer;
    int health;
    bool notBroken;
    int numBroke;
    float attackrate;
    Player owner;
    GameObject target;
	// Use this for initialization
	void Start () {
        notBroken = false;
        health = 0;
        numBroke = 0;
        Srenderer = GetComponent<SpriteRenderer>();
        Srenderer.color = colors[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (notBroken)
        {
            if (target == null)
            {
                target = targeting();
            }
            if (target == null)
            {
                return;
            }
            //Debug.Log(target.name);
            var currentAng = GetComponent<Rigidbody2D>().rotation;
            var look = target.transform.position - gameObject.transform.position;
            var targetAng =270+Tools.Vector2ToAngle(look);
            Debug.Log(targetAng);
            Debug.Log(currentAng);
            GetComponent<Rigidbody2D>().MoveRotation(Mathf.MoveTowardsAngle(currentAng, targetAng, 360));
            if (attackrate >= 2.5)
            {
                var b1 = (Bullet1)Instantiate(bullet, gameObject.transform.position, transform.rotation);

                b1.transform.parent = owner.transform;


                b1.setDamage((int)(20*Mathf.Pow(1.25f,numBroke)));

                // Mildly shake camera

                // Calculate bullet's velocity

                // Shot spread range.
                var spread = Random.Range(-3.0f, 3.0f);

                // Set final velocity based on travel angle
                b1.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((GetComponent<Rigidbody2D>().rotation * transform.forward).z + 90.0f + spread, 15.0f);

                attackrate = 0f;
                target = targeting();
            }
            attackrate += Time.deltaTime;

            if (health <= 0)
            {
                gameObject.tag = "broken";
                notBroken = false;
                Srenderer.color = colors[0];
                target = null;
            }
        }
        else
        {
                bool tryOpen = false;
                bool paidScrap = false;
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);

                foreach (Collider2D col in hitColliders)
                {
                    if (col.tag == "sword")
                    {
                        tryOpen = true;
                        break;
                    }
                }
                if (tryOpen)
                {
                    foreach (Collider2D col in hitColliders)
                    {
                        if (col.tag == "Player")
                        {
                            if (col.gameObject.GetComponent<Player>().stats.get_scrap() >= 20 * Mathf.Pow(1.25f, numBroke))
                            {
                                col.gameObject.GetComponent<Player>().stats.change_scrap(-(int)(20 * Mathf.Pow(1.25f, numBroke)));
                                paidScrap = true;
                                owner=col.gameObject.GetComponent<Player>();

                            }
                            else
                            {
                                tryOpen = false;
                            }
                        }
                    }
                }
                if (paidScrap)
                {
                    gameObject.tag = "Player";
                    notBroken = true;
                    health = (int)(20 * Mathf.Pow(1.25f, numBroke));
                    Srenderer.color = colors[1];
                target = targeting();
                }
            }
    }
    public void GetHurt(int damage)
    {
        health -= damage;
    }
    private GameObject targeting()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);
        int i = 0;
        int j = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Enemy")
            {
                j = i;
                break;
            }
            i++;
        }
        if (hitColliders[j].gameObject.tag != "Enemy")
        {
            return null;
        }
        return hitColliders[j].gameObject;
    }
}
