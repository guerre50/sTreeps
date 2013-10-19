using UnityEngine;
using System.Collections;

public class Hive : Location {
	void Start() {
		Vector3 scale = gameObject.transform.localScale;
		scale.y = scale.x;
		gameObject.transform.localScale = scale;
	}
}
