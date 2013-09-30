using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {
	
	public enum Location {
		Nest,
		Branch,
		Air
	}
	
	public enum State {
		Awake,
		Sleeping
	}
	
	private State _state = State.Awake;
	private Location _location = Location.Nest;
	public GameObject bird;
	
	public BirdNest nest;
	public GameObject branch;
	private GameObject _target;
	private Location _targetLocation;
	public iTweenPath path;
	private bool _finishFly = false;
	
	delegate void Callback();
	private Callback _callback; 
	private Hashtable _pathArgs;
	private Vector3 _pathTarget = Vector3.zero;
	private float _nestOffset;
	private Logic _logic;
		
	void Start () {
		_logic = Logic.instance;
		_pathArgs = iTween.Hash("easetype", iTween.EaseType.linear, "time", 1.0f, "oncomplete", "OnCompletePath", "oncompletetarget", gameObject);
		_nestOffset = transform.position.z - nest.transform.position.z;
	}
	
	public void Poke () {
		if (_state == State.Awake) {
			switch (_location) {
			case Location.Nest:
				FlyTo(Location.Branch);
			break;
			case Location.Air:
				iTween.PunchPosition(gameObject, Vector3.up*Random.Range (1.0f, 3.0f), Random.Range(0.5f, 1.5f));  
			break;
			case Location.Branch:
				FlyTo(Location.Nest);
			break;
			}
		}
	}
	
	public void Update() {
		if (_finishFly) {
			Fly();
		}
		
		if (_logic.IsRainy()) {
			if (_location == Location.Branch) {
				FlyTo(Location.Nest);	
			}
		}
		
	}
	
	private void Fly() {
		Vector3 target = (_pathTarget != Vector3.zero ? _pathTarget : _target.transform.position); 
			
		transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime*30);
		if (Vector3.Distance(transform.position, target) < 0.5f) {
			_callback();
		}
	}
	
	private void FollowPath() {
		iTween.MoveTo(gameObject, _pathArgs);
		_finishFly = false;
		_pathTarget = Vector3.zero;
	}

	
	public void FlyTo(Location location) {
		gameObject.transform.parent = null;
		bird.animation.CrossFade("Fly", 0.5f);
		
		
		switch(location) {
		case Location.Branch:
			_target = branch;
			_pathTarget = path.nodes[0];
			transform.up = Vector3.up;
			nest.bird = null;
			_pathArgs["path"] = iTweenPath.GetPath("birdPath");
			break;
		case Location.Nest:
			_pathTarget = path.nodes[path.nodeCount - 1];
			_target = nest.gameObject;
			_pathArgs["path"] = iTweenPath.GetPathReversed("birdPath");
			break;
		}
		transform.position += -Vector3.forward*_nestOffset;
		_targetLocation = location;
		_location = Location.Air;
		_callback = FollowPath;
		_finishFly = true;
		_.Trigger(Triggers.EyesightFollow, gameObject);
	}
	
	public void OnCompletePath() {
		_finishFly = true;
		_callback = DestinationReached;
	}
			
	public void DestinationReached() {
		_location = _targetLocation;
		bird.animation.CrossFade("Idle", 1.0f);
		
		switch(_location) {
		case Location.Branch:
			gameObject.transform.parent = branch.transform;
			break;
		case Location.Nest:
			transform.position += Vector3.forward*_nestOffset;
			gameObject.transform.parent = nest.transform;
			nest.bird = this;
			transform.up = nest.transform.up;
			break;
		}	
		_.Trigger(Triggers.EyesightUnfollow, gameObject);
		_finishFly = false;
	}
	
	public void OnPressDown(InputInfo input) {
		Poke ();	
	}
				
}
