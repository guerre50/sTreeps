using UnityEngine;
using System.Collections;

public class Windmill : MonoBehaviour {
	public GameObject windmill;
	private bool _playing = false;
	public SoundPressPlayer sounds;
	
	public void OnPressDown(InputInfo input) {
		AnimateWindmill();	
	}
	
	private void AnimateWindmill() {
		if (!_playing) {
			Hashtable args = new Hashtable();
			_playing = true;
			args["ease"] = iTween.EaseType.easeOutElastic;
			args["time"] = 2.0f;
			args["y"] = 1000;
			args["oncomplete"] = "OnCompleteAnimation";
			args["oncompletetarget"] = gameObject;
			
			iTween.PunchRotation(windmill, args);
			sounds.Play(false);
		}
	}
	
	public void OnCompleteAnimation() {
		_playing = false;	
	}
}
