using UnityEngine;
using System.Collections;

public class Windmill : MonoBehaviour {
	public GameObject windmill;
	
	void Update () {
	
	}
	
	public void OnPressDown(InputInfo input) {
		AnimateWindmill();	
	}
	
	private void AnimateWindmill() {
		Hashtable args = new Hashtable();
		
		args["ease"] = iTween.EaseType.easeOutElastic;
		args["time"] = 2.0f;
		args["y"] = 1000;
		
		iTween.PunchRotation(windmill, args);
	}
}
