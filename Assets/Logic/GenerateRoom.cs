using System;
using System.Collections;
using UnityEngine;

public class GenerateRoom : MonoBehaviour
{

	float tileWidth;
	float tileHeight;

	GameObject regularWall;
	GameObject cornerWall;

	public bool instantiate;
	public int dungeonSize;
	int[,] floor;

	// Use this for initializations
	void Start()
	{
		instantiate = false;
		dungeonSize = 10;
		tileWidth = 1.5f;
		tileHeight = 1.5f;
		floor = makeFloorMatrix(dungeonSize, dungeonSize, 2, 4);

	}

	// Update is called once per frame
	void Update()
	{
		if (instantiate) // create level when instatiate is true
		{

			String array = "";
			for (int i = 0; i < dungeonSize; i++)
			{
				for (int j = 0; j < dungeonSize; j++)
				{
					array += floor[i, j];
					if (floor[i, j] != 0 && floor[i, j] != 2)
					{
						Byte doors = 0;
						if (j - 1 >= 0)
						{
							if (floor[i, j - 1] != 0)
							{
								doors += 8;
							}
						}
						if (j + 1 < dungeonSize)
						{
							if (floor[i, j + 1] != 0)
							{
								doors += 2;
							}
						}
						if (i - 1 >= 0)
						{
							if (floor[i - 1, j] != 0)
							{
								doors += 4;
							}
						}
						if (i + 1 < dungeonSize)
						{
							if (floor[i + 1, j] != 0)
							{
								doors += 1;
							}
						}
						int[,] room = makeRoomMatrix(5, 5, doors);
						makeRoom(i * 10, j * 10, 5, 5, room);
					}
				}
				Debug.Log(array);
				array = "";
			}

			instantiate = false;
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
	//Places sprites into level to create a room
	//y and x are the position of the bottom left corner of the room
	// roomMatrix comes from the makeRoomMatrix method
	private void makeRoom(int y, int x, int roomHeight, int roomWidth, int[,] roomMatrix)
	{
		regularWall = GameObject.Find("Block");
		cornerWall = GameObject.Find("Block");

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

	int[,] makeFloorMatrix(int floorWidth, int floorHeight, int numberOfCircles, int numberOfSpecialRooms)
	{
		//matrix codes
		//nothing = 0
		//special room = 1
		//circle rooms center = 2
		//circle rooms edge = 3
		//normal room = 4

		int[,] returnMatrix = new int[floorWidth, floorHeight];
		ArrayList listOfSpecialRooms = new ArrayList();
		ArrayList listOfCircleEdgeRooms = new ArrayList();

		//insert special rooms into matrix
		//expects nothing in returnMatrix besides special rooms
		for (int i = 0; i < numberOfSpecialRooms; i++)
		{
			int x = UnityEngine.Random.Range(0, floorWidth - 1);
			int y = UnityEngine.Random.Range(0, floorHeight - 1);

			//test code
			if (returnMatrix[y, x] != 1)
			{
				returnMatrix[y, x] = 1;

				//Add the coordinates of special rooms to connect later
				listOfSpecialRooms.Add(y);
				listOfSpecialRooms.Add(x);

			}
			else //if the matrix indices were already filled , retry
			{
				x = UnityEngine.Random.Range(0, floorWidth - 1);
				y = UnityEngine.Random.Range(0, floorHeight - 1);
				i--;
			}

		}

		//insert circle centers
		for (int i = 0; i < numberOfCircles; i++)
		{
			int x = UnityEngine.Random.Range(0, floorWidth - 1);
			int y = UnityEngine.Random.Range(0, floorHeight - 1);
			if (returnMatrix[y, x] != 1 && returnMatrix[y, x] != 2)
			{
				returnMatrix[y, x] = 2;
			}
			else //if the matrix indices were already filled , retry
			{
				x = UnityEngine.Random.Range(0, floorWidth - 1);
				y = UnityEngine.Random.Range(0, floorHeight - 1);
				i--;
			}

		}

		//expande circle centers into circles
		for (int i = 0; i < floorWidth; i++)
		{

			for (int j = 0; j < floorHeight; j++)
			{

				if (returnMatrix[j, i] == 2)
				{

					//create rooms around center
					for (int x = i - 1; x < i + 2; x++)
					{
						for (int y = j - 1; y < j + 2; y++)
						{


							//check to make sure circle-edge room being added is in domain and range of floorMatrix
							if ((x >= 0 && y >= 0) && (x < floorWidth && y < floorHeight))
							{

								//check if room is already made there
								if (returnMatrix[y, x] == 0)
								{
									returnMatrix[y, x] = 3;

									//Add coordinates for pathing to special rooms later
									listOfCircleEdgeRooms.Add(x);
									listOfCircleEdgeRooms.Add(y);
								}
							}
						}
					}
				}
			}
		}

		//start pathing for each of the special rooms
		//defines position as index in numberOfSpecialRooms
		for (int i = 0; i < numberOfSpecialRooms; i++)
		{
			//pick a circle-edge to aim towards 
			//defines position as index in listOfCircleEdgeRooms

			int circleEdgeTarget = UnityEngine.Random.Range(0, listOfCircleEdgeRooms.Count / 2);
			bool foundPath = false;
			int x = (int)listOfSpecialRooms[2 * i + 1];
			int y = (int)listOfSpecialRooms[2 * i];

			//start building x towards a circle
			for (; x != (int)listOfCircleEdgeRooms[circleEdgeTarget * 2];)
			{

				if (x < (int)listOfCircleEdgeRooms[circleEdgeTarget * 2])
				{
					x++;
				}
				else
				{
					x--;
				}

				//if room hasnt been pathed to closer room 
				if (returnMatrix[y, x] == 0)
				{
					//adds new normal room into to create path
					returnMatrix[y, x] = 4;
				}
				else
				{

					foundPath = true;
					break;
				}
			}

			if (!foundPath)
			{
				for (; y != (int)listOfCircleEdgeRooms[circleEdgeTarget * 2 + 1];)
				{
					if (y < (int)listOfCircleEdgeRooms[circleEdgeTarget * 2 + 1])
					{
						y++;
					}
					else
					{
						y--;
					}

					if (returnMatrix[y, x] == 0)
					{
						//adds new normal room into to create path
						returnMatrix[y, x] = 4;
					}
					else
					{
						foundPath = true;
					}
				}
			}

		}

		return returnMatrix;
	}

}
