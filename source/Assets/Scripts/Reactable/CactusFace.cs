using UnityEngine;
using System.Collections;

public class CactusFace : FaceAnimator {
	public SoundPressPlayer maracasSound;
	private int _dayClip = 0;
	private int _nightClip = 1;
	
	public void Start () {
		base.Init(PersonageType.Cactus);
		InitListeners();
	}
	
	void InitListeners() {
		// TO-DO move this
		//_.On ("BotherSleep", OnBotherSleep);	
	}
	
	void OnPressDown(InputInfo input) {
		Dance ();
	}
	
	public void Dance() {
		if (_logic.Dance()) {
			if (_logic.IsNightTime()) {
				PlayBotherSleep();
			} else {
				PlayDance();		
			}
		}	
	}
	
	public void PlayBotherSleep() {
		maracasSound.Play(false, _nightClip);
	}
	
	public void PlayDance() {
		maracasSound.Play(false, _dayClip);	
	}
	
	/*public void OnBotherSleep(object sender, System.EventArgs events) {
		PersonageType type = (PersonageType) sender;
		
		if (type == this._personageType) {
			PlayBotherSleep();
		}
	}*/
}
