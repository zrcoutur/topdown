using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningNode : MonoBehaviour {
    public GameObject enemy;
    private float time;
    private List<LightningNode> children=new List<LightningNode>();
    public LightningNode parent;
	// Use this for initialization
	void Start () {
        time = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        time -=Time.deltaTime;
        if (time < 0)
        {
            Destroy(this);
        }
	}
}
