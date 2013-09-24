using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {
	public GameObject flower;
	bool open = true;
	
	public void Start() {
		Animate ();	
	}
	
	public void OnPressDown(InputInfo input) {
		open = !open;
		Animate ();
	}
	
	public void Animate() {
		if (open) {
			flower.animation.Play("Open");	
		} else {
			flower.animation.Play ("Close");
		}
	}
}
