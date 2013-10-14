using UnityEngine;
using System.Collections;

public class Playing : GameStateBehaviour {
	
	void Start() {
		InputController.instance.InteractiveLayer = "Strips";	
	}
}
