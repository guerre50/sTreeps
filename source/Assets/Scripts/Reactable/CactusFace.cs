using UnityEngine;
using System.Collections;

public class CactusFace : FaceAnimator {
	public SoundPressPlayer maracasSound;
	private int _dayClip = 0;
	private int _nightClip = 1;
	
	public void Start () {
		base.Init(PersonageType.Cactus);
	}
	
	void OnPressDown(InputInfo input) {
		Dance ();
	}
	
	public void Dance() {
		if (_logic.Dance()) {
			if (_logic.IsNightTime()) {
				maracasSound.Play(false, _nightClip);	
			} else {
				maracasSound.Play(false, _dayClip);		
			}
		}	
	}
}
