using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public int state;   // 0 = closed, 1 = opening, 2 = opened, 3 = closing
	public byte hinge;
	public bool delay;
	float time;
	float transitionTime;


	// Use this for initialization
	void Start()
	{
		time = 0;
		transitionTime = 1;
	}

	// Update is called once per frame
	void Update()
	{
		
		if (time <= transitionTime)
		{
			if (state == 0)
			{
				//door is closed do nothing
			}else if (state == 1)
			{
				openDoor();
				time += Time.deltaTime;
			}else if (state == 2)
			{
				//door is open do nothing
			}else if (state == 3)
			{
				closeDoor();
				time += Time.deltaTime;
			}
			
		}//after trasition states have completed, put them in open or closed state. reset time
		else
		{
			if (state == 1) 
			{
				state = 2;
			}
			if (state == 3)
			{
				state = 0;
			}
			time = 0;
		}
		

	}
	void openDoor()
	{

		float posChange = .05f; //factor of position change per function call
		if ((hinge & 1) == 1)
		{
			Vector3 position = this.transform.position;
			position.y += posChange;
			this.transform.position = position;
		}
		else if ((hinge & 2) == 2)
		{
			Vector3 position = this.transform.position;
			position.x += posChange;
			this.transform.position = position;
		}
		else if ((hinge & 4) == 4)
		{
			Vector3 position = this.transform.position;
			position.y -= posChange;
			this.transform.position = position;
		}
		else if ((hinge & 8) == 8)
		{
			Vector3 position = this.transform.position;
			position.x -= posChange;
			this.transform.position = position;
		}


	}
	void closeDoor()
	{
		float posChange = .05f;//factor of position change per function call
		if ((hinge & 1) == 1)
		{
			Vector3 position = this.transform.position;
			position.y -= posChange;
			this.transform.position = position;
		}
		else if ((hinge & 2) == 2)
		{
			Vector3 position = this.transform.position;
			position.x -= posChange;
			this.transform.position = position;
		}
		else if ((hinge & 4) == 4)
		{
			Vector3 position = this.transform.position;
			position.y += posChange;
			this.transform.position = position;
		}
		else if ((hinge & 8) == 8)
		{
			Vector3 position = this.transform.position;
			position.x += posChange;
			this.transform.position = position;
		}

	}

}

