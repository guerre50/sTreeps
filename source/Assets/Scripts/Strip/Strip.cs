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
	private float _pressTimeMargin = 0.1f;
	private InputInfo _pressInput;
	private Promise _pendentInput;
	private float _personageChangeTime = 0.0f;
	private Vector3 _target;
	private bool _freeMove = false;
	
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
		_target = _stripCamera.transform.position;
	}
	
	void Update() {
		 if (_freeMove) {
			iTween.MoveUpdate(_stripCamera.gameObject, _target, 0.5f);	
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
		AddReactable(collider.gameObject);
	}
	
	void OnTriggerExit(Collider collider) {
		RemoveReactable(collider.gameObject);
	}
	
	
	void AddReactable(GameObject reactable) {
		if (IsReactable(reactable)) {
			int layer = 1 << reactable.layer;
			Dictionary<int, GameObject> reactablesInLayer;
			
			if (!_reactables.TryGetValue(layer, out reactablesInLayer)) {
				reactablesInLayer = new Dictionary<int, GameObject>();
				_reactables[layer] = reactablesInLayer;
			}
			
			int id = reactable.GetInstanceID();
			if (!reactablesInLayer.ContainsKey(id)) {
				reactablesInLayer.Add (id, reactable);
			}
		}
	}
	
	void RemoveReactable(GameObject reactable) {
		if (IsReactable(reactable)) {
			int layer = 1 << reactable.layer;
			Dictionary<int, GameObject> reactablesInLayer;
			
			if (_reactables.TryGetValue(layer, out reactablesInLayer)) {
				int id = reactable.GetInstanceID();
				
				reactablesInLayer.Remove(id);
			}
		}
	}
	
	bool IsReactable(GameObject gameObject) {
		return 	gameObject.tag == "Reactable";
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
	
	public void OnMove(InputInfo input) {
		_pendentInput.Cancel();
		_freeMove = true;
		
		float move = input.worldMove.x/_size.x;
		float newPercentage = _percentage + move;
		
		// If we change character
		if (Mathf.Abs (newPercentage) > 0.5f) {
			float sign = Mathf.Sign(newPercentage);
			
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
		
		
	void OnEnter(InputInfo input) {
		//Debug.Log ("Enter"+ Id);
	}
	
	void OnLeave(InputInfo input) {
		//Debug.Log ("Leave"+ Id);
	}

}
