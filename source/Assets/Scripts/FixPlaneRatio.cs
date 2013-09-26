using UnityEngine;
using System.Collections;

public class FixPlaneRatio : MonoBehaviour {
	void Awake () {
		Vector3 scale = transform.localScale;
		Texture texture = renderer.material.mainTexture;
		
		// Unity Planes have a 10 to 1 scale relationship
		Rect worldRect = CameraController.instance.WorldRect;
		scale.x = worldRect.width/10.0f;
		scale.z = worldRect.height/10.0f;
		transform.localScale = scale;
	}
}
