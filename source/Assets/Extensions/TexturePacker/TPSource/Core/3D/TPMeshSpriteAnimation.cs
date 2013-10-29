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

public class TPMeshSpriteAnimation : TPBaseAnimation, TPFrameHolder {
	
	private TPMeshTexture _meshTexture = null;
	public List<TPFameInfo> frames =  new List<TPFameInfo>();

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void addFrame(TPFameInfo frame) {
		frames.Add(frame);
	}
	
	
	public override void ShowFrame(int index) {
		if(frames.Count == 0) {
			renderer.material = null;
			return;
		}
		
		TPFameInfo frame = frames[index];
		
		
		meshTexture.atlas = frame.atlasPath;
		meshTexture.texture = frame.textureName;
		
		renderer.material = TPUtils.GetAtlasMaterial(frame.atlasPath);
		
		meshTexture.applayUV();
	}
	
	public override void ClearFrames() {
		frames = new List<TPFameInfo>();
	}
	
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	
	public override int lastFrameIndex {
		get {
			return frames.Count - 1;
		}
	}
	
	public TPMeshTexture meshTexture {
		get  {
			if(_meshTexture == null) {
				_meshTexture = GetComponent<TPMeshTexture>();
				if(_meshTexture == null) {
					_meshTexture = gameObject.AddComponent<TPMeshTexture>();
				}
			}
			
			return _meshTexture;
		}
	}

	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public override void OnEditorFrameChange() {
		ShowFrame(currentFrame);
	}


	public override void OnColliderSettingsChange() {

	}
	

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
