using UnityEngine;
using System.Collections;

public class TintCleaner : MonoBehaviour {
	
	void Update () {
		Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward*transform.localScale.z;
		
		if (Input.GetMouseButtonDown(0)) {
			renderer.material.color = Color.red;
		}
		
		if (Input.GetMouseButtonUp(0)) {
			renderer.material.color = Color.blue;
		}
		
		transform.position = position;
	}
}
