using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
	public GameObject bubble;
	private Logic _logic;
	private bool _night;
	
	void Start() {
		_logic = Logic.instance;
		_night = !_logic.IsNightTime();
	}
	
	void Update() {
		bool night = _logic.IsNightTime();
		
		if (_night != night) {
			_night = night;
			
			bubble.renderer.enabled = night;
		}
	}
	
	public void Explode() {
		bubble.renderer.enabled = false;
		_.Wait(1.0f).Done(Grow);
	}
	
	public void Grow() {
		if (_night) {
			bubble.renderer.enabled = true;	
		}
	}
}
