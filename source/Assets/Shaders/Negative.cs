
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Negative")]
public class Negative : MonoBehaviour
{	
	public Shader negativeShader = null;	

	static Material m_Material = null;
	protected Material material {
		get {
			if (m_Material == null) {
				m_Material = new Material(negativeShader);
				m_Material.hideFlags = HideFlags.DontSave;
			}
			return m_Material;
		} 
	}
	
	protected void OnDisable() {
		if( m_Material ) {
			DestroyImmediate( m_Material );
		}
	}	
	
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		
		if (!negativeShader || !material.shader.isSupported) {
			enabled = false;
			return;
		}
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, material);
	}	
}
