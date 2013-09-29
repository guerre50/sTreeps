using UnityEngine;
using System.Collections;
using Radical;

public class SoundSelectPlayer : Reactable {
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
	
	public override void OnDeselect () {
		_onComplete = PauseAudio; 
		_volume.Value = 0.0f;
		_volume.Duration = _fadeOutDuration;
		
	}
	
	public override void OnSelect () {
		_volume.Value = 1.0f;
		_volume.Duration = _fadeInDuration;
		if (!enabled) return; // SnorePlayer sets it as disabled to indicate that shouldn't play
		audio.Play();
	}
	
	void PauseAudio() {
		audio.Pause();
	}
	
	void OnEnable() {
		if (selected) {
			audio.Play ();
		}
	}
	
	void OnDisable() {
		PauseAudio();
	}
}
