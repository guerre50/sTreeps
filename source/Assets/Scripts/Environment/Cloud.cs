using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {
	private ParticleSystem _rain;
	private int _maxRain = 50;
	private float _rainIncreaseTime = 1.0f;
	private float _rainTime;
	
	void Awake () {
		_rain = transform.GetComponentInChildren<ParticleSystem>();
	}
	
	void Update () {
		RainAnimation();
	}
	
	public void StartRain() {
		_rain.Play();
		_rain.emissionRate = 1;
		_rainTime = _rainIncreaseTime;
	}
	
	public void StopRain() {
		_rain.Stop ();
	}
	
	public void RainAnimation() {
		if (_rain.emissionRate <= _maxRain) {
			_rainTime -= Time.deltaTime;
			
			if (_rainTime <= 0.0f) {
				_rainTime = _rainIncreaseTime;
				_rain.emissionRate += 3;
			}
		}
	}
	
	void Deselect() {
		StopRain();
	}
	
	void Select() {
		StartRain();	
	}
}
