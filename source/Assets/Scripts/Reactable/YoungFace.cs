using UnityEngine;
using System.Collections;

public class YoungFace : FaceAnimator {
	private GameObject _followObject;
	
	public void Start(){
		base.Init(PersonageType.Young);
	}
	
	public void OnPressDown(InputInfo input) {
		_logic.Salute();
	}
}
