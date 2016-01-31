using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {

	/*******************************************************************************
	 * 
	 * Finds the nearest object from position with a given tag. Returns that object's ID.
	 * Returns null if no such tagged object exists.
	 * 
	 *******************************************************************************/

	public static Transform findNearest( Vector3 from, string tag ) {

		var nearest = (Transform) null;
		var nearestDistanceSqr = Mathf.Infinity;

		// Find all objects with tag
		var tags = GameObject.FindGameObjectsWithTag ("Player");

		// Iterate through all found object
		foreach (GameObject obj in tags) {

			// Calculate distance to target
			var to = obj.transform.position;
			var distanceSqr = (to - from).sqrMagnitude;

			// Check if distance is less than current minimum
			if (distanceSqr < nearestDistanceSqr) {

				// If so, set this object to the new minimum
				nearest = obj.transform;
				nearestDistanceSqr = distanceSqr;
			}

		}

		// Return the nearest object found.
		return nearest;

	}

	/*******************************************************************************
	 * 
	 * Converts an angle to a Vector2 with a given magnitude.
	 * 
	 *******************************************************************************/
	public static Vector2 AngleToVec2( float angle, float magn ) {

		return new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad ) * magn, Mathf.Sin (angle * Mathf.Deg2Rad ) * magn);

	}

	/*******************************************************************************
	 * 
	 * Converts a quaternion to an angle representation (by multiplying by
	 * Vector3.forward)
	 * 
	 *******************************************************************************/
	public static float QuaternionToAngle( Quaternion q ) {

		return Vector2ToAngle (q * Vector3.right);
	}


	/*******************************************************************************
	 * 
	 * Converts a Vector2 to an angle representation.
	 * 
	 *******************************************************************************/
	public static float Vector2ToAngle( Vector2 v ) {

		return Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;

	}

	/*******************************************************************************
	 * 
	 * Convets an angle to a quaternion.
	 * 
	 *******************************************************************************/
	public static Quaternion AngleToQuaternion( float f ) {

		return Quaternion.AngleAxis (f, Vector3.forward);

	}
}
