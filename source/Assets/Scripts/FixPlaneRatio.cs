using UnityEngine;
using System.Collections;

public class FixPlaneRatio : MonoBehaviour {
	// With the new packed textures, the shader causes weird artifacts in the borders
	// most likely due to floating point errors when the texture size is not >= of the
	// screen size
	private float _shaderCorrection = 1.01f;
	
	// Unity Planes have a 10 to 1 scale relationship
	private float _unityPlaneUnitsToWorldUnits = 0.1f;

	void Awake () {
		Vector3 scale = transform.localScale;

		float correctionFactor = _shaderCorrection*_unityPlaneUnitsToWorldUnits;

		Rect worldRect = CameraController.instance.WorldRect;
		scale.x = worldRect.width*correctionFactor;
		scale.z = worldRect.height*correctionFactor;
		transform.localScale = scale;
	}
}
