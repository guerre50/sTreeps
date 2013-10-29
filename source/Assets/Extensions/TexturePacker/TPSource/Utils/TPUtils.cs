using UnityEngine;
using System.Collections;

public static class TPUtils  {
	
	public static Material GetAtlasMaterial(string atlasPath) {
		return Resources.Load(atlasPath + "Material") as Material;
	}
	
	
	
	
}

