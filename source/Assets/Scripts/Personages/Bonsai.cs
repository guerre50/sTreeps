using UnityEngine;
using System.Collections;

public class Bonsai : Personage {
	private Eyes eyes;
	
	public void Awake() {
		eyes = GetComponent<Eyes>();
		eyes.Identifier = "Bonsai";
		type = PersonageType.Bonsai;
		face = GetComponent<FaceAnimator>();
	}
	
	public void NightTime() {
		eyes.Close(2.0f);
	}
	
	public void Cloudy() {
		eyes.Open(2.0f);
	}
	
	public void Sunny() {
		eyes.Open(2.0f);
	}
	
	public void Yawn() {
		eyes.Open(4.0f);
	}
	
	public void Blink() {
			
	}
	
	public override AudioClip[] ReleaseClips() {
		return clips.BonsaiRelease;	
	}
}
