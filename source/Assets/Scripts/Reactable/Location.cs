using UnityEngine;
using System.Collections;

public class Location : Reactable {
	public GameObject location;
	
	public Vector3 position {
		get {
			return location.transform.position;
		}
	}
}
