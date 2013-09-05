using UnityEngine;
using System.Collections;

public class FixPlaneRatio : MonoBehaviour {
	void Awake () {
		Vector3 scale = transform.localScale;
		Texture texture = renderer.material.mainTexture;
		
		// Planes have a 10 to 1 scale relationship
		float width = CameraController.instance.WorldRect.width/10.0f;
		float ratio = (float)texture.width/texture.height;
		
		scale.x = width;
		scale.z = scale.x/ratio;
		transform.localScale = scale;
	}
}
