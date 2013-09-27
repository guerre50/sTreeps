using UnityEngine;
using System.Collections;

public class CactusFace : MonoBehaviour {
	Logic _logic;
	public SoundPressPlayer maracasSound;
	private int _dayClip = 0;
	private int _nightClip = 1;
	
	void Start () {
		_logic = Logic.instance;
		_logic.actionHandler += ActionListener;
	}
	
	void OnPressDown(InputInfo input) {
		_logic.Dance();
	}
	
	void ActionListener(Action previous, Action newAction) {
		if (newAction == Action.Dance) {
			maracasSound.Play(false, _dayClip);	
		} else if (newAction == Action.Bother) {
			maracasSound.Play(false, _nightClip);	
		}
	}
}
