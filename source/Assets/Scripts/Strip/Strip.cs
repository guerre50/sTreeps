using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Radical;
using UnityEditor;

public class Strip : MonoBehaviour {
	public int Id;

	private StripCamera _stripCamera;
	private InputController _input;
	private float _percentage {
		get {
			return _smoothPercentage.Target;	
		}
		set {
			_smoothPercentage.Value = value;	
		}
	}
	private SmoothFloat _smoothPercentage;
	private Vector3 _size;
	private int _layer;
	private Personage _personage;
	private Personage _perviousPersonage;
	private Dictionary<int, Dictionary<int, GameObject>> _reactables = new Dictionary<int, Dictionary<int, GameObject> >();
	private Dictionary<int, int> _reactableToLayer = new Dictionary<int, int>();
	private float _pressTimeMargin = 0.1f;
	private InputInfo _pressInput;
	private Promise _pendentInput;
	private float _personageChangeTime = 0.0f;
	private Vector3 _target;
	private bool _freeMove = false;
	private StripController _stripController;
	private float _previousShake;
	private float _previousSign;
	private Callback _firstShakeCallback;
	private float _originalX;
	private SoundController _sound;
	
	
	public int Layer {
		get {
			return _stripCamera.Layer;		
		}
		set {
			_stripCamera.Layer = value;
		}
	}
	
	
	public Personage Personage {
		get {
			return _personage;	
		}
		set {
			if (_personage) {
				Deselect();
			}
			_personage = value;
			_stripCamera.Layer = _personage.Layer;
			Select();
		}
	}
	
	public Dictionary<int,GameObject>.ValueCollection StripReactables {
		get {
			Dictionary<int, GameObject> reactables;
			
			if (_reactables.TryGetValue(Layer, out reactables)) {
				return reactables.Values;
			}
			
			return new Dictionary<int,GameObject>().Values;
		} 
	}
	 
	
	void Awake () {
		_smoothPercentage = 0.0f;
		_smoothPercentage.Duration = 0.0f;
		
		_stripCamera = gameObject.GetComponentInChildren<StripCamera>();
		_input = InputController.instance;
	}
	
	void Start() {
		Select();
		_stripController = StripController.instance;
		_target = _stripCamera.transform.position;
		_sound = SoundController.instance;
	}
	
	void Update() {
		 if (_freeMove) {
			iTween.MoveUpdate(_stripCamera.gameObject, _target, 0.0f);	
		}
	}
	
	void Deselect() {
		SendMessageToReactables("Deselect");
	}
	
	void Select() {
		SendMessageToReactables("Select");
	}
	
	void SendMessageToReactables(string message) {
		foreach (GameObject reactable in StripReactables) {
			reactable.SendMessage(message, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		Reactable reactable;
		
		if (TryGetReactable(collider.gameObject, out reactable)) {
			AddReactable(reactable);
		}
	}
	
	void OnTriggerExit(Collider collider) {
		Reactable reactable;
		
		if (TryGetReactable(collider.gameObject, out reactable)) {
			RemoveReactable(reactable);
		}
	}
	
	
	void AddReactable(Reactable reactable) {
		int layer = reactable.Layer;
		Dictionary<int, GameObject> reactablesInLayer;
		
		if (!_reactables.TryGetValue(layer, out reactablesInLayer)) {
			reactablesInLayer = new Dictionary<int, GameObject>();
			_reactables[layer] = reactablesInLayer;
		}
		
		int id = reactable.gameObject.GetInstanceID();
		if (!reactablesInLayer.ContainsKey(id)) {
			reactablesInLayer.Add (id, reactable.gameObject);
			_reactableToLayer.Add (id, layer);
		}
	}
	
	void RemoveReactable(Reactable reactable) {
		int id = reactable.gameObject.GetInstanceID();
		int layer = 0;
		
		if (_reactableToLayer.TryGetValue(id, out layer)) {
			Dictionary<int, GameObject> reactablesInLayer;
			if (_reactables.TryGetValue(layer, out reactablesInLayer)) {
				reactablesInLayer.Remove(id);
				_reactableToLayer.Remove (id);
			}
		}
	}
	
	bool TryGetReactable(GameObject gameObject, out Reactable reactable) {
		reactable = null;
		
		// This is a way to discover objects that should be reactable but doesn't have the react script assigned
		if (gameObject.tag == "Reactable") {
			reactable = gameObject.GetComponent<Reactable>();
			if (reactable == null) {
				Debug.Log(gameObject.name + " should have reactable script attached!!!");	
			}
		}
		
		return reactable != null;
	}

	public void RenderCam(Rect rect, Vector3 position, Vector3 size) {
		_size = size;
		_stripCamera.SetViewport(rect, size.y/2, size);

		transform.position = position;
		BoxCollider box = GetComponent<BoxCollider>();
		
		// Rect haves camera info but we have to change z scale to make strip collider
		// collide with Reactable objects
		box.size = size + Vector3.forward*100;
		Vector3 center = box.center;
		center.z = box.size.z/2;
		box.center = center;	
	}
	
	public float Percentage (int layer) {
		float extra = 0;
		if (Personage.Left.Layer == layer) {
			extra = 1;
		} else if (Personage.Right.Layer == layer) {
			extra = -1;
		}
		return _smoothPercentage.Target + extra;
	}
	
	void ResolvePress() {
		_input.HitLayerTagSendMessage(_pressInput, Layer, "Reactable", "OnPressDown");
	}

	public void OnPressDown(InputInfo input) {
		_pressInput = input;
		
		if (_pendentInput != null) {
			_pendentInput.Cancel();	
		}
		_pendentInput = _.Wait(_pressTimeMargin).Done(ResolvePress);
	}
	
	public float StripPitch(float force = 0) {
		return Mathf.Lerp (0.8f, 1.0f, force)*Random.Range (0.95f,1.05f);
	}
	
	public void OnPressUp(InputInfo input) {
		if (Time.time - _personageChangeTime > 1.0f) {
			float move = input.worldMove.x/_size.x;
			
			if (Mathf.Abs(move) > 0.05f) {	
				// If we change character
				float sign = Mathf.Sign(_percentage + move);
				ChangePersonage(sign);
				_personageChangeTime = 0.0f;
			}
		}
		PlayPressUp();
		Shake (0.5f, _percentage);
		
	}
	
	public void Shake(float duration, float force) {
		Hashtable parameters = new Hashtable();
		parameters["time"] = duration;
		parameters["position"] = transform.position;
		parameters["easetype"] = iTween.EaseType.easeOutBounce;
		float sign = Mathf.Sign(force);
		parameters["onupdate"] = "OnShakeUpdate";
		parameters["onupdatetarget"] = gameObject;
		parameters["oncomplete"] = "OnShakeComplete";
		parameters["oncompletetarget"] = gameObject;

		_previousSign = sign;
		_originalX = transform.position.x;
		_previousShake = _stripCamera.transform.position.x - transform.position.x;
		_firstShakeCallback = () => { OnShake(duration, force); };
		iTween.MoveTo(_stripCamera.gameObject, parameters);
		
		//_target = transform.position;
		_freeMove = false;
	}
	
	public void OnShakeUpdate() {
		float currentX =  _stripCamera.transform.localPosition.x;
		float currentDelta = currentX - _previousShake;
		SetSideCameras(-(currentX -_originalX));
		
		
		if (_previousSign * currentDelta < 0 && _previousSign > 0) {
			if (_firstShakeCallback != null) {
				_firstShakeCallback();	
			}
		}
		AddToPercentage(-currentDelta);
		
		_previousSign = currentDelta;
		_previousShake = currentX;
	}
	
	public void OnShakeComplete() {
		_percentage = 0.0f;
		Vector3 pos = _stripCamera.transform.localPosition;
		pos.x = 0;
		_stripCamera.transform.localPosition = pos;
		_target = transform.position;
	}
	
	public void OnShake(float duration, float force) {
		_stripController.PunchCameras(force);
		_firstShakeCallback = null;
	}
	
	public bool Contains(GameObject gameObject) {
		return rigidbody.collider.bounds.Contains(gameObject.transform.position);	
	}
	
	void PlayPressUp() {
		AudioClip[] sounds = Personage.ReleaseClips();
		float abs = 2*Mathf.Abs (_percentage);
		_sound.PlayNatural(sounds[Id], StripPitch(abs), abs);	
	}
	
	void PlayPressDown() {
	}
	
	public void OnMove(InputInfo input) {
		_pendentInput.Cancel(PlayPressDown);
		_freeMove = true;
		
		AddToPercentage(input.worldMove.x);
		
		if (ReactableInStrip(Personage.face.gameObject)) {
			// Updates where the face is positioned according to the movement
			Personage.face.Position = -Vector2.right*(_percentage*2);
		}
		
	}
	
	private void AddToPercentage(float move) {
		float newPercentage = _percentage + move/_size.x;
		float sign = Mathf.Sign(newPercentage);
		
		// If we change character
		if (Mathf.Abs (newPercentage) > 0.5f) {
			ChangePersonage(sign);
			_stripCamera.transform.localPosition = new Vector3(sign*_size.x/2, 0, 0);
			_target += new Vector3(sign*_size.x, 0, 0);
			newPercentage -= sign;
		}
		
		// if we change direction
		if (_percentage*newPercentage <= 0) {
			SetSideCameras(newPercentage);
		}
		
		_percentage = newPercentage;
		_target -= Vector3.right*move;
	}
	
	public void SetSideCameras(float percentage) {
		if (percentage < 0) {
			_stripCamera.Left(_personage.Left.Layer);
		} else {
			_stripCamera.Right(_personage.Right.Layer);
		}	
	}
	
	public void ShakeUpdateCameras(float percentage) {
		if (!_freeMove) {
			SetSideCameras(percentage);
		}
	}
	
	private bool ReactableInStrip(GameObject go) {
		int id = go.GetInstanceID();
		int layer;
		
		_reactableToLayer.TryGetValue(id, out layer);
		
		return layer == Layer;
	}
	
	private void ChangePersonage(float sign) {
		_personageChangeTime = Time.time;
		_stripCamera.Change ();
		
		if (sign < 0) {
			Personage = _personage.Left;
		} else {
			Personage = _personage.Right;	
		}
	}
	
	private void OnDrawGizmos() {
		Color c = Gizmos.color;
		Color green = Color.green;
		Color blue = Color.blue;
		
		foreach (Dictionary<int, GameObject> react in _reactables.Values) {
			foreach (GameObject obj in react.Values) {
				Gizmos.color = (obj.layer == Layer ? green : blue);
				Gizmos.DrawLine(obj.transform.position, transform.position);
				Handles.color = Gizmos.color;
				Handles.Label(obj.transform.position, obj.name);
			}
		}
		
		Gizmos.color = c;
	}
}
