using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    public float fuseTime;
    public int damage;
    private float timer;
    public bool triggerHit = false;
	// Use this for initialization
	void Start () {
        timer = fuseTime; 
	}
	
	// Update is called once per frame
	void Update () {
        if (triggerHit)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
		Player is_player = coll.GetComponent<Player>();
        
		if (is_player != null) {
				triggerHit = true;
				is_player.GetHurt(damage);
		}
        //Add similar condition for enemy
    }
}
