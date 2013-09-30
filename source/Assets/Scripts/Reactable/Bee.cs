using UnityEngine;
using System.Collections;
using Radical;

public class Bee : Reactable {
	public GameObject paths;
	public Flower flower;
	public Hive hive;
	public GameObject bee;
	
	private int numberPaths;
	private Hashtable pathArgs;
	private Location target;
	private SmoothVector3 targetPosition;
	private enum BeeLocation { Hive, Flower, Other};
	private BeeLocation location;
	private float flyDuration = 10.0f;
	private string animationName = "beeFly";
	private float standTime = 10.0f;
	private Logic _logic;
	
	void Start () {
		target = hive;
		targetPosition = transform.position;
		targetPosition.Duration = 0.5f;
		isFlying = false;
		numberPaths = paths.transform.childCount;
		InitPathArgs();
		_logic = Logic.instance;
	}
	
	void InitPathArgs() {
		pathArgs = new Hashtable();
		pathArgs["time"] = flyDuration;
		pathArgs["orienttopath"] = true;
		pathArgs["axis"] = "x";
		pathArgs["lookahead"] = 0.2f;
		pathArgs["name"] = animationName;
		pathArgs["easing"] = iTween.EaseType.easeInExpo;
	}
	
	
	
	void Update () {
		if (!isFlying) {
			transform.rigidbody.MovePosition(targetPosition);
			targetPosition.Value = target.position;
			ExecuteLocationUpdate();
		}
	}
	
	void ExecuteLocationUpdate() {
		standTime -= Time.deltaTime;
		switch (location) {
		case BeeLocation.Flower:
			if (!flower.open || standTime < 0 || _logic.IsRainy()) {
				Fly(1.0f);	
			}
			break;
		case BeeLocation.Hive:
			if ((flower.selected && flower.open && standTime < 0 && !_logic.IsRainy())) {
				Fly (0.0f);	
			}
			break;
		}
	}
	
	public override void OnDeselect() {
		base.OnDeselect();
		_.Trigger(Triggers.EyesightUnfollow, gameObject);	
	}
	
	public void OnCompletePath() {
		standTime = Random.Range(10.0f, 30.0f);
		// Player has moved the strip and the target is not visible
		int currentLayer = strip.Layer;
		if (currentLayer != target.gameObject.layer) {
			Fly ();	
		} else {
			iTween.StopByName(animationName);
			isFlying = false;
			targetPosition = transform.position;
			EndCrossStrip();
		}
		_.Trigger(Triggers.EyesightUnfollow, gameObject);
	}
	
	void Fly() {
		Fly(0.0f);	
	}
	
	void Fly(float delay) {
		transform.parent = null;
		
		isFlying = true;
		Vector3[] path = new Vector3[0];
		
		switch (location) {
		case BeeLocation.Flower:
			target = hive;
			location = BeeLocation.Hive;
			path = iTweenPath.GetPathReversed("beePath"+Random.Range(0, numberPaths));
			break;
		case BeeLocation.Hive:
			target = flower;
			location = BeeLocation.Flower;
			path = iTweenPath.GetPath("beePath"+Random.Range(0, numberPaths));
			break;
		case BeeLocation.Other:
			
			break;
		}
		_.Trigger(Triggers.EyesightFollow, gameObject);
		
		path[path.Length - 1] = target.position;
		pathArgs["path"] = path;
		pathArgs["delay"] = delay;
		_.Wait(delay + flyDuration*0.5f).Done (OnCompletePath);
		iTween.StopByName(animationName);
		iTween.MoveTo(gameObject, pathArgs);
		StartCrossStrip();
	}
	
	public void OnPressDown(InputInfo input) {
		if (!isFlying) {
			Fly ();
		}
	}
	
	public bool isFlying = false;
}
