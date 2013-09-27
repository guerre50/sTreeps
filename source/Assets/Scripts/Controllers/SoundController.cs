using UnityEngine;
using System.Collections;

public class SoundController : Singleton<SoundController> {
	CameraController _camera;
	public AudioClip[] clips;
	
	void Start () {
		_camera = CameraController.instance;
	}
	
	void Update () {
	
	}
	
	void Play(AudioClip audio) {
		_camera.Camera.audio.PlayOneShot(audio);	
	}
}
