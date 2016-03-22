using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public int state;   // 0 = closed, 1 = opening, 2 = opened, 3 = closing
	public byte hinge;
	public bool delay;

	private float initalPos;
	private float time;
	private float transitionTime;
	private bool once = true;

	// Use this for initialization
	void Start()
	{
		
		time = 0;
		transitionTime = 1;

	}

	// Update is called once per frame
	void Update()
	{
		if (hinge != 0 && once)
		{
			once = false;
			if ((hinge & 1) == 1)
			{
				initalPos = this.transform.position.y;

			}
			else if ((hinge & 2) == 2)
			{
				initalPos = this.transform.position.x;

			}
			else if ((hinge & 4) == 4)
			{
				initalPos = this.transform.position.y;

			}
			else if ((hinge & 8) == 8)
			{
				initalPos = this.transform.position.x;
			}
		}

			if (state == 0)
			{
				//door is closed do nothing
			}else if (state == 1)
			{
				time += Time.deltaTime;
				openDoor();
				
			}else if (state == 2)
			{
				//door is open do nothing
			}else if (state == 3)
			{
			time += Time.deltaTime;
			closeDoor();
				
			}
			
		

	}
	void openDoor()
	{

		float posChange = 3f;//factor of position change per function call
		if ((hinge & 1) == 1)
		{
			Vector3 position = this.transform.position;
			position.y = initalPos + posChange * (time / transitionTime);
			this.transform.position = position;
			if ((time / transitionTime) >= 1)
			{
				state = 2;
			}
		}
		else if ((hinge & 2) == 2)
		{
			Vector3 position = this.transform.position;
			position.x = initalPos + posChange * (time / transitionTime);
			this.transform.position = position;
			if ((time / transitionTime) >= 1)
			{
				state = 2;
			}
		}
		else if ((hinge & 4) == 4)
		{
			Vector3 position = this.transform.position;
			position.y = initalPos - posChange * (time / transitionTime);
			this.transform.position = position;
			if ((time / transitionTime) >= 1)
			{
				state = 2;
			}
		}
		else if ((hinge & 8) == 8)
		{
			Vector3 position = this.transform.position;
			position.x = initalPos - posChange * (time / transitionTime);
			this.transform.position = position;
			if ((time / transitionTime) >= 1)
			{
				state = 2;
			}
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

