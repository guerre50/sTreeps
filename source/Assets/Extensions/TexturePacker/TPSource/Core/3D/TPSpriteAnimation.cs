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

public class TPSpriteAnimation : TPBaseAnimation {
	
	
	public TPSprite sprite;
	
	public Color GizmosColor = Color.green;




	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public override void ShowFrame(int index) {
		currentFrame = index;
		sprite.showFrame(currentFrame);
	}
	
	public override void ClearFrames () {
		sprite.frames =  new List<TPFameInfo>();
	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	
	public float width {
		get {
			if(sprite.frames.Count > 0) {
				return TPackManager.getAtlas(sprite.frames[0].atlasPath).getTexture(sprite.frames[0].textureName).width; 
			} else {
				return 0;
			}
		}
	}


	public float height {
		get {
			if(sprite.frames.Count > 0) {
				return TPackManager.getAtlas(sprite.frames[0].atlasPath).getTexture(sprite.frames[0].textureName).height; 
			} else {
				return 0;
			}
		}
	}
	
	
	public override int lastFrameIndex {
		get {
			return sprite.frames.Count - 1;
		}
	}

	public float materialOpacity {
		get {
			return sprite.opacity;
		}

		set {
			opacity = value;
			sprite.opacity = opacity;
			sprite.UpdateMaterialColor();
		}
	} 
	

	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	public override void OnEditorFrameChange() {

		sprite.showFrame(currentFrame);

		OnColliderSettingsChange();

		if(useImageName) {
			if(sprite.textureName != string.Empty) {
				gameObject.name = sprite.textureName;
			}
		}
	}

	public override void OnColliderSettingsChange() {

		BoxCollider c = gameObject.GetComponent<BoxCollider>();
	
		if(useCollider) {
			if(c == null) {
				c = gameObject.AddComponent<BoxCollider>();
			} 
			adjustCollider();
		} else {
			if(c!= null) {
				DestroyImmediate(c);
			}
		}
	}

	private void adjustCollider() {
		BoxCollider c = gameObject.GetComponent<BoxCollider>();

		c.size = new Vector3(sprite.transform.localScale.x, sprite.transform.localScale.y, 1f);

		Vector3 center = sprite.transform.localPosition;

		if(!sprite.meshTexture.originalMesh.name.Equals("TPPlaneCentred")) {
			center.x += sprite.transform.localScale.x / 2f;
			center.y -= sprite.transform.localScale.y / 2f;
		}

		c.center = center;
	}

	
	private void  OnDrawGizmosSelected () {
		if(sprite.frames.Count > 0) {
			Gizmos.color = GizmosColor;

			Vector3 pos = Vector3.zero;
		
			pos.x += width / 2f;
			pos.y -= height / 2f;


			pos.x -= width * pivotCenterX;
			pos.y += height * pivotCenterY;



			Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, GetWorldScale());

			Gizmos.matrix = rotationMatrix; 

			Gizmos.DrawWireCube (pos, new Vector3 (width, height, 0.01f));
		} 
	}
	
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	

	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------



}