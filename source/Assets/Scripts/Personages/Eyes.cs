using UnityEngine;
using System.Collections;
using Radical;


public class Eyes : MonoBehaviour {
	private Material _eyeMaterialLeft;
	private Material _eyeMaterialRight;
	private SmoothFloat _eyeOffset;
	public string Identifier;
	private Logic _logic;
	
	public void Start() {
		_eyeOffset = new SmoothFloat(0);	
		_eyeOffset.Ease = EasingType.Cubic;
		_eyeOffset.Mode = SmoothingMode.smooth;
		_eyeOffset.Duration = 2.0f;
		_eyeMaterialLeft = (GameObject.Find (Identifier+"_eye_Left")).renderer.material;
		_eyeMaterialRight = (GameObject.Find (Identifier+"_eye_Right")).renderer.material;
		
		_logic = Logic.instance;
		_.Wait (2.0f).Done (Blink);
	}
	
	public void Update() {
		Vector2 offset = _eyeMaterialLeft.mainTextureOffset;	
		offset.y = _eyeOffset;
		_eyeMaterialLeft.mainTextureOffset = offset;
		_eyeMaterialRight.mainTextureOffset = offset;
	}
	
	public void Open(float time) {
		_eyeOffset.Value = 1;
		_eyeOffset.Duration = time;
	}
	
	public void Close(float time) {
		_eyeOffset.Value = 0;
		_eyeOffset.Duration = time;
	}
	
	public void Blink() {
		if (!_logic.IsNightTime()) {
			CloseEyes();
			_.Wait(0.4f).Done(OpenEyes);
			if (Random.Range (0, 2) > 0) {
				_.Wait(0.9f).Done(CloseEyes);
				_.Wait(1.5f).Done(OpenEyes);
			}
		}
		_.Wait(5.0f + Random.Range (0, 10)).Done (Blink);
	}
	
	public void OpenEyes() {
		if (!_logic.IsNightTime()) {
			Open (0.2f);
		}
	}
	
	public void CloseEyes() {
		if (!_logic.IsNightTime()) {
			Close(0.2f);
		}
	}
}
