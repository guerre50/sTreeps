using UnityEngine;
using System.Collections;

public class Strip : MonoBehaviour {
	public int Id;

	private StripCamera _stripCamera;
	private InputController _input;
	private float percentage;
	private Vector3 _size;
	private int _layer;
	private Personage _personage;
	
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
			_personage = value;
			_stripCamera.Layer = _personage.Layer;
		}
	}
	 
	
	void Awake () {
		_stripCamera = gameObject.GetComponentInChildren<StripCamera>();
		_input = InputController.instance;
	}
	
	void Start() {
		percentage = 0.0f;
	}
	
	void Update() {
	}

	public void RenderCam(Rect rect, Vector3 position, Vector3 size) {
		_size = size;
		_stripCamera.SetViewport(rect, size.y/2, size);

		transform.position = position;
		GetComponent<BoxCollider>().size = size;
	}

	void OnPressDown(InputInfo input) {
		//Debug.Log ("Down"+ Id);
		_input.HitLayerSendMessage(input, _stripCamera.Layer, "OnPressDown");
	}
	
	void OnPressUp(InputInfo input) {
		//Debug.Log ("Up" + Id);
		percentage = 0.0f;
		
		Hashtable parameters = new Hashtable();
		parameters["time"] = 0.5f;
		parameters["position"] = transform.position;
		parameters["easetype"] = iTween.EaseType.easeOutBounce;
		iTween.MoveTo(_stripCamera.gameObject, parameters);
	}
	
	void OnPress(InputInfo input) {
		//Debug.Log ("Press"+ Id);
		_input.HitLayerSendMessage(input, _stripCamera.Layer, "OnPress");
	}	
	
	void OnMove(InputInfo input) {
		//Debug.Log ("Move"+ Id);
		
		float move = input.worldMove.x/_size.x;
		float newPercentage = percentage + move;
		
		
		// If we change character
		if (Mathf.Abs (newPercentage) > 0.5f) {
			float sign = Mathf.Sign(newPercentage);
			
			_stripCamera.Change ();
			if (sign < 0) {
				_personage = _personage.Left;
			} else {
				_personage = _personage.Right;	
			}
			
			_stripCamera.transform.localPosition = new Vector3(sign*_size.x/2, 0, 0);
			newPercentage += -sign;
		}
		
		// If we change direction
		if (percentage*newPercentage <= 0) {
			if (newPercentage < 0) {
				_stripCamera.Left(_personage.Left.Layer);
			} else {
				_stripCamera.Right(_personage.Right.Layer);
			}
		}
	
		percentage = newPercentage;
		_stripCamera.transform.Translate(-input.worldMove.x, 0, 0);
		
	}
	
	void OnEnter(InputInfo input) {
		//Debug.Log ("Enter"+ Id);
	}
	
	void OnLeave(InputInfo input) {
		//Debug.Log ("Leave"+ Id);
	}

}
