using UnityEngine;
using System.Collections;

public class TPSkinnedMeshTexture : TPMeshTexture {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	protected override void Awake() {
		base.Awake();
		if(TPVars.isFreeVersion) {
			Debug.LogWarning("TPSkinnedMeshTexture is not supported by free version");
			return;
		}
	}
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	

	//--------------------------------------
	// GET / SET
	//--------------------------------------



	public override Mesh original {
		get {
			if(originalMesh == null) {
				originalMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
			}

			return originalMesh;
		}
	}

	public override Mesh sharedMesh {
		get {
			return GetComponent<SkinnedMeshRenderer>().sharedMesh;
		}

		set {
			GetComponent<SkinnedMeshRenderer>().sharedMesh = value;
		}
	}
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------

	public override void applayUV() {
		if(!TPVars.isFreeVersion) {
			base.applayUV();
		}
	}


	//--------------------------------------
	// DESTROY
	//--------------------------------------
}
