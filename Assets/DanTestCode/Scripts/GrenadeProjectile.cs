using UnityEngine;
using System.Collections;

public class GrenadeProjectile : MonoBehaviour {

    public GameObject explosion;
    private Vector3 end;
	// Use this for initialization
	void Start () {
        end = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject.transform.position.Equals(end)|| (Mathf.Abs(gameObject.transform.position.x)-Mathf.Abs(end.x)<0)|| (Mathf.Abs(gameObject.transform.position.y) - Mathf.Abs(end.y) < 0))
        {
            Instantiate(explosion);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Instantiate(explosion,gameObject.transform.position,gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
