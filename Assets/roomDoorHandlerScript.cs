using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class roomDoorHandlerScript : MonoBehaviour {

	public bool paidScrap;
	List<GameObject> innerDoors;
	List<GameObject> outerDoors;
	float time;
	float transitionTime;
	int scrapCost;

	// Use this for initialization
	void Start () {
		paidScrap = false;
		innerDoors = new List<GameObject>();
		outerDoors = new List<GameObject>();
		time = 0;
		transitionTime = 3f;
		scrapCost = 10;

		findNearDoors();

	}
	
	// Update is called once per frame
	void Update () {
		if (!paidScrap)
		{
			playerDetect();
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
