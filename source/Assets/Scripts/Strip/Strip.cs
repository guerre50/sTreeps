using UnityEngine;
using System.Collections;

public class Strip : MonoBehaviour {
	public int Id;

	private StripCamera _stripCamera;
	
	void Awake () {
		_stripCamera = gameObject.GetComponentInChildren<StripCamera>();
	}

	public void RenderCam(Rect rect, Vector3 position, float orthographicSize) {
		_stripCamera.camera.rect = rect;
		_stripCamera.camera.orthographicSize = orthographicSize;

		_stripCamera.transform.position = position;
	}
}
