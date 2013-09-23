using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {
	public GameObject nightOverlay;
	public ParticleSystem stars;
	
	private Vector3 _overlayTarget;
	private Vector3 _nightPosition;
	private Vector3 _dayPosition;
	private float _overlaySpeed;
	private Logic _logic;
	
	public float inSpeed;
	public float outSpeed;
	
	
	void Awake () {
		InitNightOverlay();
		_logic = Logic.instance;
	}
	
	
	void Update () {
		nightOverlay.transform.position = Vector3.MoveTowards(nightOverlay.transform.position, _overlayTarget, Time.deltaTime*_overlaySpeed);
	}
	
	void InitNightOverlay() {
		_nightPosition = nightOverlay.transform.position;
		_dayPosition = _nightPosition - Vector3.up*(nightOverlay.transform.localScale.y*10);
		nightOverlay.transform.position = _dayPosition;
		_overlayTarget = _dayPosition;
		nightOverlay.SetActive(true);	
	}
	
	
	void Select() {
		_overlayTarget = _nightPosition;
		_overlaySpeed = inSpeed;
		
		stars.Play();
		_logic.NightTime();
	}
	
	void Deselect() {
		_overlayTarget = _dayPosition;
		_overlaySpeed = outSpeed;
		
		stars.Stop ();
	}
		
}
