using UnityEngine;
using System.Collections;

public class BirdHouse : MonoBehaviour {
	private bool rotating = false;
	public BirdNest birdNest;
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPressDown(InputInfo input) {
		Debug.Log ("press house");
		
		if (!rotating) {
			birdNest.Poke();
			AnimateHousePress();
		}
	}
	
	void AnimateHousePress() {
		Hashtable args = new Hashtable();
				
		args["time"] = 2.0f;
		args["oncomplete"] = "PressCompleted";
		args["oncompletetarget"] = gameObject;
		args["z"] = Mathf.Sign (Random.Range (-1.0f,1.0f))*Random.Range(20, 60);
		
		rotating = true;
		
		iTween.PunchRotation(gameObject, args);	
	}
	
	public void PressCompleted() {
		rotating = false;
	}
}
