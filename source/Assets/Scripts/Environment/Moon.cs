using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {
	public GameObject nightOverlay;
	private Vector3 _overlayTarget;
	private Vector3 _nightPosition;
	private Vector3 _dayPosition;
	private float _overlaySpeed;
	
	public float inSpeed;
	public float outSpeed;
	
	
	void Awake () {
		_nightPosition = nightOverlay.transform.position;
		_dayPosition = _nightPosition - Vector3.up*(nightOverlay.transform.localScale.y*10);
		nightOverlay.transform.position = _dayPosition;
		_overlayTarget = _dayPosition;
		nightOverlay.SetActive(true);
	}
	
	void Update () {
		nightOverlay.transform.position = Vector3.MoveTowards(nightOverlay.transform.position, _overlayTarget, Time.deltaTime*_overlaySpeed);
	}
	
	
	void Select() {
		_overlayTarget = _nightPosition;
		_overlaySpeed = inSpeed;
	}
	
	void Deselect() {
		_overlayTarget = _dayPosition;
		_overlaySpeed = outSpeed;
	}
		
}
