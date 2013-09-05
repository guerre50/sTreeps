using UnityEngine;
using System.Collections;

public class StripController : Singleton<StripController> {
	private Strip[] _strips;
	public GameObject StripPrefab;
	private CameraController _cameraController;
	private PersonageController _personageController;
	
	void Awake () {
		_cameraController = CameraController.instance;
		_personageController = PersonageController.instance;
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
		stripInstance.transform.parent = transform;	

		Strip strip = stripInstance.GetComponent<Strip>();
		strip.Id = id;

		// We set Camera parameters
		Rect renderRect = StripRenderRect(id);
		Vector3 cameraPosition = StripCameraPosition(renderRect.y);
		float orthographicSize = (float)_cameraController.WorldRect.height/StripNumber/2.0f;
		Vector3 size = StripSize(renderRect, orthographicSize);


		strip.RenderCam(renderRect, cameraPosition, size);
		
		// TO-DO set appart
		strip.Personage = _personageController.RandomPersonage();

		return strip;
	}
	

	void AssignPersonageToStrip(Personage personage, int stripId) {
		_strips[stripId].Personage = personage;
	}
	
	

	Rect StripRenderRect(int id) {
		return new Rect(0, StripHeight*id, 1, StripHeight);
	}

	Vector3 StripSize(Rect rect, float orthographicSize) {
		return new Vector3(orthographicSize*(2*Screen.width*rect.width/(Screen.height*rect.height)), orthographicSize*2, _cameraController.camera.nearClipPlane*0.2f);
	}

	Vector3 StripCameraPosition(float y) {
		Rect worldRect = _cameraController.WorldRect;
		return new Vector3(worldRect.center.x, worldRect.y + worldRect.height*(y + StripHeight*0.5f), _cameraController.camera.nearClipPlane*10);
	}

	
	
}
