using UnityEngine;
using System.Collections;

public class PositionGizmo : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.DrawSphere(transform.position, 2.0f);	
	}
}
