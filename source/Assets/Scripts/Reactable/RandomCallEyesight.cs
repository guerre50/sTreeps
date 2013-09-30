using UnityEngine;
using System.Collections;

public class RandomCallEyesight : MonoBehaviour {
	public float probability = 0.2f;
	public Vector2 checkRange = new Vector2(2.0f, 10.0f);
	
	void Start() {
		_.Wait (Random.Range (2.0f, 10.0f)).Done (CheckEyesight);	
	}
	
	void CheckEyesight() {
		if (Random.Range(0.0f, 1.0f) < probability) {
			_.Trigger(Triggers.EyesightFollow, gameObject);
		}
		_.Wait (Random.Range (checkRange.x, checkRange.y)).Done (CheckEyesight);
	}
}
