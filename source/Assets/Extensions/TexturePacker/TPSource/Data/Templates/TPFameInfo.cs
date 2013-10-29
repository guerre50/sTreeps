using UnityEngine;
using System.Collections;


[System.Serializable]
public class TPFameInfo  {
	
	public string textureName;
	public string atlasPath;
	
	public TPFameInfo(string _atlasPath, string _textureName) {
		atlasPath = _atlasPath;
		textureName = _textureName;
	}
	
}

