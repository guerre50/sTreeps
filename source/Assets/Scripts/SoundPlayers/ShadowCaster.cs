using UnityEngine;
using System.Collections;

public class ShadowCaster : Reactable {
	public GameObject referenceShadow;
	public GameObject shadow;
	private Vector3 referencePosition;
	
	void Start () {
		referencePosition = referenceShadow.transform.position;
	}
	
	void LateUpdate () {
		
		Vector3 pos = referencePosition;
		Vector3 realPos = strip.LocalToGlobalPosition(transform.position, Layer);
		pos.x = realPos.x;
		shadow.transform.position = pos;
	}
}
