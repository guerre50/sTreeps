using UnityEngine;
using System.Collections;

public class Intro : GameStateBehaviour {
	private StripController _stripController;
	private GameObject _title;
	private GameObject _playButton;
	
	void Start() {
		InputController.instance.InteractiveLayer = "GUI";
		_title = GameObject.Find("title");
		_playButton = GameObject.Find ("playButton"); 
		_stripController = StripController.instance;
		_.On (Triggers.StartGame, Play);
		
	}
	
	void Update() {
		if (_outroFinished != null) {
			if (!_stripController.Shaking) {
				OutroFinished();
			}
		} else {
			_stripController.IdleRotate();	
		}
		
	}
	
	void Play(object obj, System.EventArgs args) {
		_title.animation.Play("titleDisappear");
		_playButton.animation.Play("playButtonDisappear");
		_stripController.Shake();
		Next ();
	}
	
	void OnDestroy() {
		_title.SetActive(false);
		_playButton.SetActive(false);
	}
}
