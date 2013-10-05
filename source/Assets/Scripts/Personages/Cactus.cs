using UnityEngine;
using System.Collections;

public class Cactus : Personage {
	void Awake() {
		type = PersonageType.Cactus;
		face = GetComponent<FaceAnimator>();
	}
	
	public override AudioClip[] ReleaseClips() {
		return clips.CactusRelease;	
	}
}
