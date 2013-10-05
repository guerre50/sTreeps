using UnityEngine;
using System.Collections;

public class ParallaxItem : Reactable {
	public float distance;
	private float _originalX;
	
	void Start() {
		_originalX = transform.position.x;	
	}
	
	void LateUpdate () {
		if (strip) {
			UpdateParallax(strip.Percentage(gameObject.layer));
		}
	}
	
	void UpdateParallax(float percentage) {
		Vector3 position = transform.position;
		position.x = _originalX + percentage*distance;
		transform.position = position;
	}
}
