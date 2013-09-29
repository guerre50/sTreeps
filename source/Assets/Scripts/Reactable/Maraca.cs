using UnityEngine;
using System.Collections;

public class Maraca : Reactable {
	public CactusFace cactus;
	
	void OnPressDown(InputInfo input) {
		cactus.Dance();
	}
}
