using UnityEngine;
using System.Collections;
<<<<<<< HEAD

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
=======
using System;

public class GenerateRoom : MonoBehaviour
{

	float tileWidth;
	float tileHeight;

	GameObject regularWall;
	GameObject cornerWall;

	public int coordinateX;
	public int coordinateY;
	public bool instantiate;
	public byte door;
	public int size;
	// Use this for initializations
	void Start()
	{
		instantiate = false;
		door = 15;
		size = 5;
		tileWidth = 1.5f;
		tileHeight = 1.5f;
		regularWall = GameObject.Find("Block");
		cornerWall = GameObject.Find("Block");

	}

	// Update is called once per frame
	void Update()
	{
		if (instantiate) // create level when instatiate is true
		{
			for (int i = 0; i < 12 * size; i += size + 3)
			{
				for (int j = 0; j < 12 * size; j += size + 3)
				{
					int[,] roomMatrix = makeRoomMatrix(size, size, door);
					makeRoom(coordinateY + i, coordinateX + j, size, size, roomMatrix);
					instantiate = false;
				}
			}
		}

	}
	//byte for determining doors; Byte is a boolean string for determining which walls have doors starting 
	//0x0000 = no doors, 0x0001 means one door on the north (up) direction , 0x0010 means one door on the east (left) direction , and so forth clockwise
	int[,] makeRoomMatrix(int height, int width, byte door)
	{
		int[,] returnMatrix = new int[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{

				if (y == height - 1 && x == width / 2 && (door & 1) == 1) //make north ( up ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if (y == 0 && x == width / 2 && (door & 4) == 4) //make south ( down ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if (y == height / 2 && x == width - 1 && (door & 2) == 2) //make east ( left ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if (y == height / 2 && x == 0 && (door & 8) == 8) //make east ( left ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else // create walls on edge of room where there are no doors
				{
					if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1))
					{
						returnMatrix[x, y] = 1;
					}

				}
			}
		}
		return returnMatrix;
	}

	private void makeRoom(int y, int x, int roomHeight, int roomWidth, int[,] roomMatrix)
	{

		for (int i = y; i < roomWidth + y; i++)
		{

			for (int j = x; j < roomHeight + x; j++)
			{

				if (roomMatrix[j - x, i - y] != 0)
				{


					//check if sprite is in corner
					if ((j - x == 0 && i - y == 0) || (j - x == roomHeight - 1 && i - y == 0) || (j - x == 0 && i - y == roomWidth - 1) || (j - x == roomHeight - 1 && i - y == roomWidth - 1))
					{
						GameObject block = (GameObject)Instantiate(cornerWall, new Vector3(j * tileHeight, i * tileWidth, 0), Quaternion.identity);
						block.AddComponent<BoxCollider2D>();
						Rigidbody2D body = block.GetComponent<Rigidbody2D>();

						//rotate corners for appearance with bottom right corner as default
						if (j - x == 0 && i - y != 0)
						{
							block.transform.Rotate(Vector3.forward * -90);
						}
						else if (j - x != 0 && i - y == 0)
						{
							block.transform.Rotate(Vector3.forward * 90);
						}
						else if (j - x != 0 && i - y != 0)
						{
							block.transform.Rotate(Vector3.forward * 180);
						}
					}
					else // if sprite coordinates are not in corner
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(j * tileHeight, i * tileWidth, 0), Quaternion.identity);
						block.AddComponent<BoxCollider2D>();
						Rigidbody2D body = block.GetComponent<Rigidbody2D>();
						//rotate left and right walls for appearence
						if (j - x == 0 || j - x == roomHeight - 1)
						{
							block.transform.Rotate(Vector3.forward * -90);
						}
					}
				}
			}
		}
	}
>>>>>>> Tiled room Gen
