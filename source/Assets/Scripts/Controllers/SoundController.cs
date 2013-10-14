using UnityEngine;
using System.Collections;

public class SoundController : Singleton<SoundController> {
	CameraController _camera;
	float _fadeTime;
	float _fadeDuration;
	
	void Start () {
	}
	
	void Update () {
		if (_fadeDuration > 0) {
			audio.volume = 1 - Mathf.Lerp (0, 1, (Time.time - _fadeTime)/_fadeDuration);
			if (audio.volume <= 0) {
				_fadeDuration = 0;	
			}
		}
	}
	
	public void Play(AudioClip audio) {
		this.audio.PlayOneShot(audio);	
	}
	
	public void PlayNatural(AudioClip audio, float pitch = -1, float volume = 1) {
		if (pitch < 0) {
			pitch = Random.Range (0.9f, 1.1f);	
		}
		
		this.audio.volume = volume; 
		
		this.audio.pitch = pitch;
		this.audio.PlayOneShot(audio);	
	}
	
	public void PlayFaded(AudioClip audio, float duration) {
		this.audio.clip = audio;
		this.audio.volume = 1.0f;
		_fadeTime = Time.time;
		_fadeDuration = duration;
		this.audio.PlayOneShot(audio);
	}
	
	
}
