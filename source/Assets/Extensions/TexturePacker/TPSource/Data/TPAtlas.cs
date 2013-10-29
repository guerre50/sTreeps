////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPAtlas
{

	private IDictionary<string, TPAtlasTexture> _frames =  new Dictionary<string, TPAtlasTexture>();
	private IDictionary<string, Texture2D> _unityTextures =  new Dictionary<string, Texture2D>();
	private Texture _atlas;
	private string _name;
	
	private float _height;
	private float _width;
	
	private TPMeta _meta;
	
	
	private string[] supportedExtensions = new string[] { "", ".png", ".jpg", ".tga" };
	
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public TPAtlas(string json, Texture atlas, string name) {
		
		_atlas  = atlas;
		_name   = name;
		
		IDictionary JSON =  TPMiniJSON.Json.Deserialize(json) as IDictionary;
		IDictionary frames = JSON["frames"] as IDictionary;
		
		IDictionary meta = JSON["meta"] as IDictionary;
		IDictionary size = meta["size"] as IDictionary;
		
		_height = System.Convert.ToSingle(size["h"]);
		_width  = System.Convert.ToSingle(size["w"]);

		_meta =  new TPMeta();
		_meta.atlasSize =  new Size2D(_width, _height);
		_meta.imageSize = new Size2D(atlas.width, atlas.height);
		_meta.scale = System.Convert.ToSingle(meta["scale"]);
		_meta.format = System.Convert.ToString(meta["format"]);

			
		TPAtlasTexture f;
		foreach(string cKey in frames.Keys) {
			f = new TPAtlasTexture(cKey, frames[cKey] as IDictionary<string, System.Object>, this);
			_frames.Add(cKey, f);
		}

		
	}
	
	public void draw(Rect rect, string textureName) {
		GUI.DrawTextureWithTexCoords(rect, _atlas, _frames[textureName].coords);
	}
	
	public void unload() {
		_atlas = null;
		_frames.Clear();
		_unityTextures.Clear();
	}
	
	
	public void generateUnityTexturesFromAtlas() {
		foreach (KeyValuePair<string, TPAtlasTexture> item in _frames)  {
			Texture2D tx = item.Value.generateTexture();
			string name = StringUtil.cutFileExtention(item.Value.name);
			_unityTextures.Add(name, tx);
		}

		_atlas = null;
		_frames.Clear();
	}
	
	public TPAtlasTexture getTexture(string textureName) {
		TPAtlasTexture tx = findTexture(textureName);
		
		if(tx == null) {
			Debug.LogWarning("Texture " +  textureName + " not found in " + _name + " atlas");
		} 
		
		return tx;
	}
	
	public TPAtlasTexture getPngTexture(string textureName) {
		return getTexture(textureName + ".png");
	}
	
	public TPAtlasTexture getJpgTexture(string textureName) {
		return getTexture(textureName + ".jpg");
	}
	
	public Texture2D getUnityTexture(string textureName) {
		
		if(_unityTextures.ContainsKey(textureName)) {
			return _unityTextures[textureName];
		}
		
		TPAtlasTexture tx = findTexture(textureName);
			
		if(tx ==  null) {
			Debug.LogWarning("Texture " +  textureName + " not found in " + _name + " atlas");
			return null;
		}
		
		_unityTextures.Add(textureName,  tx.generateTexture());
		
		return _unityTextures[textureName];
	}
	
	public bool hasTexture(string textureName) {
		if(findTexture(textureName) == null) {
			return false;
		} else {
			return true;
		}
	}
	
	public TPAtlasTexture findTexture(string textureName) {
		string name;
		TPAtlasTexture tx;
		
		foreach(string ext  in supportedExtensions) {
			name = textureName + ext;
			tx = getTextureNoWarnings(name);
			if(tx != null) {
				return tx;
			}
		}
		
		return null;
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public Texture texture {
		get {
			return _atlas;
		}
	}
	
	public string name {
		get {
			return _name;
		}
	}
	
	
	public float height {
		get {
			return _height;
		}
	}
	
	public float width {
		get {
			return _width;
		}
	}
	
	public TPMeta meta {
		get {
			return _meta;
		}
	}

	public TPAtlasTexture[] frames {
		get {

			TPAtlasTexture[] f = new TPAtlasTexture[_frames.Count];
			int index = 0;
			foreach(TPAtlasTexture texFrame in _frames.Values) {
				f[index] = texFrame;
				index++;
			}

			return f;
		}
	}


	public string[] frameNames {
		get {

			string[] f = new string[_frames.Count];
			int index = 0;
			foreach(string frName in _frames.Keys) {
				f[index] = frName;
				index++;
			}

			return f;
		}
	}
	
	public int IndexOfFrame(string frameName) {
		
		int index = 0;
		foreach(string frName in _frames.Keys) {
			if(frameName.Equals(frName)) {
				return index;
			}
			index++;
		}

		return 0;
	}

	
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	private TPAtlasTexture getTextureNoWarnings(string textureName) {
		if(_frames.ContainsKey(textureName)) {
			return _frames[textureName];
		} else {
			return null;
		}
	}
}

