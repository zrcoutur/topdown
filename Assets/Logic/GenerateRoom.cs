using System;
using System.Collections;
using UnityEngine;

public class GenerateRoom : MonoBehaviour
{

	public GameObject regularWall;
	public GameObject cornerWall;
	public GameObject outerCornerWall;
	public GameObject spawner;
	public GameObject spawnerHandler;
	public GameObject Spaceman;
	public GameObject breakableBox;
	public GameObject unbreakableBox;
	public GameObject mine;
	public GameObject innerDoor;
	public GameObject outerDoor;
	public GameObject doorHandler;
	public GameObject roomFloor;
	public GameObject hallFloorVertical;
	public GameObject hallFloorHorizontile;

	int[,] floor;

	bool instantiate;
	bool teleportedPlayer;

	int dungeonSize;
	int roomWidth;
	int roomHeight;
	int hallLength;
	int spawnerCap;
	int spawnersPlaced;
	float tileSize;

	// Use this for initializations
	void Start()
	{
		instantiate = true;
		teleportedPlayer = false;
		dungeonSize = 10;
		tileSize = 1.6f;
		roomWidth = 9;  //preferably odd
		roomHeight = 9; //preferably odd
		hallLength = 6; //preferably even
		spawnerCap = 3;
		

		floor = makeFloorMatrix(dungeonSize, dungeonSize, 2, 4);

	}

	// Update is called once per frame
	void Update()
	{

		if (instantiate) // create level when instatiate is true
		{
			GameObject handler = (GameObject)Instantiate(spawnerHandler, new Vector3(0, 0, 0), Quaternion.identity);
			String array = "";
			for (int i = 0; i < dungeonSize; i++)
			{
				for (int j = 0; j < dungeonSize; j++)
				{
					array += floor[i, j] + ",";
					int type = 0;
					if (floor[i, j] != 0 && floor[i, j] != 32)
					{
						//find the type of door code by searching through room types 

						int doors = (floor[i, j] - 48);
						
						if (doors < 0)
						{
							doors += 16;
							if (doors < 0)
							{
								type = 1;
							}
							else
							{
								type = 2;
							}
							
						}

						int[,] room = makeRoomMatrix(roomHeight, roomWidth, doors,type);

						makeRoom(i * (roomHeight + hallLength), j * (roomWidth + hallLength), room, doors);
						if (doors != 0)
						{
							makeHall(doors, i * (roomHeight + hallLength), j * (roomWidth + hallLength));
						}
						if (floor[i, j] < 32 && floor[i,j] > 16 )
						{

							//teleport player to first special room, all other special rooms in map get doors
							if (!teleportedPlayer)
							{
								Spaceman.transform.position = new Vector3(((j * (roomWidth + hallLength)) + (roomWidth / 2)) * tileSize, (i * (roomHeight + hallLength) + (roomHeight / 2)) * tileSize, 0);
								teleportedPlayer = true;
							}
							else
							{
								if ((doors & 1) == 1)
								{
									makeDoor((j * (roomWidth + hallLength) + (roomWidth / 2) - 1), (i * (roomHeight + hallLength) + roomHeight - 1), true);
								}
								if ((doors & 2) == 2)
								{
									makeDoor((j * (roomWidth + hallLength) + roomWidth - 1), (i * (roomHeight + hallLength) + (roomHeight / 2) - 1), false);
								}
								if ((doors & 4) == 4)
								{
									makeDoor((j * (roomWidth + hallLength) + (roomWidth / 2) - 1), (i * (roomHeight + hallLength)), true);
								}
								if ((doors & 8) == 8)
								{
									makeDoor((j * (roomWidth + hallLength)), (i * (roomHeight + hallLength) + (roomHeight / 2) - 1), false);
								}
								if (doors != 0)
								{
									GameObject block = (GameObject)Instantiate(doorHandler, new Vector3(tileSize * (j * (roomWidth + hallLength) + (roomWidth / 2)), tileSize * (i * (roomHeight + hallLength) + (roomHeight / 2)), 0), Quaternion.identity);

								}
							}
						}
					}
				}
				//Debug.Log(array);
				array = "";
			}

			instantiate = false;
		}

	}

	void makeHall(int doors, int y, int x)
	{


		//make hallway up
		if ((doors & 1) == 1)
		{

			int tempx = x + (roomWidth / 2) - 2;
			int tempy = y + roomHeight;
			GameObject floor = (GameObject)Instantiate(hallFloorVertical, new Vector3(tileSize * (x + (roomWidth / 2)), tileSize * (y + (roomHeight + hallLength / 2)) - .8f, 0), Quaternion.identity);
			for (int j = tempy; j < tempy + hallLength / 2; j++)
			{
				for (int i = tempx; i < tempx + 5; i += 4)
				{
					if (i == tempx)
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * -90);
						block.AddComponent<BoxCollider2D>();
					}
					else
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * 90);
						block.AddComponent<BoxCollider2D>();
					}

				}
			}
		}
		//make hallway right
		if ((doors & 2) == 2)
		{
			GameObject floor = (GameObject)Instantiate(hallFloorHorizontile, new Vector3(tileSize * (x + roomWidth + hallLength / 2) - .8f, tileSize * (y + (roomHeight / 2)), 0), Quaternion.identity);

			int tempx = x + roomWidth;
			int tempy = y + (roomHeight / 2) - 2;
			for (int j = tempy; j < tempy + 5; j += 4)
			{
				for (int i = tempx; i < tempx + hallLength / 2; i++)
				{
					if (j == tempy)
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.AddComponent<BoxCollider2D>();
					}
					else
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * 180);
						block.AddComponent<BoxCollider2D>();
					}

				}
			}
		}
		//make hallway down
		if ((doors & 4) == 4)
		{

			int tempx = x + (roomWidth / 2) - 2;
			int tempy = y - 1;
			for (int j = tempy; j >= tempy - hallLength / 2; j--)
			{
				for (int i = tempx; i < tempx + 5; i += 4)
				{
					if (i == tempx)
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * -90);
						block.AddComponent<BoxCollider2D>();
					}
					else
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * 90);
						block.AddComponent<BoxCollider2D>();
					}
				}
			}

		}
		//make hallway left
		if ((doors & 8) == 8)
		{
			int tempx = x;
			int tempy = y + (roomHeight / 2) - 2;
			for (int j = tempy; j < tempy + 5; j += 4)
			{
				for (int i = tempx; i >= tempx - hallLength / 2; i--)
				{
					if (j == tempy)
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.AddComponent<BoxCollider2D>();
					}
					else
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * 180);
						block.AddComponent<BoxCollider2D>();
					}
				}
			}

		}

	}
	

	//byte for determining doors; Byte is a boolean string for determining which walls have doors starting 
	//0x0000 = no doors, 0x0001 means one door on the north (up) direction , 0x0010 means one door on the east (left) direction , and so forth clockwise
	int[,] makeRoomMatrix(int height, int width, int door, int roomType)
	{

		int[,] returnMatrix = new int[width, height];
		int[,] prefab = choosePrefabRoom(height, width,roomType);


		for (int i = 0; i < 9; i++)
		{

			for (int j = 0; j < 9; j++)
			{
				returnMatrix[i, j] = prefab[i, j];
			}
		}

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				// create openings on edge of room where there are doors
				if ((x == (width / 2) || x == (width / 2) + 1 || x == (width / 2) - 1) && (((door & 4) == 4 && y == 0)||((door & 1) == 1 &&(y == height - 1)))) //make south ( down ) and north (up) directions open if opening should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if ((y == height / 2 || y == (height / 2) + 1 || y == (height / 2) - 1) && ((x == width - 1 && (door & 2) == 2) || (x == 0 && (door & 8) == 8) )) //make east ( left ) and west (left) directions open if opening should exist
				{
					returnMatrix[x, y] = 0;
				}

				else
				{   // create walls on edge of room where there are no doors and space is empty
					if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1))
					{
						if (returnMatrix[x, y] == 0)
						{
							returnMatrix[x, y] = 1;
						}
					}
					/*
					if ((x == 0 && !(y == 0 || y == height - 1)) || (x == width - 1 && !(y == 0 || y == height - 1)) || (y == 0 && !(x == 0 || x == width - 11)) || (y == height - 1 && !(x == 0 || x == width - 11)))
					{
						int chance = UnityEngine.Random.Range(0, 10);
						if (chance == 0 && spawnerCap > spawnersPlaced)
						{
							returnMatrix[x, y] = 2;
							spawnersPlaced++;
						}

					}
					*/
				}

			}
		}
		return returnMatrix;
	}

	private int[,] choosePrefabRoom(int height, int width,int roomType)
	{
		int b = 5;//code for boxes
		int m = 4;//code for mines
		int x = 3; // code for breakable boxes
		int s = 2; // spawner code
		int[,,] prefabs = {
			{ //prefab 1 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			},
			{ //prefab 2 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,m,0,0,0,0, },
				{0,0,0,b,b,b,0,0,0, },
				{0,0,m,b,0,b,m,0,0, },
				{0,0,0,b,b,b,0,0,0, },
				{0,0,0,0,m,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			},
			{ //prefab 3 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,b,b,0,m,0,b,b,0, },
				{0,b,b,0,0,0,b,b,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,m,0,0,0,0,0,m,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,b,b,0,0,0,b,b,0, },
				{0,b,b,0,m,0,b,b,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			}, 
			{ //prefab 4 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,x,x,0,0,0,0,m,0, },
				{0,x,0,0,0,0,b,0,0, },
				{0,0,0,0,0,b,0,0,0, },
				{0,0,0,0,b,0,0,0,0, },
				{0,0,0,b,0,0,0,0,0, },
				{0,0,b,0,0,0,0,x,0, },
				{0,m,0,0,0,0,x,x,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			},
			{ //prefab 5 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,m,0,0,0,0,0,m,0, },
				{0,0,0,b,0,b,0,0,0, },
				{0,0,b,b,x,b,b,0,0, },
				{0,0,0,x,0,x,0,0,0, },
				{0,0,b,b,x,b,b,0,0, },
				{0,0,0,b,0,b,0,0,0, },
				{0,m,0,0,0,0,0,m,0, },
				{0,0,0,0,s,0,0,0,0, },	//8
			},
				{ //prefab 6 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,0,b,0,0,0,b,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,x,0,0,0,x,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			},
				{ //prefab 7 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,m,0,0,0,0,0,m,0, },
				{0,0,b,b,0,b,b,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,0,0,m,0,0,0,0, },
				{0,0,b,0,0,0,b,0,0, },
				{0,0,b,b,0,b,b,0,0, },
				{0,m,0,0,0,0,0,m,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			},
				{ //prefab 8 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,x,x,x,0,0,0, },
				{0,0,0,x,x,x,0,0,0, },
				{0,0,0,x,x,x,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			},
				{ //prefab 9 for 9X9
				{0,0,0,0,0,0,0,0,0, },	//0
				{0,0,0,0,0,0,0,0,0, },
				{0,b,b,b,x,b,b,b,0, },  
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,b,b,b,x,b,b,b,0, },
				{0,0,0,0,0,0,0,0,0, },
				{0,0,0,0,0,0,0,0,0, },	//8
			}


		};
		int[,] returnArray = new int[9, 9];
		int prefabchoice = 0;
		if (roomType != 1)
		{
			prefabchoice = UnityEngine.Random.Range(0, 8);
			
		}


		for (int i = 0; i < 9; i++)
		{

			for (int j = 0; j < 9; j++)
			{
				returnArray[i, j] = prefabs[prefabchoice, j, i];
			}
		}
		return returnArray;
	}

	//orientationVertical = true, means door is meant for vertical hallways
	void makeDoor(float x, float y, bool orientationVertical)
	{
		GameObject door;
		if (orientationVertical)
		{

			door = (GameObject)Instantiate(outerDoor, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity);
			door.GetComponent<Door>().hinge = 8;
			door.GetComponent<Door>().delay = true;
			door = (GameObject)Instantiate(innerDoor, new Vector3((x + 0.7f) * tileSize, y * tileSize, 0), Quaternion.identity);
			door.GetComponent<Door>().hinge = 8;

			door = (GameObject)Instantiate(outerDoor, new Vector3((x + 2) * tileSize, y * tileSize, 0), Quaternion.identity);
			door.transform.Rotate(Vector3.forward * 180);
			door.GetComponent<Door>().hinge = 2;
			door.GetComponent<Door>().delay = true;
			door = (GameObject)Instantiate(innerDoor, new Vector3((x + 1.3f) * tileSize, y * tileSize, 0), Quaternion.identity);
			door.transform.Rotate(Vector3.forward * 180);
			door.GetComponent<Door>().hinge = 2;
		}
		else
		{
			door = (GameObject)Instantiate(outerDoor, new Vector3(x * tileSize, (y + 2) * tileSize, 0), Quaternion.identity);
			door.transform.Rotate(Vector3.forward * -90);
			door.GetComponent<Door>().hinge = 1;
			door.GetComponent<Door>().delay = true;
			door = (GameObject)Instantiate(innerDoor, new Vector3(x * tileSize, (y + 1.30f) * tileSize, 0), Quaternion.identity);
			door.transform.Rotate(Vector3.forward * -90);
			door.GetComponent<Door>().hinge = 1;

			door = (GameObject)Instantiate(outerDoor, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity);
			door.transform.Rotate(Vector3.forward * 90);
			door.GetComponent<Door>().hinge = 4;
			door.GetComponent<Door>().delay = true;
			door = (GameObject)Instantiate(innerDoor, new Vector3(x * tileSize, (y + 0.7f) * tileSize, 0), Quaternion.identity);
			door.transform.Rotate(Vector3.forward * 90);
			door.GetComponent<Door>().hinge = 4;

		}
	}

	//Places sprites into level to create a room
	//y and x are the position of the bottom left corner of the room
	// roomMatrix comes from the makeRoomMatrix method
	private void makeRoom(int y, int x, int[,] roomMatrix, int door)
	{
		GameObject floor = (GameObject)Instantiate(roomFloor, new Vector3(tileSize * (x + (roomWidth / 2)), tileSize * (y + (roomHeight / 2)), 0), Quaternion.identity);
		for (int i = y; i < roomHeight + y; i++)
		{

			for (int j = x; j < roomWidth + x; j++)
			{

				if (roomMatrix[j - x, i - y] != 0)
				{


					//check if sprite is in corner
					if ((j - x == 0 && i - y == 0) || (j - x == roomHeight - 1 && i - y == 0) || (j - x == 0 && i - y == roomWidth - 1) || (j - x == roomHeight - 1 && i - y == roomWidth - 1))
					{
						GameObject block = (GameObject)Instantiate(outerCornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
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
						if (roomMatrix[j - x, i - y] == 1)
						{ //wall
						  // corner for top halls
							if ((door & 1) == 1 && (j - x == roomWidth / 2 - 2 || j - x == roomWidth / 2 + 2) && i - y == roomWidth - 1)
							{
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();

								if (j - x == roomWidth / 2 - 2)
								{
									corner.transform.Rotate(Vector3.forward * 90);
								}
								else {

								}


							}// corner for bottom halls
							else if ((door & 4) == 4 && (j - x == roomWidth / 2 - 2 || j - x == roomWidth / 2 + 2) && i - y == 0)
							{
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();
								//rotate left and right walls for appearence
								if (j - x == roomWidth / 2 - 2)
								{
									corner.transform.Rotate(Vector3.forward * 180);
								}
								else {
									corner.transform.Rotate(Vector3.forward * -90);
								}
							}// corner for left/east halls
							else if ((door & 2) == 2 && (i - y == roomHeight / 2 - 2 || i - y == roomHeight / 2 + 2) && j - x == roomWidth - 1)
							{
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();
								//rotate left and right walls for appearence
								if (i - y == roomHeight / 2 - 2)
								{
									corner.transform.Rotate(Vector3.forward * -90);
								}
								else {

								}
							}// corner for left/east halls
							else if ((door & 8) == 8 && (i - y == roomHeight / 2 - 2 || i - y == roomHeight / 2 + 2) && j - x == 0)
							{
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();
								//rotate left and right walls for appearence
								if (i - y == roomHeight / 2 - 2)
								{
									corner.transform.Rotate(Vector3.forward * 180);
								}
								else {
									corner.transform.Rotate(Vector3.forward * 90);
								}
							}
							else {
								GameObject block = (GameObject)Instantiate(regularWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								block.AddComponent<BoxCollider2D>();
								Rigidbody2D body = block.GetComponent<Rigidbody2D>();
								//rotate left walls for appearence
								if (j - x == 0)
								{
									block.transform.Rotate(Vector3.forward * -90);

								}//rotate right walls for appearence
								else if (j - x == roomHeight - 1)
								{
									block.transform.Rotate(Vector3.forward * 90);
								}
								else if (i - y == roomWidth - 1)
								{
									block.transform.Rotate(Vector3.forward * 180);
								}
							}


						}
						else if (roomMatrix[j - x, i - y] == 2)
						{ //spawner

							GameObject spawnerBlock = (GameObject)Instantiate(spawner, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
							spawnerBlock.AddComponent<BoxCollider2D>();

							if (j - x == 0)
							{
								spawnerBlock.GetComponent<EnemySpawner>().east = true;
							}
							else if (j - x == roomWidth - 1)
							{
								spawnerBlock.GetComponent<EnemySpawner>().west = true;
							}
							else if (i - y == 0)
							{
								spawnerBlock.GetComponent<EnemySpawner>().north = true;
							}
							else if (i - y == roomHeight - 1)
							{
								spawnerBlock.GetComponent<EnemySpawner>().south = true;
							}

						}
						else if (roomMatrix[j - x, i - y] == 3)
						{ // breakable box
							GameObject boxBlock = (GameObject)Instantiate(breakableBox, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
						}
						else if (roomMatrix[j - x, i - y] == 4)
						{
							GameObject mineObject = (GameObject)Instantiate(mine, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
						}
						else if (roomMatrix[j - x, i - y] == 5)
						{
							GameObject block = (GameObject)Instantiate(unbreakableBox, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
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
		//special room = 16+*
		//circle rooms = 32+*
		//cooridor room = 48+*
		//* = door code in ones place


		int[,] returnMatrix = new int[floorWidth, floorHeight];
		ArrayList listOfCircleEdgeRoomsX = new ArrayList();
		ArrayList listOfCircleEdgeRoomsY = new ArrayList();


		//pick center of circle formation
		int curCenterRoomX = UnityEngine.Random.Range(1, floorWidth - 2);
		int curCenterRoomY = UnityEngine.Random.Range(1, floorHeight - 2); ;

		//create rooms around center
		for (int x = curCenterRoomX - 1; x < curCenterRoomX + 2; x++)
		{
			for (int y = curCenterRoomY - 1; y < curCenterRoomY + 2; y++)
			{

				//check to make sure circle-edge room being added are in the domain and range of floorMatrix
				if ((x >= 0 && y >= 0) && (x < floorWidth && y < floorHeight))
				{
					returnMatrix[y, x] = 32;
					//Add coordinates for pathing to special rooms later
					listOfCircleEdgeRoomsX.Add(y);
					listOfCircleEdgeRoomsY.Add(x);

					//create doors codes for doors
					if (x == curCenterRoomX - 1 && y == curCenterRoomY - 1)
					{
						returnMatrix[y, x] += 3;
					}
					if (x == curCenterRoomX - 1 && y == curCenterRoomY)
					{
						returnMatrix[y, x] += 5;
					}
					if (x == curCenterRoomX - 1 && y == curCenterRoomY + 1)
					{
						returnMatrix[y, x] += 6;
					}
					if (x == curCenterRoomX && y == curCenterRoomY - 1)
					{
						returnMatrix[y, x] += 10;
					}
					///middle room
					if (x == curCenterRoomX && y == curCenterRoomY)
					{

					}
					if (x == curCenterRoomX && y == curCenterRoomY + 1)
					{
						returnMatrix[y, x] += 10;
					}
					if (x == curCenterRoomX + 1 && y == curCenterRoomY - 1)
					{
						returnMatrix[y, x] += 9;
					}
					if (x == curCenterRoomX + 1 && y == curCenterRoomY)
					{
						returnMatrix[y, x] += 5;
					}
					if (x == curCenterRoomX + 1 && y == curCenterRoomY + 1)
					{
						returnMatrix[y, x] += 12;
					}


				}
			}
		}


		//start pathing for each of the special rooms
		//defines position as index in numberOfSpecialRooms
		for (int curSpecialRoom = 0; curSpecialRoom < numberOfSpecialRooms; curSpecialRoom++)
		{
			//Debug.Log("starting iteration :" + curSpecialRoom);
			//pick a circle-edge room
			int curEdgeRoom = UnityEngine.Random.Range(0, listOfCircleEdgeRoomsX.Count);
			int curX = (int)listOfCircleEdgeRoomsY[curEdgeRoom];
			int curY = (int)listOfCircleEdgeRoomsX[curEdgeRoom];

			//make sure edge room cannot be choosen again for special room
			listOfCircleEdgeRoomsY.RemoveAt(curEdgeRoom);
			listOfCircleEdgeRoomsX.RemoveAt(curEdgeRoom);

			int lastPath = 0;
			bool madeFirstStep = false;
			//branch from special room
			for (int pathLength = 0; pathLength < 6; pathLength++)
			{
				int paths = 0;
				

				//check available moves
				//edge of level check
				//not occupied room check
				if (curX + 1 < floorWidth)
				{
					if (returnMatrix[curY, curX + 1] == 0)
					{
						//add right direction from path options
						paths += 2;
					}

				}
				if ((curX - 1 > -1))
				{
					if (returnMatrix[curY, curX - 1] == 0)
					{
						//add left direction from path options
						paths += 8;
					}

				}
				if ((curY + 1 < floorHeight))
				{
					if (returnMatrix[curY + 1, curX] == 0)
					{
						//add up direction from path options
						paths += 1;
					}
				}
				if ((curY - 1 > -1))
				{
					if (returnMatrix[curY - 1, curX] == 0)
					{
						//add down direction from path options
						paths += 4;
					}
				}



				//pick move from available moves
				//picks a code of a door, or code of end of path
				if (paths != 0)
				{
					int randPath = UnityEngine.Random.Range(0, 4);
					randPath = (int)Math.Pow(2, randPath);

					//find random possible path
					//try 50 times
					for (int i = 0; i < 50; i++)
					{
						if (madeFirstStep)
						{
							randPath = UnityEngine.Random.Range(0, 5);
							randPath = (int)Math.Pow(2, randPath);
						}
						else
						{
							randPath = UnityEngine.Random.Range(0, 3);
							randPath = (int)Math.Pow(2, randPath);
						}
						if ((randPath & paths) == 0 && randPath < 16)
						{
							break;
						}
						//set paths to the choosen path i.e. randpath
						paths = randPath;
					}
				}
				else
				{

				}
				madeFirstStep = true;


				//define room
				//room is cooridor
				if ((paths & 1) == 1)
				{
					//add path to current room to next room: up
					returnMatrix[curY, curX] += 1;
					curY += 1;
					returnMatrix[curY, curX] = 52;
					lastPath = 1;
				}
				else if ((paths & 2) == 2)
				{
					//add path to current room to next room: right
					returnMatrix[curY, curX] += 2;
					curX += 1;
					returnMatrix[curY, curX] = 56;
					lastPath = 2;
				}
				else if ((paths & 4) == 4)
				{
					//add path to current room to next room: down
					returnMatrix[curY, curX] += 4;
					curY -= 1;
					returnMatrix[curY, curX] = 49;
					lastPath = 4;
				}
				else if ((paths & 8) == 8)
				{
					//add path to current room to next room: left
					returnMatrix[curY, curX] += 8;
					curX -= 1;
					returnMatrix[curY, curX] = 50;
					lastPath = 8;
				}

				if (paths >= 16 || pathLength == 5)
				{
					if (lastPath == 0)
					{
						returnMatrix[curY, curX] -= 16;
						if (returnMatrix[curY, curX] == 16)
						{
							//pick a random direction to join middle special room to circle formation
							int randPath = UnityEngine.Random.Range(0, 3);
							randPath = (int)Math.Pow(2, randPath);
							returnMatrix[curY, curX] += randPath;

							//join circle formation to the middle special room
							if (randPath == 1)
							{
								returnMatrix[curY + 1, curX] += 4;
							}
							else if (randPath == 2)
							{
								returnMatrix[curY, curX + 1] += 8;
							}
							else if (randPath == 4)
							{
								returnMatrix[curY - 1, curX] += 1;
							}
							else if (randPath == 8)
							{
								returnMatrix[curY, curX - 1] += 2;
							}
						}
					}
					else
					{
						//transform temp cooridor into special room
						if (returnMatrix[curY, curX] > 32)
						{
							returnMatrix[curY, curX] -= 32;
						}
					}
					//Debug.Log("Y,X:" + curY + "," + curX);
					break;
				}
				
			}
		}
		return returnMatrix;
	}
}
