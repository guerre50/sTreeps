////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TPMeshAnimation : BaseTPConmponents {
	
	
	public float FPS = 25;
	public bool playOnStart = true;
	public bool loop = true;
	public string[] frames;
	
	protected bool _isPlaying = false;
	protected TPAtlasTexture[] _viewFrames;
	protected int _index;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	protected override void Awake() {
		
		base.Awake();
		
		if(Application.isPlaying) {
			initUVValues();
		}
	}
	
	
	protected virtual void Start() {
		if(Application.isPlaying) {
			initFrames();
		
			_index = 1;
			
			if(_viewFrames.Length > 0) {
				showFrame(1);
			}
			
			
			if(playOnStart) {
				StartAnimation();
			}
		}
		
	}
	
	protected virtual void Update() {
		if(!Application.isPlaying) {
			applayUV();
		}
	}
	
	
	protected virtual void OnDrawGizmos() {
		if(!Application.isPlaying) {
			applayUV();
		}
	}
	
	
	protected virtual void applayUV() {
		if(frames.Length > 0) {
			TPAtlasTexture tx = getTexture(frames[0]);
			showFrame(tx);
		}
	}
	
	protected virtual void initFrames() {
		_viewFrames = new TPAtlasTexture[frames.Length];
		
		int index = 0;
		foreach(string texture in frames) {
			_viewFrames[index] = getTexture(texture);
			index++;
		}
	}
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
	public void Play() {
		StartAnimation();
	}
	
	public void StartAnimation() {
		if(_isPlaying) {
			return;
		}
		
		_isPlaying = true;
		StartCoroutine("AnimationSequence");

	}
	
	
	public void Stop() {
		StartAnimation();
	}
	
	public void GoToAndStop(int index) {
		showFrame(index);
		Stop();
	}
	
	public void GoToAndPlay(int index) {
		showFrame(index);
		Play();
	}
	
	
	public void StopAnimation() {
		if(!_isPlaying) {
			return;
		}
		
		_isPlaying = false;
		
		StopCoroutine("AnimationSequence");
		
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public bool isPlaying {
		get {
			return _isPlaying;
		}
	}
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	private IEnumerator AnimationSequence() {
		while(true) {
			showFrame(_index);
			if(_index == _viewFrames.Length) {
				if(loop) {
					_index = 0;
				} else {
					StopAnimation();
				}
			}
			
			_index++;
			yield return new WaitForSeconds(1f / FPS);
		}
	}
	
	private void showFrame(int index) {
		if(index > _viewFrames.Length) {
			return;
		}
		
		index--;
		showFrame(_viewFrames[index]);
		
	}
	
	//--------------------------------------
	// DESTROY
	//--------------------------------------
}
