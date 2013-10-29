////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TFrame 
{
	public int x;
	public int y;
	public int w;
	public int h;
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public TFrame(IDictionary<string, System.Object> frameData) {
		foreach(string key in frameData.Keys) {
			switch(key) {
				case "x":
					x = System.Int32.Parse(frameData[key].ToString());
					break;
				case "y":
					y = System.Int32.Parse(frameData[key].ToString());
					break;
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

