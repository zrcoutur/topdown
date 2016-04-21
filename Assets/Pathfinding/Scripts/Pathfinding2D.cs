using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pathfinding2D : MonoBehaviour
{
	public List<Vector3> Path = new List<Vector3>();
    public bool JS = false;

    public void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Pathfinder2D.Instance.InsertInQueue(startPosition, endPosition, SetList);
    }

    public void FindJSPath(Vector3[] arr)
    {
        if (arr.Length > 1)
        {
            Pathfinder2D.Instance.InsertInQueue(arr[0], arr[1], SetList);
        }
    }

    //A test move function, can easily be replaced
	public void Move ()
	{
		if (Path.Count > 0) {
			// Unit direction vector to point
			Vector2 dir = (Path [0] - transform.position).normalized;

			// Get rigidbody
			var body = GetComponent<Rigidbody2D>();

			// Add a force towards next point in the path.
			body.AddForce (dir * GetComponent<Baseenemy> ().speed);

			// Calculate angle to target
			float currentAngle = Tools.QuaternionToAngle (body.transform.rotation);
			float targetAngle = Tools.Vector2ToAngle (dir) + 90.0f;

			GetComponent<Baseenemy> ().transform.rotation = Tools.AngleToQuaternion (Mathf.MoveTowardsAngle (currentAngle, targetAngle, 3.0f * GetComponent<Baseenemy> ().speed));


			// Continue to next path segment
			if (Vector3.Distance (transform.position, Path [0]) < 1.2f) {
				Path.RemoveAt (0);
			}
			// Careful traversal
			else if (Vector3.Magnitude (body.velocity) > 3.0f) {
				body.velocity = Vector3.Normalize(body.velocity) * 3.0f;
			}

			// Skip one segment
			if (Path.Count > 1) {
				if (Vector3.Distance (transform.position, Path [1]) < 1.2f) {
					Path.RemoveAt (0);
					Path.RemoveAt (0);
				}
			}
        }
    }

    protected virtual void SetList(List<Vector3> path)
    {
        if (path == null)
        {
            return;
        }

        if (!JS)
        {
            Path.Clear();
            Path = path;
            //Path[0] = new Vector3(path[0].x, path[0].y, path[0].z);
            //Path[Path.Count - 1] = new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y, Path[Path.Count - 1].z);
        }
        else
        {           
            Vector3[] arr = new Vector3[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                arr[i] = path[i];
            }

            arr[0] = new Vector3(arr[0].x, arr[0].y , arr[0].z);
            arr[arr.Length - 1] = new Vector3(arr[arr.Length - 1].x, arr[arr.Length - 1].y, arr[arr.Length - 1].z);
            gameObject.SendMessage("GetJSPath", arr);
        }
    }
}
