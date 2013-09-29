using UnityEngine;
using System.Collections;

public class AnimateTrail : MonoBehaviour {
	private LineRenderer _trail;
	private Vector3 _lastPosition;
	public float stepDistance = 1.0f;
	private int _numPositions;
	public float offsetIncreaseStep = 0.3f;
	private float _totalDistance;
	private float _currentStepDistance;
	public Bee _bee;
	
	
	void Start () {
		_trail = transform.GetComponentInChildren<LineRenderer>();
		_lastPosition = transform.position;
		_numPositions = 1;
		_trail.SetVertexCount(1);
		_trail.SetPosition(0, transform.position);
		_totalDistance = _currentStepDistance = 0;
	}
	
	void Update () {
		if (_bee.isFlying) {
			float deltaDistance = Vector3.Distance(_lastPosition, transform.position);
			_currentStepDistance += deltaDistance;
			_totalDistance += deltaDistance;
			
			if (_currentStepDistance > stepDistance) {
				_currentStepDistance = 0.0f;
				_trail.SetVertexCount(_numPositions);
				_trail.SetPosition(_numPositions - 1, transform.position);
				_numPositions++;
				Vector2 tiling = _trail.material.mainTextureScale;
				tiling.x += offsetIncreaseStep;
				_trail.material.mainTextureScale = tiling;
			}
		}
		_lastPosition = transform.position;
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawSphere(transform.position, 0.1f);	
	}
}
