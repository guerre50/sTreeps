using UnityEngine;
using System.Collections;
using Radical;

public class SoundSelectPlayer : MonoBehaviour {
	SmoothFloat _volume;
	private float _fadeInDuration = 5.0f;
	private float _fadeOutDuration = 0.5f;
	private Callback _onComplete;
	
	
	void Awake() {
		_volume = 0.0f;	
	}
	
	void Update() {
		audio.volume = _volume;
		if (_volume.IsComplete && _onComplete != null) {
			_onComplete();
			_onComplete = null;
		}
	}
	
	void Deselect () {
		if (!enabled) return;
		_onComplete = PauseAudio; 
		_volume.Value = 0.0f;
		_volume.Duration = _fadeOutDuration;
	}
	
	void Select () {
		if (!enabled) return;
		audio.Play();
		_volume.Value = 1.0f;
		_volume.Duration = _fadeInDuration;
	}
	
	void PauseAudio() {
		audio.Pause();
	}
	
	
	void OnEnable() {
		audio.Play ();
	}
	
	void OnDisable() {
		PauseAudio();
	}
}
