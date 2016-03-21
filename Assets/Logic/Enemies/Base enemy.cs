﻿using UnityEngine;
using System.Collections;

public abstract class Baseenemy : MonoBehaviour
{
    protected float SearchDelay;
    protected float flash;
    protected int toggle = 1;
	protected int dieState = 0;
	protected float recollideTimer = 0;

    protected Transform nearest;
    protected Rigidbody2D body;
    protected SpriteRenderer Srenderer;

    protected int Maxhealth;
    public GameObject poof;
	 
	public int[] numYielded;
	public GameObject[] yields;
	public float[] chanceYield;
	private Pathfinding2D pf;

	public float speed;
	public int health;
    public float rate = 1f;
	public float rateVariance = 0f;
    public float range = 10f;
    public float timer;
	public int damage = 5;

	//used to add to player score
	public int pointValue;
	public GameObject lastPlayerToAttack;

    public Color[] colors;
    // Use this for initialization
    void Start()
    {
        SearchDelay = 1.0f;
        nearest = null;

        body = GetComponent<Rigidbody2D>();
        Srenderer = GetComponent<SpriteRenderer>();
        timer = rate;
		pf = GetComponent<Pathfinding2D> ();

		pointValue = 25;
    }

    // Update is called once per frame
    void Update()
    {

		//Debug.Log((ulong)pointValue);

		recollideTimer -= Time.deltaTime;
		flash -= Time.deltaTime;

		if (dieState == 1) {
			//update score adding one kill to the player giving killing attack, and add pointValue to score
			lastPlayerToAttack.GetComponent<Player>().score.enemies_killed++;
			lastPlayerToAttack.GetComponent<Player>().score.totalScore += (ulong)pointValue;

			Destroy(gameObject);
			return;
		}

        if (health < 0)
        {
			dieState = 1;

			Instantiate (poof, gameObject.transform.position, Quaternion.Euler (0, 0, 0));

			// Get powerups

			// Iterate through yields list
			for (int j = 0; j < numYielded.GetLength(0); j++) {
				// Number of items to give
				for (int i = 0; i < numYielded [j]; i++) {
					// Percentage chance to give this item
					if (Random.value <= chanceYield[j]) {
						// Create item
						var s = (GameObject) Instantiate (yields [j], transform.position, Quaternion.Euler (0, 0, 0));
						// Fly out randomly
						s.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-250f, 250f), Random.Range (-250f, 250f)));
					}
				}
			}



			return;
        }

		if (flash >= 0)
        {
            toggle = 1 - toggle;
            Srenderer.color = colors[toggle];
        }
        else
            Srenderer.color = colors[0];

        SearchDelay -= Time.deltaTime;

        if (SearchDelay <= 0)
        {

			// Find nearest player
			nearest = Tools.findNearest(transform.position, "Player");

			// Pathfind to player
			pf.FindPath (transform.position, nearest.transform.position);

			// Long re-track timer
			SearchDelay = 5.0f;

			// Much more accurate re-tracking at point-blank
			if ( Vector3.Distance( transform.position, nearest.transform.position ) < 10f)
				SearchDelay -= 4.0f;

        }

		if (nearest != null )
		{

			// Attack check - within range and you attack
			if (Vector3.Distance( transform.position, nearest.position) <= range 
				&& rate != -1f )
			{
				if (timer <= 0)
				{
					attack();
					timer += rate + Random.Range (-rateVariance, rateVariance);
				}
				timer -= Time.deltaTime;
			}

			// Move towards target

			// Check if something obstructs your movement to the target
			if (Vector2.Distance( transform.position, nearest.position ) > 10.0f ) {
				pf.Move ();
			}
			// Otherwise, move straight to the target
			else {
				// Calculate angle to target
				Vector2 dir = (nearest.position - transform.position).normalized;
				float currentAngle = Tools.QuaternionToAngle(transform.rotation);
				float targetAngle = Tools.Vector2ToAngle(dir) + 90.0f;

				// Rotate to face target
				transform.rotation = Tools.AngleToQuaternion(Mathf.MoveTowardsAngle(currentAngle, targetAngle, 3.0f));

				// Move at target
				body.AddForce (dir * GetComponent<Baseenemy>().speed);

			}

			// Check if you have completed your path - search sooner if so
			if (SearchDelay > 0.5f && pf.Path.Count == 0)
				SearchDelay = 0.5f;


		}
    }

	// Bump into walls/player
	void OnCollisionStay2D( Collision2D col ) {

		// Hurt player by contact
		if (col.gameObject.tag == "Player" && recollideTimer <= 0) {
			recollideTimer = 1.0f;
			var p = col.gameObject.GetComponent<Player> ();
			p.GetHurt ( damage );
			col.gameObject.GetComponent<Rigidbody2D> ().AddForce ( new Vector2 (
				( p.transform.position.x - transform.position.x ) * 380.0f,
				( p.transform.position.y - transform.position.y ) * 380.0f)
			);
		}
	}


	void OnHit(PlayerAttack hit)
    {
		//keep track of last player to attack and update their scores
		lastPlayerToAttack = hit.transform.parent.gameObject;
		if (hit is Bullet1 || hit is Slash) {
			hit.transform.parent.gameObject.GetComponent<Player>().score.enemies_hit++;
		}


		// Pushback
		body.AddForce( hit.hitImpulse );

		// Take damage
		health -= hit.damage;

        // Flash
        flash = 0.3f;

    }
    public abstract void TimeIncrease(float time);
    public abstract void attack();
}
