using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class is designed to control the number of breakable crates that can spawn
 * with the coordinates [-20,20] x and [-15,15] y.
 * 
 * @author Joshua Hooker
 * 
 * 2 February 2016
 */
public class CrateManager : MonoBehaviour {

	public GameObject crate;
	// the max number of crates that can be on the map at a time
	private static readonly int MAX_CRATES = 20;
	// set of crates in the game
	private static readonly HashSet<GameObject> crates = new HashSet<GameObject>();
	// time between crate spawns
	private static readonly float SPAWN_TIME = 5.0f;
	// the time before the next crate will spawn
	private float spawn_delay;

	// Use this for initialization
	void Start () {
		// set spawn delay
		spawn_delay = SPAWN_TIME;
		// add a number of crates to the world map
		for (int count = 0; count < 3; ++count) {
			GameObject new_obj = createCrate (Random.Range (-20, 20), Random.Range (-15, 15));

			if (new_obj != null) {
				crates.Add (new_obj);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		spawn_delay -= Time.deltaTime;

		if (spawn_delay <= Time.deltaTime) {
			spawn_delay = SPAWN_TIME; // reset spawn delay

			removeCrates();
			// add another crate somewhere within a set coordinate range
			if (crates.Count < MAX_CRATES) {
				GameObject new_obj = createCrate (Random.Range (-20, 20), Random.Range (-15, 15));

				if (new_obj != null) {
					crates.Add (new_obj);
				}
			}
		}
	}

	/* Removes any crates from the set that have been destroyed by the player */
	void removeCrates() {
		HashSet<GameObject> toRemove = new HashSet<GameObject>();
		// identify destroyed crates
		foreach (GameObject obj in crates) {
			if (obj == null) {
				toRemove.Add (obj);
			} else if (obj.GetComponent<BreakableCrate>().collision_tag) {
				Destroy(obj);
				toRemove.Add(obj);
			}
		}
		// remove any crates that have been destroyed from the set
		foreach (GameObject obj in toRemove) {
			crates.Remove(obj);
		}
	}

	/* create a crate at the specified x and y cooridinates */
	GameObject createCrate(int x, int y) {
		GameObject obj = (GameObject)GameObject.Instantiate (crate, new Vector3 (x, y, 5), Quaternion.identity);
		BreakableCrate bc = obj.GetComponent<BreakableCrate>();

		if (bc != null && bc.collision_tag) {
			Debug.Log ("collision\n");
			return null;
		} else {
			return obj;
		}
	}
}
