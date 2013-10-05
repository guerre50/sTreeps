using UnityEngine;
using System.Collections;

public class ParallaxGroup : Reactable {
	private ParallaxItem[] parallaxItems;
	
	void Start() {
		parallaxItems = GetComponentsInChildren<ParallaxItem>();		
	}
	
	public override void  OnSelect() {
		foreach(ParallaxItem t in parallaxItems) {
			t.strip = strip;
		}
	}
}
