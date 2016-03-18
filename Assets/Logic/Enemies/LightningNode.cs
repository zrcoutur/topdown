using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningNode : MonoBehaviour {
    public GameObject enemy;
    private float time;
    public bool rootNode;
    public bool destroy;
	// Use this for initialization
	void Start () {
        time = 0.5f;
        rootNode = true;
        destroy = false;
	}
	
	// Update is called once per frame
	void Update () {
        time -=Time.deltaTime;
        if (time < 0)
        {
            enemy.GetComponent<Baseenemy>().health = 0;
            destroy = true;

        }
	}
}
