////////////////////////////////////////////////////////////////////////////////
//  
// @module Affter Effect Importer
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPShaders  {

	private static string[] _importedShaders = null;
	private static Shader[] _shaders = null;

	//--------------------------------------
	// GET / SET
	//--------------------------------------



	public static Shader[] shaders {
		get {
			if(_shaders == null) {
				//_shaders = (Shader[])UnityEngine.Resources.FindObjectsOfTypeAll(typeof(Shader));

				_shaders = new Shader[5];
				_shaders[0] =   Shader.Find ("Diffuse");
				_shaders[1] =   Shader.Find ("Transparent/Diffuse");
				_shaders[2] =   Shader.Find ("Unlit/Transparent");
				_shaders[3] =   Shader.Find ("Mobile/Diffuse");
				_shaders[4] =   Shader.Find ("Particles/Additive");
			}

			return _shaders;
		}

	}

	public static string[] importedShaders {
		get {
			if(_importedShaders == null) {

				List<string> names = new List<string> ();
				foreach(Shader sh in shaders) {
					if(sh.name != "" && !sh.name.StartsWith("__")) {
						names.Add (sh.name);
					}
				}


				_importedShaders = new string[names.Count];
				int index = 0;
				foreach(string sh in names) {
					_importedShaders [index] = sh;
					index++;
				}

			}

			return _importedShaders;
		}
	}

}
