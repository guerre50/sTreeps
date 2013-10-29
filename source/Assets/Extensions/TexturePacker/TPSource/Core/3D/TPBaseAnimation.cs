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

public abstract class TPBaseAnimation : TPForceSelectedComponent {
	
	public int currentFrame = 0;
	public bool PlayOnStart = true;
	public bool Loop = true;


	public float opacity = 1f;
	
	public float pivotCenterX = 0f;
	public float pivotCenterY = 0f;
	
	
	public int frameRate = 25;
	
	protected bool _isPlaying = false;

	public bool useCollider = false;
	public bool useImageName = true;

	public bool useSeparateMaterial = false;

	
	public int custom_shader = 2;

	

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public virtual void Start() {
		if(Application.isPlaying) {

			if(PlayOnStart) {
				StartAnimation();
			}
		}
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public abstract void ShowFrame(int index);
	public abstract void OnEditorFrameChange();
	public abstract void OnColliderSettingsChange();
	public abstract void ClearFrames();


	public virtual void Refresh() {
		OnEditorFrameChange();
	}
	
	public virtual void Play() {
		StartAnimation();
	}


	
	public virtual void StartAnimation() {
		if(_isPlaying) {
			return;
		}

		if(TPVars.isFreeVersion) {
			if(frameRate > 20) {
				Debug.LogWarning("Free version has frame rate limit. 20fps max");
				return;
			}
		}
		
		_isPlaying = true;
		StartCoroutine("OnAnimationSequence");

	}
	
	
	public virtual void Stop() {
		StopAnimation();
	}
	
	public virtual void StopAnimation() {
		if(!_isPlaying) {
			return;
		}
		
		_isPlaying = false;
		
		StopCoroutine("OnAnimationSequence");
		
	}
	
	
	public virtual void GoToAndStop(int index) {
		ShowFrame(index);
		Stop();
	}
	
	public virtual void GoToAndPlay(int index) {
		ShowFrame(index);
		Play();
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public abstract int lastFrameIndex {get;} 
	
	
	public bool isPlaying {
		get {
			return _isPlaying;
		}
	}


	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	protected  virtual IEnumerator OnAnimationSequence() {
		while(true) {
			ShowFrame(currentFrame);
			if(currentFrame >= lastFrameIndex) {
				if(Loop) {
					currentFrame = 0;
				} else {
					StopAnimation();
				}
			} else {
				currentFrame++;
			}
			
			yield return new WaitForSeconds(1f / frameRate);
		}
	}
	
	
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	protected Vector3 GetWorldScale() {

        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;


        while (parent != null) {
			worldScale = Vector3.Scale(worldScale,parent.localScale);
            parent = parent.parent;
        }
        return worldScale;
	}
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
