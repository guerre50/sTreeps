using UnityEngine;
using System.Collections;

public class Maraca : MonoBehaviour {
	Logic _logic;
	
	void Start () {
		_logic = Logic.instance;
	}
	
	void OnPressDown(InputInfo input) {
		_logic.Dance();
	}
}
