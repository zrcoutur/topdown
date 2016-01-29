using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
    public int health;
    public int ammo;
    public GameObject laser;
    public GameObject grenade;
    public float grenadeLength;
    private int mode;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        float z = gameObject.transform.position.z;
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newPos = new Vector3(x - .01f, y + .01f, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(x + .01f, y + .01f, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newPos = new Vector3(x - .01f, y - .01f, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(x +.01f, y - .01f, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.UpArrow)){
            Vector3 newPos = new Vector3(x, y + .01f, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 newPos = new Vector3(x, y - .01f, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newPos = new Vector3(x - .01f, y, z);
            gameObject.transform.position = newPos;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(x + .01f, y, z);
            gameObject.transform.position = newPos;
        }
    }
    private void shoot()
    {
        Vector3 pos = Input.mousePosition;
        
        if (mode == 1)
        {
            
        }
    }
}
