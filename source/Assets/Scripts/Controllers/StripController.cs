using UnityEngine;
using System.Collections;


public class StripController : Singleton<StripController> {
	private Strip[] _strips;
	public GameObject StripPrefab;
	private CameraController _cameraController;
	private PersonageController _personageController;
	private bool _shaking = false;
	private float _shakeTime = 0.0f;
	private Vector3 _stripSize;
	private float punchDirection;
	private float previousPosition;
	
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
		_stripSize = StripSize(renderRect, orthographicSize);


		strip.RenderCam(renderRect, cameraPosition, _stripSize);
		
		// TO-DO set appart
		strip.Personage = _personageController.RandomPersonage();

		return strip;
	}
	
	public void Update() {
		if (_shaking && Time.time - _shakeTime > 1.0f) {
			ShakeEnd();
		} 
	}
	
	public void Shake() {
		if (_shaking == false) {
			ShakeStart();	
		}
		ShakeUpdate();
	}
	
	public Strip GameObjectToStrip(GameObject gameObject) {
		if (gameObject.tag == StripPrefab.tag) {
			return gameObject.GetComponent<Strip>();
		}
		
		return null;
	}
	
	public Strip ReactableToStrip(GameObject gameObject) {
		foreach (Strip strip in _strips) {
			if (strip.Contains(gameObject)) {
				return strip;	
			}
		}
		return null;
	}
	
	void ShakeStart() {
		_shaking = true;
		InputInfo inputInfo = new InputInfo();
		for (int i = 0; i < _strips.Length; ++i) {
			_strips[i].OnPressDown(inputInfo);
		}
	}
	
	void ShakeUpdate() {
		_shakeTime = Time.time;
		InputInfo inputInfo = new InputInfo();
		for (int i = 0; i < _strips.Length; ++i) {
			inputInfo.worldMove = Vector3.right*(Random.Range(-1, 2)*_stripSize.x);
			_strips[i].OnMove(inputInfo);
		}
	}
	
	void ShakeEnd() {
		_shaking = false;
		InputInfo inputInfo = new InputInfo();
		
		for (int i = 0; i < _strips.Length; ++i) {
			_strips[i].OnPressUp(inputInfo);
		}
	}
	
	public void PunchCameras(float force) {
		float sign = Mathf.Sign(force);
		float absForce = sign*force;
		absForce *= absForce;
		
		Vector3 amount = new Vector3(Mathf.Lerp(0.0f, 2.0f, absForce/0.25f)*sign, 0.0f, 0.0f);
		
		previousPosition = transform.position.x;
		punchDirection = amount.x;
		
		Hashtable parameters = new Hashtable();
		parameters["time"] = 0.5f;
		parameters["position"] = transform.position;
		parameters["easetype"] = iTween.EaseType.easeOutBounce;
		parameters["amount"] = amount;
		parameters["onupdate"] = "OnShakeUpdate";
		parameters["onupdatetarget"] = gameObject;
		iTween.PunchPosition(gameObject, parameters);		
	}
	
	void OnShakeUpdate() {
		float currentDirection = transform.position.x - previousPosition;
		
		if (currentDirection*punchDirection < 0) {
			foreach(Strip strip in _strips) {
				strip.ShakeUpdateCameras(currentDirection);	
			}
		}
		
		punchDirection = currentDirection;
		previousPosition = transform.position.x;
	}
	
	void AssignPersonageToStrip(Personage personage, int stripId) {
		_strips[stripId].Personage = personage;
	}

	Rect StripRenderRect(int id) {
		return new Rect(0, StripHeight*id, 1, StripHeight);
	}

	Vector3 StripSize(Rect rect, float orthographicSize) {
		return new Vector3(orthographicSize*(2*Screen.width*rect.width/(Screen.height*rect.height)), orthographicSize*2, _cameraController.Camera.nearClipPlane*0.2f);
	}

	Vector3 StripCameraPosition(float y) {
		Rect worldRect = _cameraController.WorldRect;
		return new Vector3(worldRect.center.x, worldRect.y + worldRect.height*(y + StripHeight*0.5f), _cameraController.Camera.nearClipPlane*10);
	}
	
	
	
	
}
