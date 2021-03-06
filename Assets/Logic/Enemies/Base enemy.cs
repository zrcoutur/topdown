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
	public EnemyBullet e_bullet;
	 
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
	public int damage;

	private bool isKiting;
	private bool kiteCW;

	private bool slowed;
	private float slow_duration;

	//used to add to player score
	public int pointValue;
	public GameObject lastPlayerToAttack;

    public Color[] colors;
    // Use this for initialization

    public bool infected;
    public bool isBoss;
    private float timeTillDestroy;

    void Start() {
		isKiting = false;
		kiteCW = false;

		slowed = false;
		slow_duration = 0f;

        SearchDelay = 1.0f;
        nearest = null;

        body = GetComponent<Rigidbody2D>();
        Srenderer = GetComponent<SpriteRenderer>();
        timer = rate;
		pf = GetComponent<Pathfinding2D> ();

        timeTillDestroy = 0.5f;
        infected = false;
    }

    // Update is called once per frame
    public void Update() {

		if (!Time_Count.game_pause) {
		
			// Slow debuff logic
			if (slow_duration >= 0f) {
				slow_duration -= Time.deltaTime;
			} else if (slowed) {
				speed *= 3;
				slowed = false;
			}

			//Debug.Log((ulong)pointValue);

			recollideTimer -= Time.deltaTime;
			flash -= Time.deltaTime;

			if (dieState == 1) {
				//update score adding one kill to the player giving killing attack, and add pointValue to score
				if (lastPlayerToAttack != null) {
					lastPlayerToAttack.GetComponent<Player>().score.enemies_killed++;
					lastPlayerToAttack.GetComponent<Player>().score.totalScore += (ulong)pointValue;
				}

				Destroy(gameObject);
				return;
			}

			if (health < 0) {
				dieState = 1;

				Instantiate(poof, gameObject.transform.position, Quaternion.Euler(0, 0, 0));

				// Get powerups

				// Iterate through yields list
				for (int j = 0; j < numYielded.GetLength(0); j++) {
					// Number of items to give
					for (int i = 0; i < numYielded[j]; i++) {
						// Percentage chance to give this item
						if (Random.value <= chanceYield[j]) {
							// Create item
							var s = (GameObject)Instantiate(yields[j], transform.position, Quaternion.Euler(0, 0, 0));
							// Fly out randomly
							s.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-250f, 250f), Random.Range(-250f, 250f)));
						}
					}
				}



				return;
			}

			if (flash >= 0) {
				toggle = 1 - toggle;
				Srenderer.color = colors[toggle];
			} else
				Srenderer.color = colors[0];

			SearchDelay -= Time.deltaTime;

			if (SearchDelay <= 0) {

				// Find nearest player
				nearest = Tools.findNearest(transform.position, "Player");

				// Pathfind to player
				pf.FindPath(transform.position, nearest.transform.position);

				// Long re-track timer
				SearchDelay = 5.0f;

				// Much more accurate re-tracking at point-blank
				if (Vector3.Distance(transform.position, nearest.transform.position) < 10f)
					SearchDelay -= 4.0f;

			}

			if (nearest != null) {

				// Attack check - within range and you attack
				if (Vector3.Distance(transform.position, nearest.position) <= range
				            && rate != -1f) {
					if (timer <= 0) {
						attack();
						timer += rate + Random.Range(-rateVariance, rateVariance);
					}
					timer -= Time.deltaTime;
				}

				// Move towards target

				// Check if something obstructs your movement to the target
				if (Vector2.Distance(transform.position, nearest.position) > 8.0f) {
					pf.Move();
				}
			// Otherwise, move straight to the target
			else {
					// Calculate angle to target
					Vector2 dir = (nearest.position - transform.position).normalized;
					float currentAngle = Tools.QuaternionToAngle(transform.rotation);
					float targetAngle = Tools.Vector2ToAngle(dir) + 90.0f;

					// Rotate to face target
					transform.rotation = Tools.AngleToQuaternion(Mathf.MoveTowardsAngle(currentAngle, targetAngle, 7.0f * speed));

					if (gameObject.GetComponent<FlyerDrone>() != null) {
						// Logic invovling the movement of the Flyer drone
						float dist = Vector3.Distance(transform.position, nearest.position);

						if (dist < (range - 5.5f)) {
							// Move away from target
							isKiting = false;
							body.AddForce(-dir * (3f + speed / 5));
						} else if (dist >= (range - 5.5f) && dist <= (range - 0.5f)) {
							// Kite the target

							// Stop movement if the drone is not already kiting the player
							if (!isKiting) {
								body.velocity = Vector2.zero;
								isKiting = true;
								// Kite clockwise or counter-clockwise
								kiteCW = UnityEngine.Random.value > 0.5f;
							}

							Vector2 force = Tools.AngleToVec2((body.rotation * transform.forward).z + ((kiteCW) ? -180f : 0f), 2f + speed / 10);
							body.AddForce(force);
						} else {
							// Move at target
							isKiting = false;
							body.AddForce(dir * GetComponent<Baseenemy>().speed);
						}
					} else {
						// Move at target
						body.AddForce(dir * GetComponent<Baseenemy>().speed);
					}

				}
				if (infected) {
					Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);
					int count = 0;
					foreach (Collider2D coll in hitColliders) {
						if (coll.gameObject.tag == "Player") {
							int damage = (int)(coll.gameObject.GetComponent<Player>().stats.get_shield() * 1.1) + 1;
							coll.gameObject.GetComponent<Player>().GetHurt(damage);
							Debug.DrawLine(gameObject.transform.position, coll.gameObject.transform.position, Color.yellow, 2f);
							break;
						}
						if (count == 2) {
							break;
						}
						if (coll.gameObject.tag == "Enemy" && !coll.gameObject.GetComponent<Baseenemy>().isBoss) {
							coll.gameObject.GetComponent<Baseenemy>().infected = true;
							Debug.DrawLine(gameObject.transform.position, coll.gameObject.transform.position, Color.yellow, 2f);
							count++;
						}
					}
					timeTillDestroy -= Time.deltaTime;
				}
				// Check if you have completed your path - search sooner if so
				if (SearchDelay > 0.5f && pf.Path.Count == 0)
					SearchDelay = 0.5f;


			}
			Change();
		}
    }
	// Enemies take damage from explosions
	public void OnTriggerEnter2D(Collider2D trigger) {
		Explosion exl = trigger.gameObject.GetComponent<Explosion>();

		if (exl != null) {
			if (exl._parent != null) {
				// If the explosion was caused by a PLayer's grenade launcher
				exl._parent.gameObject.GetComponent<Player>().score.enemies_hit++;
				lastPlayerToAttack = exl._parent.gameObject;
			}

			health -= exl.getDamage();

			// Flash
			flash = 0.3f;
		}
	}
	// Enemies are slowed down by slow areas
	public void OnTriggerStay2D(Collider2D trigger) {
		if (trigger.gameObject.GetComponent<SlowArea>() != null) {
			if (!slowed) { speed /= 3; }

			slowed = true;
			slow_duration = 3f;
		}
	}

	// Bump into walls/player
    void OnCollisionStay2D(Collision2D col)
    {

		// Hurt player by contact
        if (col.gameObject.tag == "Player" && recollideTimer <= 0)
        {
			recollideTimer = 1.0f;
            var p = col.gameObject;
            p.SendMessage("GetHurt", damage);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(
                (p.transform.position.x - transform.position.x) * 380.0f,
                (p.transform.position.y - transform.position.y) * 380.0f)
			);
		}
	}


	void OnHit(PlayerAttack hit) {

		// The DV boss has a chance to deflect normal and shotgun bullets
		if ((hit is Bullet1 || hit is Bullet3) && GetComponent<DVBoss>() != null) {
			float chance = UnityEngine.Random.value;

			if (chance <= 0.2f) {
				// reflect enemy bullet
				GameObject bullet = hit.gameObject;
				// Delfects an incoming bullet back at an enemy
				EnemyBullet reverse = (EnemyBullet)Instantiate(e_bullet, bullet.transform.position, Quaternion.identity);
				// Sets transfomr's parent for indicating, which player deflected the bullet
				reverse.transform.parent = transform.parent;
				reverse.set_duration(float.MaxValue);
				reverse.damage = damage / 3;
				// Sets bullet trajectory
				reverse.GetComponent<Rigidbody2D>().velocity = -hit.gameObject.GetComponent<Rigidbody2D>().velocity;

				Destroy(hit.gameObject);
			}
			return;
		}

		//keep track of last player to attack and update their scores
		lastPlayerToAttack = hit.transform.parent.gameObject;

		if (hit is Bullet1 || hit is Bullet2 || hit is Bullet3 || hit is Slash) {
			hit.transform.parent.gameObject.GetComponent<Player>().score.enemies_hit++;
		}

		// Pushback
		body.AddForce( hit.hitImpulse );

		// Take damage
		health -= hit.getDamage();

        // Flash
        flash = 0.3f;

		if (hit is Bullet2 && GetComponent<GrowBoss>() != null) {
			// Grow Boss stops piercing bullets
			Destroy(hit.gameObject);
		}
    }

    public abstract void TimeIncrease(float time);
    public abstract void attack();
    public abstract void Change();
}
