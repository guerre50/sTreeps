using UnityEngine;
using System.Collections;
using Radical;

public class Young : Personage {
	private Eyes eyes;
	private Material _eyeMaterialLeft;
	private Material _eyeMaterialRight;
	private SmoothFloat _eyeOffset;
	
	public void Awake() {
		eyes = GetComponent<Eyes>();
		eyes.Identifier = "Young";
		type = PersonageType.Young;
	}
	
	
	public void NightTime() {
		eyes.Close(3.0f);
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
	
	
}
