using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	public AudioClip[] audio;
	private SoundController _soundController;
	
	void Start() {
		_soundController = SoundController.instance;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			_.Trigger(Triggers.StartGame, null, null);
		}
	}
	
	void OnPressDown(InputInfo input) {
		_soundController.Play(audio[0]);
		animation.Play("playButtonPress");
	}
	
	void OnPressUp(InputInfo input) {
		_.Trigger(Triggers.StartGame, null, null);
		_soundController.Play(audio[1]);
	}
}
