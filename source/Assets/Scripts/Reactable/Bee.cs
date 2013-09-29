using UnityEngine;
using System.Collections;
using Radical;

public class Bee : MonoBehaviour {
	public GameObject paths;
	public Flower flower;
	public Hive hive;
	
	private int numberPaths;
	private Hashtable pathArgs;
	private Location target;
	private SmoothVector3 targetPosition;
	private enum BeeLocation { Hive, Flower, Other};
	private BeeLocation location;
	private StripController stripController;
	private Strip currentStrip;
	private float flyDuration = 10.0f;
	private string animationName = "beeFly";
	
	void Start () {
		target = hive;
		targetPosition = target.position;
		targetPosition.Ease = EasingType.Sine;
		isFlying = false;
		numberPaths = paths.transform.childCount;
		stripController = StripController.instance;
		InitPathArgs();
	}
	
	void InitPathArgs() {
		pathArgs = new Hashtable();
		pathArgs["time"] = flyDuration;
		pathArgs["orienttopath"] = true;
		pathArgs["axis"] = "x";
		pathArgs["name"] = animationName;
		//pathArgs["oncomplete"] = "OnCompletePath";
		pathArgs["easing"] = iTween.EaseType.easeInExpo;
		//pathArgs["oncompletetarget"] = gameObject;	
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Fly();
		}
		if (!isFlying) {
			transform.position = targetPosition;
			targetPosition.Value = target.position;
			ExecuteLocationUpdate();
		}
	}
	
	void ExecuteLocationUpdate() {
		switch (location) {
		case BeeLocation.Flower:
			if (!flower.open) {
				Fly(1.0f);	
			}
			break;
		case BeeLocation.Hive:
			
			break;
		}
	}
	
	public void OnCompletePath() {
		// Player has moved the strip and the target is not visible
		int currentLayer = currentStrip.Layer;
		if (currentLayer != target.gameObject.layer) {
			Fly ();	
		} else {
			isFlying = false;
			targetPosition = transform.position;
			
			_.SetLayerRecursively(gameObject, currentLayer);
		}
	}
	
	void Fly() {
		Fly(0.0f);	
	}
	
	void Fly(float delay) {
		transform.parent = null;
		_.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Overlay"));
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
		
		path[path.Length - 1] = target.position;
		pathArgs["path"] = path;
		pathArgs["delay"] = delay;
		_.Wait(delay + flyDuration*0.7f).Done (OnCompletePath);
		iTween.StopByName(animationName);
		iTween.MoveTo(gameObject, pathArgs);
	}
	
	public void OnPressDown(InputInfo input) {
		if (!isFlying) {
			Fly ();
		}
	}
	
	public void OnTriggerEnter(Collider collider) {
		if (target != null) {
			Strip strip = stripController.GameObjectToStrip(collider.gameObject);
			if (strip != null) {
				currentStrip = strip;
			}
		}
	}
	
	public bool isFlying = false;
}
