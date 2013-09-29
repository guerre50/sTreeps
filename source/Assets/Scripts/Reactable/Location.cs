using UnityEngine;
using System.Collections;

public class Location : MonoBehaviour {
	public GameObject location;
	
	public Vector3 position {
		get {
			return location.transform.position;
		}
	}
}
