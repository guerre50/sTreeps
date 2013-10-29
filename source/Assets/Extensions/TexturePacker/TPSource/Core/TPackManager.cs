////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.20@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPackManager {
	
	private static IDictionary<string, TPAtlas> atlases =  new Dictionary<string, TPAtlas>();
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static void load(string atlas) {
		try {
			
			if(atlases.ContainsKey(atlas)) {
				return;
			}
			
			TextAsset atlas_data      = Resources.Load(atlas + "_data") as TextAsset;
			Texture   atlas_texture   = Resources.Load(atlas) as Texture;
			atlases.Add(atlas, new TPAtlas(atlas_data.text, atlas_texture, atlas));
		} catch(System.Exception ex) {
			Debug.LogError("Atlas Can not be loaded form: " + atlas);
			Debug.LogError(ex.Message);
		}
		
	}


	public static void load(string data, Texture textute, string name) {
		try {

			Texture   atlas_texture   = textute;
			atlases.Add(name, new TPAtlas(data, atlas_texture, name));
		} catch(System.Exception ex) {
			Debug.LogError("Atlas Can not be loaded form: " + name);
			Debug.LogError(ex.Message);
		}

	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public static void unload(string atlas) {
		if(atlases.ContainsKey(atlas)) {
			atlases[atlas].unload();
			atlases.Remove(atlas);
		}
	}

	public static void clear() {
		atlases.Clear();
	}
	
	
	
	public static TPAtlas getAtlas(string atlas) {
		if(atlas == string.Empty || atlas == null) {
			return null;
		}
		
		if(atlases.ContainsKey(atlas)) {
			return atlases[atlas];
		} else {
			if(Resources.Load(atlas + "_data") == null || Resources.Load(atlas) == null) {
				return null;
			}
			
			load(atlas);
			return atlases[atlas];
		}
	}
	
	public static void draw(Rect rect, string atlas, string textureName) {
		if(atlases.ContainsKey(atlas)) {
			atlases[atlas].draw(rect, textureName);
		} else {
			Debug.Log("Can not draw texture " + textureName + " atlas " + atlas + " not found");
		}
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public static List<string> loadedAtlases {
		get {
			List<string> curentlyLoaded = new List<string>();
			foreach(string atlas in atlases.Keys) {
				curentlyLoaded.Add(atlas);
			}
			
			return curentlyLoaded;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
