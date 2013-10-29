////////////////////////////////////////////////////////////////////////////////
//  
// @module TexturePcker Unity3d Util 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseTPConmponents : MonoBehaviour {
	
	public string atlas;

	//[HideInInspector]
	public Mesh originalMesh;

	
	//private MeshFilter meshFilter = null;
	//private Vector2[] initialUVS;
//	private Vector2[] scaledUVS;
	

	private bool isRotated = false;

	public bool useSharedMesh = false;

	private Vector2[] storedUVS = null;

	public static List<Mesh> storedMeshes =  new List<Mesh>();
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	protected virtual void Awake() {
		initUVValues();
	}
	
	public  void initUVValues() {
		
		if(useSharedMesh && Application.isPlaying) {
			if(storedMeshes.Contains(original) || storedMeshes.Contains(sharedMesh)) {
				sharedMesh = original;
				return;
			} else {
				storedUVS = original.uv;
				sharedMesh = original;

				return;
			}
						
		}

		if(sharedMesh != null) {
			if(sharedMesh.name.Length > 4) {
				if(getExtention(sharedMesh.name) == "_uvs") {
					DestroyImmediate(sharedMesh);
				}
			}


		}


		sharedMesh = CopyMesh(original);
		sharedMesh.name = original.name + "_uvs";
	}
	
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public void setTile(float x, float y) {



		if(sharedMesh == null ) {
			sharedMesh = original;
			initUVValues();
		}

		Vector2[] uvs = new Vector2[sharedMesh.vertices.Length]; 
		for (var i = 0 ; i < uvs.Length; i++) {
			uvs[i].x  = original.uv[i].x * x; 
			uvs[i].y  = original.uv[i].y * y; 
		} 
		sharedMesh.uv = uvs;
	}
	
	public void setOffset(float x, float y) {



		Vector2[] uvs = new Vector2[sharedMesh.vertices.Length]; 

		for (var i = 0 ; i < uvs.Length; i++) {
			uvs[i].x  = sharedMesh.uv[i].x + x;
			uvs[i].y  = sharedMesh.uv[i].y + y;
			
			if(isRotated) {
				if(!TPVars.isFreeVersion) {
					Quaternion rot = Quaternion.Euler(0 ,0, -90);
					uvs[i] = rot * uvs[i];
				} else {
					Debug.LogWarning("Texture Rotation is not supported by free version");
				}

			} 
			
		} 

		sharedMesh.uv = uvs;
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	public virtual Mesh original {
		get {
			if(originalMesh == null) {
				originalMesh = GetComponent<MeshFilter>().sharedMesh;
			}
			
			return originalMesh;
		}
	}

	public virtual Mesh sharedMesh {
		get {
			return GetComponent<MeshFilter>().sharedMesh;
		}

		set {
			GetComponent<MeshFilter>().sharedMesh = value;
		}
	}

	public TPAtlas TextureAtlas {
		get {
			TPAtlas a = TPackManager.getAtlas(atlas);
			return a;
		}
	}
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	protected void showFrame(TPAtlasTexture texture) {
		Rect frame;
		
		if(texture == null) {
			frame =  new Rect(0, 0, 1, 1);
		} else {
			
			if(texture.isRotated) {
				frame =  new Rect();
				
				frame.x = 0 - texture.coords.y - texture.coords.height;
				frame.y = texture.coords.x;
				frame.width  = texture.coords.height;
				frame.height = texture.coords.width;
				isRotated = true;
			} else {
				frame = texture.coords;
				isRotated = false;
			}
			
			
			
			
		}
	

		if(useSharedMesh && Application.isPlaying) {
			if(storedMeshes.Contains(original) || storedMeshes.Contains(sharedMesh)) {
				return;
			} else {
				storedMeshes.Add (original);
			}
		}
		
		
		
		
		setTile(frame.width,  frame.height);
		setOffset(frame.x, frame.y);
		
		
	}
	
	
	protected TPAtlasTexture getTexture(string texture) {
		

		if(tpAtlas == null) {
			return null;
		}
		
		if(tpAtlas.hasTexture(texture)) {
			return tpAtlas.getTexture(texture);
		} else {
			return null;
		}
	}
	
	protected Mesh CopyMesh(Mesh m) {
        Mesh newMesh = new Mesh();
		
		newMesh.vertices 	= m.vertices;
        newMesh.triangles 	= m.triangles;
        newMesh.uv 			= m.uv;
        newMesh.normals 	= m.normals;
        newMesh.colors 		= m.colors;
        newMesh.tangents 	= m.tangents;
		newMesh.name      	= m.name;

		newMesh.boneWeights      = m.boneWeights;
		newMesh.bindposes      	= m.bindposes;

		
        return newMesh;
    }
	
	
	private static string getExtention(string fileName) {
		return fileName.Substring(fileName.Length - 4, 4);
	}

	protected void OnDestroy() {
		if(Application.isPlaying) {
			if(useSharedMesh && storedUVS != null) {
								sharedMesh.uv = storedUVS;
								original.uv = storedUVS;
								storedMeshes.Clear ();
			}
		}
	}
	
	public TPAtlas tpAtlas {
		get {
			return TPackManager.getAtlas(atlas);
		}
	}
}

