using UnityEngine;
using System.Collections;
using Radical;

public class StripController : Singleton<StripController> {
	private Strip[] _strips;
	public GameObject StripPrefab;
	public bool Shaking  {
		get {
			return _shaking;	
		}
	}
	
	public AudioClip spinClip;
	private CameraController _cameraController;
	private PersonageController _personageController;
	private bool _shaking = false;
	private Vector3 _stripSize;
	private float punchDirection;
	private float previousPosition;
	private SmoothVector3[] _smoothInputShake;
	private InputInfo[] _inputInfoShake;
	private SoundController _soundController;
	
	void Awake () {
		_cameraController = CameraController.instance;
		_personageController = PersonageController.instance;
		_soundController = SoundController.instance;
	}
	
	public int StripNumber {
		get {
			return _strips.Length;
		}
		set {
			int num = value;
			_strips = new Strip[num];
			_smoothInputShake = new SmoothVector3[num];
			_inputInfoShake = new InputInfo[num];
			StripHeight = 1.0f/num;
			Personage initialPersonage = _personageController.RandomPersonage();

			for (int i = 0; i < num; ++i) {
				_strips[i] = CreateStrip(i);
				_strips[i].Personage = initialPersonage;
				_smoothInputShake[i] = Vector3.zero;
				_smoothInputShake[i].Duration = 1.0f;
				_inputInfoShake[i] = new InputInfo();
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

		return strip;
	}
	
	public void Update() {
		if (_shaking) {
			Shake();
			if (!_shaking) {
				ShakeEnd();
			}
		}
	}
	
	public void IdleRotate() {
		InputInfo input = new InputInfo();
		
		foreach(Strip strip in _strips) {
			input.worldMove = Vector3.right*Time.deltaTime*10;
			strip.OnMove(input);
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
		float maxDuration = 0;
		for (int i = 0; i < _strips.Length; ++i) {
			float targetSign = 1;//(Random.Range (0.0f, 1.0f) > 0.5f ? -1: 1);
			Vector3 target = Vector3.right*(Random.Range(3, 5)*_stripSize.x);
			float duration = Mathf.Lerp (1.0f, 3.0f, target.x/(49*_stripSize.x));
			
			_smoothInputShake[i].Value = target*targetSign;
			_smoothInputShake[i].Duration = duration;
			maxDuration = Mathf.Max(duration, maxDuration);
			
			InputInfo inputInfo = _inputInfoShake[i];
			inputInfo.worldMove = Vector3.zero;
			inputInfo.worldPosition = Vector3.zero;
			inputInfo.screenMove = Vector3.zero;
			inputInfo.screenPosition = Vector3.zero;
			
			_strips[i].OnPressDown(inputInfo);
		}
		
		_soundController.PlayFaded(spinClip, maxDuration);
	}
	
	void ShakeUpdate() {
		_shaking = false;
		for (int i = 0; i < _strips.Length; ++i) {
			Vector3 target = _smoothInputShake[i];
			
			if (Vector3.Distance(_inputInfoShake[i].worldPosition, target) > 0 || !_smoothInputShake[i].IsComplete) {
				_inputInfoShake[i].worldMove = _inputInfoShake[i].worldPosition - target;
				_inputInfoShake[i].worldPosition = target;
				
				_strips[i].OnMove(_inputInfoShake[i]);
				_shaking = _shaking || true;
			}
		}
	}
	
	void ShakeEnd() {
		_shaking = false;
		for (int i = 0; i < _strips.Length; ++i) {
			InputInfo inputInfo = _inputInfoShake[i];
			
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
