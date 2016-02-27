using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public bool north = false;
    public bool south = false;
    public bool east = false;
    public bool west = false;
    private Vector2[] location = new Vector2[4];
    private int i;
    // Use this for initialization
    void Start()
    {

        float yScale = transform.lossyScale.y / 2;
        float xScale = transform.lossyScale.x / 2;
        i = 0;
        if (north)
        {
            location[i] = new Vector2(transform.position.x, transform.position.y + yScale);
            i++;
        }
        if (south)
        {
            location[i] = new Vector2(transform.position.x, transform.position.y - yScale);
            i++;
        }
        if (east)
        {
            location[i] = new Vector2(transform.position.x + xScale, transform.position.y);
            i++;
        }
        if (west)
        {
            location[i] = new Vector2(transform.position.x - xScale, transform.position.y);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {


    }
    public Baseenemy spawn(Baseenemy b)
    {
        Random.seed = System.DateTime.Now.Millisecond + 1;
        int rand = Random.Range(0, i);
        return (Baseenemy)Instantiate(b, location[rand], Quaternion.Euler(0, 0, 0));
    }
}