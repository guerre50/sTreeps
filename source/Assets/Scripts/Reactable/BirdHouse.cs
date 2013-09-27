using UnityEngine;
using System.Collections;

public class BirdHouse : MonoBehaviour {
	private bool rotating = false;
	public BirdNest birdNest;
	public SoundPressPlayer sounds;
	
	void Start() {
		sounds.enabled = false;	
	}
	
	void OnPressDown(InputInfo input) {
		if (!rotating) {
			birdNest.Poke();
			sounds.Play(false);
			AnimateHousePress();
		}
	}
	
	void AnimateHousePress() {
		Hashtable args = new Hashtable();
				
		args["time"] = 0.8f;
		args["oncomplete"] = "PressCompleted";
		args["oncompletetarget"] = gameObject;
		args["z"] = Mathf.Sign (Random.Range (-1.0f,1.0f))*Random.Range(20, 40);
		
		rotating = true;
		
		iTween.PunchRotation(gameObject, args);	
	}
	
	public void PressCompleted() {
		rotating = false;
	}
}
