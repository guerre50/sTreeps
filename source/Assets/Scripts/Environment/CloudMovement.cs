using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {
	public Vector2 velocityRange = Vector2.one;
	public float movementRadius = 3.0f;
	
	private Vector3 _target;
	private Vector3 _originalPosition;
	private float _velocity;
	
	public Color[] _brightness;
	private int _currentBrightness;
	
	void Start () {  
		_originalPosition = transform.localPosition;
		UpdateTarget();
	}
	
	void LateUpdate () {
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, _target, _velocity*Time.deltaTime);
		if (Vector3.Distance(transform.localPosition, _target) < 0.1f) {
			UpdateTarget();	
		}
	}
	
	void UpdateTarget() {
		_target = _originalPosition + new Vector3(RandomDistance(), 0, RandomDistance());
		_velocity = RandomVelocity();
	}
	
	float RandomDistance() {
		return Random.Range(-movementRadius, movementRadius);	
	}
	
	float RandomVelocity() {
		return Random.Range(velocityRange.x, velocityRange.y);	
	}
	
	public void AnimateBrightness() {
		iTween.ColorTo(gameObject, _brightness[_currentBrightness], 0.5f);	
	}
	
	public void StartRain() {
		_currentBrightness = 1;
		AnimateBrightness();
	}
	
	public void StopRain() {
		_currentBrightness = 0;
		AnimateBrightness();
	}
	
	
}
