////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class TPMeshTexture : BaseTPConmponents {
	
	public string texture = string.Empty;
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	protected override void Awake() {
		base.Awake();
		applayUV();
	}
	


	protected virtual void Update() {
		
		if(!Application.isPlaying) {
			if(sharedMesh == null ) {
				applayUV();
			}
		} 
	}
	
	
	
	public virtual void applayUV() {
		TPAtlasTexture tx = getTexture(texture);
		showFrame(tx);
	}

	

	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public void UpdateView() {
		applayUV();
	}
	

	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	// DESTROY
	//--------------------------------------
	
	
}
