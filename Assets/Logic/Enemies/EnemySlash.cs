using UnityEngine;
using System.Collections;

public class EnemySlash : MonoBehaviour {

    double slashTimer;
    public int damage;

    // Use this for initialization
    void Start() {
		slashTimer = 0.25;
    }

    // Update is called once per frame
	void Update() {
		
		if (slashTimer <= 0) {
			Destroy(gameObject);
		} else {
			slashTimer -= Time.deltaTime;
		}
    }

    void OnTriggerEnter2D(Collider2D col) {

        if (col.tag == "Player") {
            col.gameObject.GetComponent<Player>().GetHurt(damage);
        } else if (col.gameObject.GetComponent<Bullet1>() != null || col.GetComponent<Bullet3>() != null) {
			// Enemy slashes block non-piercing bullets
			Destroy(col.gameObject);
		}

    }
}
