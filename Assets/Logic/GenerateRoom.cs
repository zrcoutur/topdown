using UnityEngine;
using System.Collections;

public class GenerateRoom : MonoBehaviour {
    static int roomWidth = 3;
    static int roomHeight = 3;
	float tileWidth = 1.5f;
	float tileHeight = 1.5f;
	int[,] roomMatrix;
	GameObject blockToClone;

	public GameObject[,] GeneratedRoom = new GameObject[roomWidth, roomHeight];
    
    // Use this for initializations
    void Start()
    {
		roomMatrix = new int[,] {	{ 1,1,1 },
									{ 0,0,0 },
									{ 1,1,1 }
	};
		blockToClone = GameObject.Find("Block");

		for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomHeight; j++)
            {
				if (roomMatrix[i,j] != 0) {
					GameObject block = (GameObject)Instantiate(blockToClone, new Vector3(j * tileHeight, i * tileWidth, 0), Quaternion.identity);
					block.AddComponent<BoxCollider2D>();
				}
			}
        }


    }
    // Update is called once per frame
    void Update () {
	
	}
}
