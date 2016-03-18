using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LightningTree : MonoBehaviour {
    List<LightningNode> tree;
	// Use this for initialization
	void Start () {
        tree = new List<LightningNode>();
	}
	
	// Update is called once per frame
	void Update () {
        if (tree.Count == 0)
        {
            Destroy(this);
        }
        List<LightningNode> temp = new List<LightningNode>();
        foreach (LightningNode n in tree){

            if (n.rootNode)
            {

                n.rootNode = false;
                var tags1 = GameObject.FindGameObjectsWithTag("Player");
                var tags2 = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject g in tags1)
                {
                    Vector3 t = g.transform.position - n.transform.position;
                    float distance = Mathf.Sqrt(Mathf.Pow(t.x, 2) + Mathf.Pow(t.y,2));
                    if (distance <= 200)
                    {
                        //Player.stats.change_shield((-1) * Player.stats.get_shield() + 10);

                        ///Player p= g.GetComponent<Player>();
                    }
                }
                int count = 0;
                foreach(GameObject g in tags2)
                {
                    Vector3 t = g.transform.position - n.transform.position;
                    float distance = Mathf.Sqrt(Mathf.Pow(t.x, 2) + Mathf.Pow(t.y, 2));

                    if (distance <= 200)
                    {
                        LightningNode x = new LightningNode();
                        x.enemy = g;
                        count++;
                        temp.Add(x);
                    }
                    if (count == 2)
                    {
                        break;
                    }
                }


            }
        }
        foreach(LightningNode x in temp)
        {
            tree.Add(x);
        }
	}
    public void addNode(LightningNode n)
    {
        tree.Add(n);
    }
}
