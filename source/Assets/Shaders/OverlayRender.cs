using UnityEngine;
using System.Collections;

// http://answers.unity3d.com/questions/32735/modifying-my-shader-to-overlay-rather-than-blend.html

public class OverlayRender : ImageEffectBase {
	public SquidTint squidTint;
	
	override protected void Start()
	{
		if(!SystemInfo.supportsRenderTextures)
		{
			enabled = false;
			return;
		}
		base.Start();
	}
	
	override protected void OnDisable()
	{
		base.OnDisable();
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit (squidTint.RenderResult, destination, material);
	}
}
