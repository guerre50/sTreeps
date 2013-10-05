using UnityEngine;
using System.Collections;

public class SoundController : Singleton<SoundController> {
	CameraController _camera;
	
	void Start () {
	}
	
	void Update () {
	
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
}
