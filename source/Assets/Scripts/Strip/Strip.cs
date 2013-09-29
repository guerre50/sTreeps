using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Strip : MonoBehaviour {
	public int Id;

	private StripCamera _stripCamera;
	private InputController _input;
	private float _percentage = 0.0f;
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
	private PersonageController _personageController;
	
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
		_stripCamera = gameObject.GetComponentInChildren<StripCamera>();
		_input = InputController.instance;
	}
	
	void Start() {
		Select();
		_personageController = PersonageController.instance;
		_target = _stripCamera.transform.position;
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
		//Debug.Log ("Enter "+ LayerMask.LayerToName(Layer)+ " "  + collider.name);
		if (TryGetReactable(collider.gameObject, out reactable)) {
			AddReactable(reactable);
		}
	}
	
	void OnTriggerExit(Collider collider) {
		Reactable reactable;
		//Debug.Log ("Exit "+ LayerMask.LayerToName(Layer)+ " "  + collider.name);
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
		
		_percentage = 0.0f;
		Hashtable parameters = new Hashtable();
		parameters["time"] = 0.5f;
		parameters["position"] = transform.position;
		parameters["easetype"] = iTween.EaseType.easeOutBounce;
		
		iTween.MoveTo(_stripCamera.gameObject, parameters);
		_target = transform.position;
		_freeMove = false;
	}
	
	public bool Contains(GameObject gameObject) {
		return rigidbody.collider.bounds.Contains(gameObject.transform.position);	
	}
	
	public void OnMove(InputInfo input) {
		_pendentInput.Cancel();
		_freeMove = true;
		
		float move = input.worldMove.x/_size.x;
		float newPercentage = _percentage + move;
		float sign = Mathf.Sign(newPercentage);
		
		if (sign < 0) {
			//_personageController.sTreeps.animation.Blend(Personage.type+"Left", Mathf.Lerp(0, 1.0f, Mathf.Abs (newPercentage)));
		} else {
			//_personageController.sTreeps.animation.Blend(Personage.type+"Right", Mathf.Lerp(0, 1.0f, Mathf.Abs (newPercentage)));
		}
		
		// If we change character
		if (Mathf.Abs (newPercentage) > 0.5f) {
			ChangePersonage(sign);
			_stripCamera.transform.localPosition = new Vector3(sign*_size.x/2, 0, 0);
			_target += new Vector3(sign*_size.x, 0, 0);
			newPercentage -= sign;
		}
		
		// If we change direction
		if (_percentage*newPercentage <= 0) {
			if (newPercentage < 0) {
				_stripCamera.Left(_personage.Left.Layer);
			} else {
				_stripCamera.Right(_personage.Right.Layer);
			}
		}
	
		_percentage = newPercentage;
		_target -= Vector3.right*input.worldMove.x;
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

}
