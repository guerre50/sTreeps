using UnityEngine;
using System.Collections;

public class Owl : MonoBehaviour {
	public GameObject owl;
	private float blinkTime;
	private Vector2 blinkRange = new Vector2(2.0f, 5.0f);
	private bool _blink = true;
	
	void Start () {
		Blink ();
		owl.animation["Idle"].layer = 1;
		owl.animation["Rotate"].layer = 1;
		owl.animation["Blink"].layer = 2;
	}
	
	private void Blink() {
		_.Wait(Random.Range(blinkRange.x, blinkRange.y)).Done(Blink);
		if (_blink) {
			owl.animation.Play("Blink");
		}
	}
	
	public void OnPressDown(InputInfo input) {
		if (!_blink) return;
		_blink = false;
		owl.animation.CrossFade("Rotate", 1.0f, PlayMode.StopAll);
		owl.animation.CrossFadeQueued("Idle", 1.0f);
		_.Wait (3.0f).Done (EnableBlink);
	}
	
	public void EnableBlink() {
		_blink = true;	
	}
	
}
