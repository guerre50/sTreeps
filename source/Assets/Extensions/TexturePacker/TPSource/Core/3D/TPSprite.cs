////////////////////////////////////////////////////////////////////////////////
//
//  CRYSTAL CLEAR SOFTWARE
//  Copyright 2012 Crystal Clear Software. http://ccsoft.ru 
// All Rights Reserved. "$(ProjectName)" Project
// @author Osipov Stanislav lacost.20@gmail.com
// 
//
//  NOTICE: Crystal Soft does not allow to use, modify, or distribute this file
//  for any purpose
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPSprite : MonoBehaviour, TPFrameHolder {
	
	private TPMeshTexture _meshTexture;
	private TPBaseAnimation _anim;

	private string _textureName = string.Empty;
	
	public List<TPFameInfo> frames =  new List<TPFameInfo>();

	public float opacity = 1f;
	
	public Color materialColor = Color.white;

	public bool useSeparateMaterial = false;
	public int custom_shader = 2;

	private Dictionary<string, Material> materials =  new Dictionary<string, Material>();

	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public virtual void addFrame(TPFameInfo frame) {
		frames.Add(frame);
	}
	

	public void showFrame(int index) {
		
		if(frames.Count == 0) {
			renderer.material = null;
			return;
		}
		//index--;
		TPFameInfo frame = frames[index];
		
		
		meshTexture.atlas = frame.atlasPath;
		meshTexture.texture = frame.textureName;

		renderer.material = GetMaterial(frame.atlasPath);
		
		meshTexture.applayUV();
		
		TPAtlasTexture texture = GetAtlasTExture(index);
		Vector3 size =  new Vector3();
		
		size.x = texture.spriteSourceSize.w;
		size.y = texture.spriteSourceSize.h;
		size.z = size.x;
		
		transform.localScale = size / texture.scale;
		
		Vector3 pos =  new Vector3();
		pos.x = texture.spriteSourceSize.x  - texture.sourceSize.w * anim.pivotCenterX;
		pos.y = -texture.spriteSourceSize.y + texture.sourceSize.h * anim.pivotCenterY;
		pos.z = 0;
		
		transform.localPosition = pos / texture.scale;

		if(meshTexture.originalMesh.name.Equals("TPPlaneCentred")) {
			Vector3 p = transform.localPosition;
			p.y -= transform.localScale.y / 2f;
			p.x += transform.localScale.x / 2f;
			transform.localPosition = p;
		}

		if(index == 0) {
			_textureName = texture.nameNoExtention;
		}

		if(opacity != 1f) {
			materialColor.a = opacity;
			color = materialColor;
		}

	}

	public void UpdateMaterialColor() {
		materialColor = color;
	}

	private Material GetMaterial(string atlasPath) {
		if(anim.useSeparateMaterial) {
			if(materials.ContainsKey(atlasPath)) {
				Material m = materials[atlasPath];
				if(m.shader != GetShader()) {
					m.shader = GetShader();
				}

				return m;
			} else {
				Material m = new Material( TPUtils.GetAtlasMaterial(atlasPath));
				m.shader = GetShader();
				materials.Add(atlasPath, m);
				return m;
				
			}
		} else {
			return  TPUtils.GetAtlasMaterial(atlasPath);
		}
	}

	private Shader GetShader () {
		return TPShaders.shaders [anim.custom_shader];
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------


	
	public TPMeshTexture meshTexture {
		get {
			if(_meshTexture == null) {
				_meshTexture = GetComponent<TPMeshTexture>();
				if(_meshTexture == null) {
					_meshTexture = gameObject.AddComponent<TPMeshTexture>();
				}
				
			}
			
			return _meshTexture;
		}
		
	}
	
	public TPBaseAnimation anim {
		get {
			if(_anim == null) {
				_anim = transform.parent.GetComponent<TPBaseAnimation>();
			}
	
			
			return _anim;
		}
	}

	public string textureName {
		get  {
			return _textureName;
		}
	}


	public Color color {
		get {
			Material m = renderer.sharedMaterial;
			if(m == null) {
				return Color.white;
			}

			if(m.HasProperty("_Color")) {
				return m.color;
			} else {
				if(m.HasProperty("_TintColor")) {
					return m.GetColor ("_TintColor");
				} else {
					return Color.white;
				}

			}
		}

		set {
			if(renderer.sharedMaterial.HasProperty("_Color")) {
				renderer.sharedMaterial.color = value;
			}  else {
				if(renderer.sharedMaterial.HasProperty ("_TintColor")) {
					renderer.sharedMaterial.SetColor ("_TintColor", value);
				}

			}
		}
	}

	 
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	

	
	
	private TPAtlasTexture GetAtlasTExture(int index) {
		TPFameInfo frame = frames[index];
		return TPackManager.getAtlas(frame.atlasPath).getTexture(frame.textureName);
	}
	
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
