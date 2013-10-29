////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPAtlasTexture
{
	public string name;
	public string nameNoExtention;
	private TFrame _frame;
	private TSourceSize _sourceSize;
	private TFrame _spriteSourceSize;
	
	private Rect _coords =  new Rect();
	private TPAtlas atlas;
	
	private bool _trimmed = false;
	private bool _rotated = false;
	
	private float _width;
	private float _height;

	private float _scale;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public TPAtlasTexture(string _name,  IDictionary<string, System.Object> frameData, TPAtlas _atlas) {
		name = _name;

		if(name.Length > 4 && name.Contains(".")) {
			nameNoExtention = name.Substring (0, name.Length - 4);
		}
				
		foreach(string cKey in frameData.Keys) {
			switch(cKey) {
				case "frame": 
					_frame =  new TFrame(frameData[cKey] as IDictionary<string, System.Object>);
					break;
				case "sourceSize": 
					_sourceSize =  new TSourceSize(frameData[cKey] as IDictionary<string, System.Object>);
					break;
				case "spriteSourceSize": 
					_spriteSourceSize =  new TFrame(frameData[cKey] as IDictionary<string, System.Object>);
					break;
				case "trimmed":
					_trimmed = System.Boolean.Parse(frameData[cKey].ToString());
					break;
				case "rotated":
					_rotated = System.Boolean.Parse(frameData[cKey].ToString());
					break;
				
			}
			
		 }

		_scale = _atlas.meta.scale;;

		_width = (float)_sourceSize.w / _scale;
		_height = (float) _sourceSize.h / _scale;
		
		atlas = _atlas;
		calculateVars();
	}
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
	public Texture2D generateTexture() {
		Texture2D atlastexture = atlas.texture as Texture2D;
		Texture2D tx =  new Texture2D(_sourceSize.w, _sourceSize.h);
		
		
		if(!_rotated) {
			
			for(int x  = 0; x < _spriteSourceSize.x; x++) {
				for(int y = 0; y < _sourceSize.h; y++) {
					tx.SetPixel(x, _sourceSize.h - y, new Color(0, 0, 0, 0));
				}
			}
			
			for(int y  = 0; y < _spriteSourceSize.y; y++) {
				for(int x = 0; x < _sourceSize.w; x++) {
					tx.SetPixel(x, _sourceSize.h - y, new Color(0, 0, 0, 0));
				}
			}
			
			
			for(int x = _spriteSourceSize.x; x < _sourceSize.w; x++) {
				for(int y = _spriteSourceSize.y; y < _sourceSize.h; y++) {
					int cx = x + _frame.x - _spriteSourceSize.x;
					int cy = atlastexture.height - (_frame.y + y - _spriteSourceSize.y);
					
					if(!_trimmed) {
						tx.SetPixel(x, _sourceSize.h - y, atlastexture.GetPixel(cx, cy));
					} else {
						Color pixel;
						if( x >= _spriteSourceSize.x + _spriteSourceSize.w || y >= _spriteSourceSize.y + _spriteSourceSize.h) {
							pixel =  new Color(0, 0, 0, 0);
						} else {
							pixel = atlastexture.GetPixel(cx, cy);
						}
						
						tx.SetPixel(x, _sourceSize.h - y, pixel);
					}
				}
			} 
		} else {
			
			for(int x  = 0; x < _spriteSourceSize.x; x++) {
				for(int y = 0; y < _sourceSize.h; y++) {
					tx.SetPixel(x, _sourceSize.h - y, new Color(0, 0, 0, 0));
				}
			}
			
			
			for(int y  = 0; y < _spriteSourceSize.y; y++) {
				for(int x = 0; x < _sourceSize.w; x++) {
					tx.SetPixel(x,  y, new Color(0, 0, 0, 0));
				}
			} 
			
			for(int x = _spriteSourceSize.x; x < _sourceSize.w; x++) {
				for(int y = _spriteSourceSize.y; y < _sourceSize.h; y++) {
					int cx = y - _spriteSourceSize.y + _frame.x;
					int cy = atlastexture.height - (_frame.y + x - _spriteSourceSize.x);
					
					if(!_trimmed) {
						tx.SetPixel(x, y, atlastexture.GetPixel(cx, cy));
					} else {
						Color pixel;
						if( x >= _spriteSourceSize.x + _spriteSourceSize.w || y >= _spriteSourceSize.y + _spriteSourceSize.h) {
							pixel =  new Color(0, 0, 0, 0);
						} else {
							pixel = atlastexture.GetPixel(cx, cy);
						}
						
						tx.SetPixel(x, y, pixel);
					}
					
					
				}
			}  
		}
		
		
		
		
		tx.Apply();
		return tx;
		
	}
	
	public void calculateVars() {
		if(_rotated) {
			_coords.width  = (1f / (float) atlas.width) * (float) _frame.h;
			_coords.height = (1f / (float) atlas.height) * (float) _frame.w;
		} else {
			_coords.width  = (1f / (float) atlas.width) * (float) _frame.w;
			_coords.height = (1f / (float) atlas.height) * (float) _frame.h;
		}
		
		_coords.x = (1f / (float) atlas.width) * (float) _frame.x;
		_coords.y = 1f - (1f / (float) atlas.height) * (float) _frame.y - _coords.height;
	}
	
	
	public void  draw(float x, float y, float w, float h)  {
		draw(new Rect(x, y, w, h));
	}

	public void draw(Rect rect) {
		draw (rect, false);
	}
	public void draw(Rect rect, bool ignoreRotation) {
				
		if(atlas.texture == null) {
			return;
		}
		
		if(_trimmed) {
			float scale = rect.width / _sourceSize.w;
			rect.x += _spriteSourceSize.x * scale;
			rect.y += _spriteSourceSize.y * scale;
			rect.width = _spriteSourceSize.w * scale;
			rect.height = _spriteSourceSize.h * scale;

		}
		
		if(_rotated) {
			float w;
			w = rect.width;
			rect.width = rect.height;
			rect.height = w;
			rect.x += rect.height / 2 - rect.width / 2;
			rect.y += rect.width  / 2 - rect.height / 2;

			Matrix4x4 matrixBackup = GUI.matrix;
			if(!ignoreRotation) {
				GUIUtility.RotateAroundPivot(-90, new Vector2(rect.center.x, rect.center.y));
			}
	        
			
			GUI.DrawTextureWithTexCoords(rect, atlas.texture, _coords);
	        GUI.matrix = matrixBackup; 
			

			
		}  else {
			GUI.DrawTextureWithTexCoords(rect, atlas.texture, _coords);
		} 
		
		
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public TFrame frame {
		get {
			return _frame;
		}
	}

	public float scale {
		get {
			return _scale;
		}
	}
	
	
	public Rect coords {
		get {
			return _coords;
		}
	}
	
	public float width {
		get {
			return _width;
		}
	} 
	
	public float height {
		get {
			return _height;
		}
	}
	
	public bool isRotated {
		get {
			return _rotated;
		}
	}
	
	public bool isTrimmed {
		get {
			return _trimmed;
		}
	}
	
	public TSourceSize sourceSize {
		get {
			return _sourceSize;
		}
	}
	
	public TFrame spriteSourceSize {
		get {
			return _spriteSourceSize;
		}
	}

	
}

