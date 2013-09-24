using UnityEngine;
using System.Collections;

public class YoungSalute : MonoBehaviour {
	private Logic _logic;
	
	void Start(){
		_logic = Logic.instance;
	}
	
	public void OnPressDown(InputInfo input) {
		_logic.Salute();
	}
}
