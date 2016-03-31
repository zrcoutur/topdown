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
	int itemCap;

	float tileSize;

	// Use this for initializations
	void Start()
	{
		instantiate = true;
		teleportedPlayer = false;
		dungeonSize = 5;
		tileSize = 1.6f;
		roomWidth = 9;	//preferably odd
		roomHeight = 9;	//preferably odd
		hallLength = 6; //preferably even
		spawnerCap = 2;
		itemCap = 5;

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
					array += floor[i, j];
					if (floor[i, j] != 0 && floor[i, j] != 2)
					{
						Byte doors = 0;
						if (j - 1 >= 0)
						{
							if (floor[i, j - 1] != 0 && floor[i, j - 1] != 2)
							{
								doors += 8;
							}
						}
						if (j + 1 < dungeonSize)
						{
							if (floor[i, j + 1] != 0 && floor[i, j + 1] != 2)
							{
								doors += 2;
							}
						}
						if (i - 1 >= 0)
						{

							if (floor[i - 1, j] != 0 && floor[i - 1, j] != 2)
							{
								doors += 4;
							}
						}
						if (i + 1 < dungeonSize)
						{

							if (floor[i + 1, j] != 0 && floor[i + 1, j] != 2)

							{
								doors += 1;
							}
						}
						int[,] room = makeRoomMatrix(roomHeight, roomWidth, doors);

						makeRoom(i * (roomHeight + hallLength) , j * (roomWidth + hallLength), room, doors);
						if (doors != 0)
						{
							makeHall(doors, i * (roomHeight + hallLength), j * (roomWidth + hallLength));
						}
						if ( floor[i,j] == 1)
						{

							//teleport player to first special room, all other special rooms in map get doors
							if (!teleportedPlayer ) {
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
									makeDoor((j * (roomWidth + hallLength) + roomWidth - 1), (i * (roomHeight + hallLength) + (roomHeight/2) - 1), false);
								}
								if ((doors & 4) == 4)
								{
									makeDoor((j * (roomWidth + hallLength) + (roomWidth / 2) - 1), (i * (roomHeight + hallLength) ), true);
								}
								if ((doors & 8) == 8)
								{
									makeDoor((j * (roomWidth + hallLength) ), (i * (roomHeight + hallLength) + (roomHeight / 2) - 1), false);
								}
								if (doors != 0)
								{
									GameObject block = (GameObject)Instantiate(doorHandler, new Vector3(tileSize * (j * (roomWidth + hallLength) + (roomWidth / 2)), tileSize * (i * (roomHeight + hallLength) + (roomHeight / 2) ), 0), Quaternion.identity);

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

	void makeHall(byte doors, int y , int x)
	{
		

		//make hallway up
		if ( (doors & 1) == 1)
		{
			
			int tempx = x + (roomWidth / 2) - 2;
			int tempy = y + roomHeight;
			GameObject floor = (GameObject)Instantiate(hallFloorVertical, new Vector3(tileSize * (x + (roomWidth/2)), tileSize * (y + (roomHeight + hallLength/2))-.8f, 0) , Quaternion.identity);
			for (int j = tempy; j < tempy + hallLength/2 ; j++)
			{
				for (int i = tempx; i < tempx + 5 ; i+=4)
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
			GameObject floor = (GameObject)Instantiate(hallFloorHorizontile, new Vector3(tileSize * (x + roomWidth + hallLength/2) - .8f, tileSize * (y + (roomHeight/2 )), 0), Quaternion.identity);
			
			int tempx = x + roomWidth;
			int tempy = y  + (roomHeight / 2) - 2;
			for (int j = tempy; j < tempy + 5; j += 4)
			{
				for (int i = tempx; i < tempx + hallLength/2; i ++)
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
				for (int i = tempx; i >= tempx - hallLength / 2 ; i--)
				{
					if (j == tempy)
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.AddComponent<BoxCollider2D>();
					}
					else
					{
						GameObject block = (GameObject)Instantiate(regularWall, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
						block.transform.Rotate(Vector3.forward * 180 );
						block.AddComponent<BoxCollider2D>();
					}
				}
			}

		}

	}

	//byte for determining doors; Byte is a boolean string for determining which walls have doors starting 
	//0x0000 = no doors, 0x0001 means one door on the north (up) direction , 0x0010 means one door on the east (left) direction , and so forth clockwise
	int[,] makeRoomMatrix(int height, int width, byte door)
	{
		int[,] returnMatrix = new int[width, height];
		int spawnersPlaced = 0;
		int boxesPlaced = 0;
		int minesPlaced = 0;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				// create openings on edge of room where there are doors
				if (y == height - 1 && ( x == width / 2 || x == (width / 2) + 1 || x == (width / 2) - 1 ) && (door & 1) == 1) //make north ( up ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if (y == 0 && (x == (width / 2) || x == (width / 2) + 1 || x == (width / 2) - 1 ) && (door & 4) == 4) //make south ( down ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if ((y == height / 2 || y == (height / 2) + 1 || y == (height / 2) - 1) && x == width - 1 && (door & 2) == 2) //make east ( left ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else if ((y == height / 2 || y == (height / 2) + 1 || y == (height / 2) - 1) && x == 0 && (door & 8) == 8) //make east ( left ) direction door if it should exist
				{
					returnMatrix[x, y] = 0;
				}
				else 
				{	// create walls on edge of room where there are no doors
					if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1))
					{
						returnMatrix[x, y] = 1;
					}
					else // put stuff in the middle of the room
					{
						int chance = UnityEngine.Random.Range(0, 100);
						if (chance <= 8 && itemCap > (boxesPlaced + minesPlaced)) {
							returnMatrix [x, y] = 3;
							boxesPlaced++;
						} else if (chance <= 13 && itemCap > (boxesPlaced + minesPlaced)) {
							returnMatrix [x, y] = 4;
							minesPlaced++;
						}
					}
					//
					if ( (x == 0 && !(y == 0 || y == height - 1)) ||  (x == width - 1 && !(y == 0 || y == height - 1)) || (y == 0 && !(x == 0 || x == width - 11)) || (y == height - 1 && !(x == 0 || x == width - 11)))
					{
						int chance = UnityEngine.Random.Range(0, 10);
						if (chance == 0 && spawnerCap > spawnersPlaced)
						{
							returnMatrix[x, y] = 2;
							spawnersPlaced++;
						}
						
					}
					
				}
			}
		}
		return returnMatrix;
	}

	//orientationVertical = true, means door is meant for vertical hallways
	void makeDoor(float x, float y, bool orientationVertical)
	{
		GameObject door;
		if (orientationVertical) {
			
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
	private void makeRoom(int y, int x, int[,] roomMatrix, byte door)
	{
		GameObject floor = (GameObject)Instantiate(roomFloor, new Vector3(tileSize* (x  + (roomWidth / 2)), tileSize* (y + (roomHeight / 2)),0), Quaternion.identity);
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
						if (roomMatrix [j - x, i - y] == 1) { //wall
							// corner for top halls
							if ((door & 1) == 1 && (j - x == roomWidth / 2 - 2 || j - x == roomWidth / 2 + 2) && i - y == roomWidth - 1) {
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();

								if (j - x == roomWidth / 2 - 2) {
									corner.transform.Rotate(Vector3.forward * 90);
								} else {

								}


							}// corner for bottom halls
							else if ((door & 4) == 4 && (j - x == roomWidth / 2 - 2 || j - x == roomWidth / 2 + 2) && i - y == 0) {
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();
								//rotate left and right walls for appearence
								if (j - x == roomWidth / 2 - 2) {
									corner.transform.Rotate(Vector3.forward * 180);
								} else {
									corner.transform.Rotate(Vector3.forward * -90);
								}
							}// corner for left/east halls
							else if ((door & 2) == 2 && (i - y == roomHeight / 2 - 2 || i - y == roomHeight / 2 + 2) && j - x == roomWidth - 1) {
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();
								//rotate left and right walls for appearence
								if (i - y == roomHeight / 2 - 2) {
									corner.transform.Rotate(Vector3.forward * -90);
								} else {
									
								}
							}// corner for left/east halls
							else if ((door & 8) == 8 && (i - y == roomHeight / 2 - 2 || i - y == roomHeight / 2 + 2) && j - x == 0) {
								GameObject corner = (GameObject)Instantiate(cornerWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								corner.AddComponent<BoxCollider2D>();
								Rigidbody2D body = corner.GetComponent<Rigidbody2D>();
								//rotate left and right walls for appearence
								if (i - y == roomHeight / 2 - 2) {
									corner.transform.Rotate(Vector3.forward * 180);
								} else {
									corner.transform.Rotate(Vector3.forward * 90);
								}
							} else {
								GameObject block = (GameObject)Instantiate(regularWall, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
								block.AddComponent<BoxCollider2D>();
								Rigidbody2D body = block.GetComponent<Rigidbody2D>();
								//rotate left walls for appearence
								if (j - x == 0) {
									block.transform.Rotate(Vector3.forward * -90);

								}//rotate right walls for appearence
								else if (j - x == roomHeight - 1) {
									block.transform.Rotate(Vector3.forward * 90);
								} else if (i - y == roomWidth - 1) {
									block.transform.Rotate(Vector3.forward * 180);
								}
							}

						
						} else if (roomMatrix [j - x, i - y] == 2) { //spawner
							
							GameObject spawnerBlock = (GameObject)Instantiate(spawner, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
							spawnerBlock.AddComponent<BoxCollider2D>();

							if (j - x == 0) {
								spawnerBlock.GetComponent<EnemySpawner>().east = true;
							} else if (j - x == roomWidth - 1) {
								spawnerBlock.GetComponent<EnemySpawner>().west = true;
							} else if (i - y == 0) {
								spawnerBlock.GetComponent<EnemySpawner>().north = true;
							} else if (i - y == roomHeight - 1) {
								spawnerBlock.GetComponent<EnemySpawner>().south = true;
							}

						} else if (roomMatrix [j - x, i - y] == 3) { // breakable box
							GameObject boxBlock = (GameObject)Instantiate(breakableBox, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
						} else if (roomMatrix [j - x, i - y] == 4) {
							GameObject mineObject = (GameObject)Instantiate(mine, new Vector3(j * tileSize, i * tileSize, 0), Quaternion.identity);
						}
					}
				}
			}
		}
	}
	int[,] makeFloorMatrix(int floorWidth, int floorHeight, int numberOfCircles , int numberOfSpecialRooms)

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

		//insert circle center for primary circle
		for (int i = 0; i < 1; i++)
		{

			int x = UnityEngine.Random.Range(0, floorWidth - 1 );
			int y = UnityEngine.Random.Range(0, floorHeight - 1);
			if (returnMatrix[y, x] != 1 && returnMatrix[y, x] != 2 )

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

				if (returnMatrix[j,i] == 2)
				{ 
					
					//create rooms around center
					for (int x = i-1; x < i+2 ; x++)
					{
						for (int y = j-1; y < j+2; y++)
						{
							

							//check to make sure circle-edge room being added is in domain and range of floorMatrix
							if ( (x >= 0 && y >= 0) && (x < floorWidth  && y < floorHeight ))
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
		for (int i = 0; i < numberOfSpecialRooms ; i++)
		{
			//pick a circle-edge to aim towards 
			//defines position as index in listOfCircleEdgeRooms


			int circleEdgeTarget = UnityEngine.Random.Range(0, listOfCircleEdgeRooms.Count/2);
			bool foundPath = false;
			int x = (int)listOfSpecialRooms[2*i + 1];
			int y = (int)listOfSpecialRooms[2*i];

			//start building x towards a circle
			for (; x != (int)listOfCircleEdgeRooms[ circleEdgeTarget*2];)
			{
				
				if ( x < (int)listOfCircleEdgeRooms[ circleEdgeTarget*2 ])

				{
					x++;
				}
				else
				{
					x--;
				}
				//if room hasnt been pathed to closer room 
				if (returnMatrix[y, x] == 0 )
				{
					//adds new normal room into to create path
					returnMatrix[y, x] = 4;
				}else

				{

					foundPath = true;
					break;
				}
			}
						
			if (!foundPath)
			{
				for (; y != (int)listOfCircleEdgeRooms[ circleEdgeTarget*2 + 1] ;)
				{
					if (y < (int)listOfCircleEdgeRooms[ circleEdgeTarget*2 + 1])

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

		//insert circle centers for the rest of circles
		for (int i = 0; i < numberOfCircles - 1; i++)
		{

			int x = UnityEngine.Random.Range(0, floorWidth - 1);
			int y = UnityEngine.Random.Range(0, floorHeight - 1);
			if (returnMatrix[y, x] == 1)
			{
				int targetx = UnityEngine.Random.Range(0, floorWidth - 1);
				int targety = UnityEngine.Random.Range(0, floorHeight - 1);
				//start pathing for each of the special rooms
				//defines position as index in numberOfSpecialRooms
				for (int j = 0; j < numberOfSpecialRooms; j++)
				{
					//pick a circle-edge to aim towards 
					//defines position as index in listOfCircleEdgeRooms


					int circleEdgeTarget = UnityEngine.Random.Range(0, listOfCircleEdgeRooms.Count / 2);
					bool foundPath = false;

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
							for (int tempx = i - 1; tempx < i + 2; tempx++)
							{
								for (int tempy = j - 1; tempy < j + 2; tempy++)
								{
									//check to make sure circle-edge room being added is in domain and range of floorMatrix
									if ((tempx >= 0 && tempy >= 0) && (tempx < floorWidth && tempy < floorHeight))
									{

										//check if room is already made there
										if (returnMatrix[tempy, tempx] == 0)
										{
											returnMatrix[tempy, tempx] = 3;

										}
									}
								}
							}
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

								//create rooms around center
								for (int tempx = i - 1; tempx < i + 2; tempx++)
								{
									for (int tempy = j - 1; tempy < j + 2; tempy++)
									{
										//check to make sure circle-edge room being added is in domain and range of floorMatrix
										if ((tempx >= 0 && tempy >= 0) && (tempx < floorWidth && tempy < floorHeight))
										{

											//check if room is already made there
											if (returnMatrix[tempy, tempx] == 0)
											{
												returnMatrix[tempy, tempx] = 3;

											}
										}
									}
								}
							}
						}
					}
				}


			}
			else //if the matrix indices were already filled , retry
			{
				x = UnityEngine.Random.Range(0, floorWidth - 1);
				y = UnityEngine.Random.Range(0, floorHeight - 1);
				i--;
			}

		}
		

		return returnMatrix;
	}

}
