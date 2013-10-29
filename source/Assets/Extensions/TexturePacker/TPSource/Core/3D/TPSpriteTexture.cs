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

public class TPSpriteTexture : TPSpriteAnimation {



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public override void Start() {

	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public override void OnEditorFrameChange() {



		sprite.showFrame(0);
		OnColliderSettingsChange();

		materialOpacity = opacity;

		if(useImageName) {
			if(sprite.textureName != string.Empty) {
				gameObject.name = sprite.textureName;
			}
		}
	}
	

	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	
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
