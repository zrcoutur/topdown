using UnityEngine;
using System.Collections;

public class BreakableCrate : MonoBehaviour {
	// Prefabs associated with a crate
	public GameObject crate;
	public GameObject hp_bar;

	private static readonly float MAX_DURABILITY = 10.0f;
	/* The amount of damage a crate can sustain until it breaks */ 
	private int durability;

	// Use this for initialization
	void Start () {
		durability = (int)MAX_DURABILITY;
	}
	
	// Update is called once per frame
	void Update () {
		/* the HP bar shrinks as the crate's durability decreases */
		Transform hp_len = hp_bar.transform;
		var x_scale = ( (float)durability / MAX_DURABILITY );

		hp_len.localScale = new Vector3(x_scale, hp_len.localScale.y, hp_len.localScale.y);

		/* remove the crate when its durability reaches zero */
		if (durability <= 0) { Destroy(crate); }
	}
		
	void OnTriggerEnter2D(Collider2D trigger) {
		var collider = trigger.gameObject;
		/* Reduce durability upon coming in contact with bullets and beam swords */
		if (collider != null) {
			/* bullets and swords deal differing damage */
			if (collider.tag == "bullet_1") {
				durability -= 1;
			} else if (collider.tag == "sword") {
				durability -= 3;
			}

			// Debug.Log("Durability: " + durability + "\n");
		}
	}
}
