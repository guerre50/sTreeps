using UnityEngine;
using System.Collections;

public class Flower : Location {
	public GameObject flower;
	public bool open;
	
	public void Start() {
		open = false;
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
