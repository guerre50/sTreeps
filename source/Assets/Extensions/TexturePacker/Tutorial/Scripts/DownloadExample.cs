////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class DownloadExample : MonoBehaviour {

	public GameObject atlasObject;

	private static string   path =  "file://" + Application.dataPath + "/StreamingAssets/";

	private int loadProgress = 0;

	private Texture atlasImage;
	private string atlasData;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Start() {


		// Forming imag download url
		string imageUrl = path + "example.png";
		WWW www1  = new WWW (imageUrl);


		// Starting image download
		StartCoroutine(StartImageCoroutine(www1));



		// Forming data download url
		string dataUrl = path + "example_data.txt";
		WWW www2  = new WWW (dataUrl);


		// Starting data download
		StartCoroutine(StartDataCoroutine(www2));


	}


	IEnumerator StartImageCoroutine(WWW www) {
		yield return www;

		// check for errors
		if (www.error == null) {

			atlasImage = www.texture;
			OnLoadProgress ();
		} else {
			Debug.LogError("WWW Error: "+ www.error);
		}    
	}


	IEnumerator StartDataCoroutine(WWW www) {
		yield return www;

		// check for errors
		if (www.error == null) {
			atlasData = www.text;
			OnLoadProgress ();

		} else {
			Debug.LogError("WWW Error: "+ www.error);
		}    
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private void OnLoadProgress () {
		loadProgress++;

		if(loadProgress == 2) {
			//image and atlas data downloaded
			//registrating new atlas
			TPackManager.load (atlasData, atlasImage, "NewDownloadedAtlas");


			//applaying downloaded image to material
			atlasObject.renderer.material.mainTexture = atlasImage;


			//creating mesh texture component
			TPMeshTextureNoHelperEx meshTexture = atlasObject.AddComponent<TPMeshTextureNoHelperEx> ();

			//applyong propertis and updating view
			meshTexture.atlas = "NewDownloadedAtlas";
			meshTexture.texture = "play";
			meshTexture.UpdateView ();

		}
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
