using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public bool open;
	public byte hinge;
	public bool delay;
	float time;
	float actionTime;
	int count;
	public int max;


	// Use this for initialization
	void Start()
	{
		open = true;
		time = 0;
		actionTime = .01f;
		count = 0;
		max = 300;
	}

	// Update is called once per frame
	void Update()
	{
		time += Time.deltaTime;
		if (time > actionTime)
		{
			time = 0;

			if (delay)
			{
				if (count > max)
				{
					delay = false;
					count = 0;
				}
				else if (!open) 
				{
					count++;
				}
				
			}else if (count > max)
			{
				open = true;
			}
			else if (open == false)
			{
				count++;
				openDoor();
			}
			
		}
		

		
	}
	void openDoor()
	{
		if (open == false)
		{
			
			if ((hinge & 1) == 1)
			{
				Vector3 position = this.transform.position;
				position.y+=.01f;
				this.transform.position = position;
			}
			else if ((hinge & 2) == 2)
			{
				Vector3 position = this.transform.position;
				position.x += .01f;
				this.transform.position = position;
			}
			else if ((hinge & 4) == 4)
			{
				Vector3 position = this.transform.position;
				position.y -= .01f;
				this.transform.position = position;
			}
			else if ((hinge & 8) == 8)
			{
				Vector3 position = this.transform.position;
				position.x -= .01f;
				this.transform.position = position;
			}
		}


	}


}

