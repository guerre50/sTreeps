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

public class TPGUIAnimation : MonoBehaviour {

	public int index = 1;
	public int fps = 30;
	public bool loop = false;

	private int _totalFrames = 0;
	private List<TPAtlasTexture> frames;
	private bool _isPlaying = false;
	private float _scale =  1;
	
	private Vector2 _pos;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static TPGUIAnimation Create() {
		return new GameObject("TPGUIAnimation").AddComponent<TPGUIAnimation>();
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	void Awake() {
		frames = new List<TPAtlasTexture>();
	}
	
	
	public void addFrame(TPAtlasTexture texture) {
		frames.Add(texture);
		_totalFrames = frames.Count;
	}
	
	
	private IEnumerator PlayCoroutine() {
		
		while(_isPlaying) {
			yield return new WaitForSeconds(1f/fps);
			index++;
			
			if(index >= _totalFrames) {
				if(loop) {
					index = 1;
				} else {
					_isPlaying = false;
					//dispatch(BaseEvent.ANIMATION_COMPLETE);
				}
			}
			
		}
	
	}
	
	
	public virtual void draw() {
		Rect drawRect = new Rect(_pos.x, _pos.y, frames[index - 1].width * _scale, frames[index - 1].height * _scale);
		frames[index - 1].draw(drawRect);
	}
	
	
	public void Play() {
		if(TPVars.isFreeVersion) {
			Debug.LogWarning("GUI Animation is not supported by free version");
			return;
		}
		
		if(!_isPlaying) {
			_isPlaying  = true;
			StartCoroutine("PlayCoroutine");
			
		}
	}
	
	
	public void Stop() {
		if(_isPlaying) {
			_isPlaying = false;
			StopCoroutine("PlayCoroutine");
		}
		
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public virtual float width {
		get {
			return frames[index - 1].width * _scale;
		}
	}
	
	public virtual float height {
		get {
			return frames[index - 1].height * _scale;
		}
	}
	
	
	public bool isPlaying {
		get {
			return _isPlaying;
		}
	}
	
	public float scale {
		get {
			return _scale;
		}
		
		set {
			_scale = value;
		}
	}
	
	public Vector2 pos {
		get {
			return _pos;
		}
		
		set {
			_pos = value;
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
