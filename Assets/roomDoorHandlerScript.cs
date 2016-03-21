using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class roomDoorHandlerScript : MonoBehaviour {

	public bool paidScrap;
	List<GameObject> innerDoors;
	List<GameObject> outerDoors;
	public GameObject upgrade;
	float time;
	float transitionTime;
	int scrapCost;
	float rePath;

	// Use this for initialization
	void Start () {
		paidScrap = false;
		innerDoors = new List<GameObject>();
		outerDoors = new List<GameObject>();
		time = 0;
<<<<<<< c047f5b1a2108231883fd556a4921c31898f7ca9:Assets/Logic/roomDoorHandlerScript.cs
		transitionTime = .5f;
		rePath = 10f;
		scrapCost = 10;
		open = false;

		playerCheck = 0;
		playerTime = .2f;

=======
		transitionTime = 3f;
		scrapCost = 10;
>>>>>>> Fix?:Assets/roomDoorHandlerScript.cs

		findNearDoors();

	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< c047f5b1a2108231883fd556a4921c31898f7ca9:Assets/Logic/roomDoorHandlerScript.cs

		if (rePath != 10f )
			rePath -= Time.deltaTime;

		if (rePath <= 0f) {
			rePath = 10f;
			Pathfinder2D.Instance.gen = 0;
		}

		if (!paidScrap && playerCheck >= playerTime)
=======
		if (!paidScrap)
>>>>>>> Fix?:Assets/roomDoorHandlerScript.cs
		{
			playerDetect();
			playerCheck = 0; 
		}
		if (paidScrap == true)
		{
			if (time < transitionTime)
			{
				// open inner doors
				foreach (GameObject gameOBJ in innerDoors)
				{
					if (gameOBJ.GetComponent<Door>().state == 0)
					{
						gameOBJ.GetComponent<Door>().state = 1;
					}
				}
			}
			else
			{	// open outer doors
				foreach (GameObject gameOBJ in outerDoors)
				{
					if (gameOBJ.GetComponent<Door>().state == 0)
					{
						gameOBJ.GetComponent<Door>().state = 1;
<<<<<<< c047f5b1a2108231883fd556a4921c31898f7ca9:Assets/Logic/roomDoorHandlerScript.cs
						open = true;

						rePath = 1.0f;
=======
>>>>>>> Fix?:Assets/roomDoorHandlerScript.cs
					}
				}

			}
			time += Time.deltaTime;

		}

	}

	private void playerDetect()
	{
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);
		
		foreach (Collider2D col in hitColliders)
		{
			if (col.gameObject.name.Equals("Player"))
			{
				if (time > transitionTime)
				{
					if (col.gameObject.GetComponent<Player>().stats.get_scrap() >= scrapCost)
					{
						col.gameObject.GetComponent<Player>().stats.change_scrap(-scrapCost);
						paidScrap = true;
						time = 0;
						transitionTime = .5f;
					}

				}
				else
				{
					time += Time.deltaTime;
				}
			}
<<<<<<< c047f5b1a2108231883fd556a4921c31898f7ca9:Assets/Logic/roomDoorHandlerScript.cs
			//findNearDoors();
=======
>>>>>>> Fix?:Assets/roomDoorHandlerScript.cs
		}
	}

	private void findNearDoors()
	{
		Collider2D[]hitColliders = Physics2D.OverlapCircleAll(this.transform.position,10);
		
		foreach (Collider2D col in hitColliders)
		{
			if (col.gameObject.name.Equals("DoorPiece1(Clone)"))
			{
				innerDoors.Add(col.gameObject);
			}else if (col.gameObject.name.Equals("DoorPiece2(Clone)") )
			{
				outerDoors.Add(col.gameObject);
			}
			
		}
		
	}
}
