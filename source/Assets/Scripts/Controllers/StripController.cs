using UnityEngine;
using System.Collections;

public class StripController : Singleton<StripController> {
	private Strip[] _strips;
	public GameObject StripPrefab;
	private CameraController _cameraController;

	void Awake () {
		_cameraController = CameraController.instance;

	}
	
	public int StripNumber {
		get {
			return _strips.Length;
		}
		set {
			int num = value;
			_strips = new Strip[num];
			StripHeight = 1.0f/num;

			for (int i = 0; i < num; ++i) {
				_strips[i] = CreateStrip(i);
			}

		}
	}

	public float StripHeight {
		get; private set;
	}

	Strip CreateStrip(int id) {
		GameObject stripInstance = Instantiate(StripPrefab, Vector3.zero, Quaternion.identity) as GameObject;	

		Strip strip = stripInstance.GetComponent<Strip>();
		strip.Id = id;

		// We set Camera parameters
		Rect renderRect = StripRenderRect(id);
		Vector3 cameraPosition = StripCameraPosition(renderRect.y);
		float ortographicSize = _cameraController.WorldRect.height/StripNumber/2;

		strip.RenderCam(renderRect, cameraPosition, ortographicSize);

		return strip;
	}

	Rect StripRenderRect(int id) {
		return new Rect(0, StripHeight*id, 1, StripHeight);
	}

	Vector3 StripCameraPosition(float y) {
		Rect worldRect = _cameraController.WorldRect;
		return new Vector3(worldRect.center.x, worldRect.y + worldRect.height*(y + StripHeight*0.5f), 0);
	}

	
	
}
