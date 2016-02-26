using UnityEngine;
using System.Collections;

public class BaseMine : MonoBehaviour {

	public Explosion expl;

	void OnTriggerEnter2D(Collider2D coll) {
		Player is_player = coll.GetComponent<Player>();

		if (is_player != null) {

			var ex = (Explosion) Instantiate (expl, transform.position, transform.rotation);
			Destroy (this.gameObject);
		}
		//Add similar condition for enemy
	}
}