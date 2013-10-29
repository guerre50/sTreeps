////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TSourceSize
{
	public int w;
	public int h;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public TSourceSize(IDictionary<string, System.Object> frameData) {
		foreach(string key in frameData.Keys) {
			switch(key) {
				case "w":
					w = System.Int32.Parse(frameData[key].ToString());
					break;
				case "h":
					h = System.Int32.Parse(frameData[key].ToString());
					break;
			}
		}
	}
	
}

