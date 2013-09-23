using UnityEngine;
using System.Collections;

public class BirdNest : MonoBehaviour {
	public Bird bird;
	
	public void Poke() {
		if (bird) {
			bird.Poke();
		}
	}
}
