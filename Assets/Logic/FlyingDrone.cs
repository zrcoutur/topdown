using UnityEngine;
using System.Collections;

public class FlyingDrone : MonoBehaviour
{

	float SearchDelay;
	float flash;
	int toggle = 1;

	Transform nearest;
	Rigidbody2D body;
	SpriteRenderer Srenderer;

	public int health;
	private int Maxhealth;

	public Color[] colors;

	// Use this for initialization
	void Start()
	{

		SearchDelay = 0.5f;
		nearest = null;

		body = GetComponent<Rigidbody2D>();
		Srenderer = GetComponent<SpriteRenderer>();

	}

	// Update is called once per frame
	void Update()
	{

		flash -= Time.deltaTime;

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

			SearchDelay = 1.0f;

			nearest = Tools.findNearest(transform.position, "Player");
		}

		if (nearest != null)
		{

			// Calculate angle to target
			Vector2 dir = nearest.position - transform.position;
			float currentAngle = Tools.QuaternionToAngle(transform.rotation);
			float targetAngle = Tools.Vector2ToAngle(dir) + 90.0f;

			// Rotate to face target
			transform.rotation = Tools.AngleToQuaternion(Mathf.MoveTowardsAngle(currentAngle, targetAngle, 3.0f));

			// Move towards target
			body.AddForce(Tools.AngleToVec2(currentAngle - 90.0f, 1.5f));


		}

	}

	void OnHit(Vector2 forceVector)
	{

		// Pushback
		body.AddForce(forceVector);

		// Flash
		flash = 0.3f;


	}

}