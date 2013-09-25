using UnityEngine;
using System.Collections;

//http://technology.blurst.com/camera-render-with-shader/


// This class implements simple ghosting type Motion Blur.
// If Extra Blur is selected, the scene will allways be a little blurred,
// as it is scaled to a smaller resolution.
// The effect works by accumulating the previous frames in an accumulation
// texture.
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/SquidTint")]
[RequireComponent(typeof(Camera))]

public class SquidTint : ImageEffectBase
{
	public RenderTexture RenderResult {
		get {
			return accumTexture;
		}
	}
	private RenderTexture accumTexture;
	private RenderTexture downSampledTexture;
	private Texture2D cleanedCheckTexture;
	
	public float _accumOrig = 0.0f;
	public OverlayRender overlayCamera;
	private float _accumTarget = 0.5f;
	
	private float _blurRate = 0.1f;
	private float _blurTime = -Mathf.Infinity;
	private float _cleanPercent = 1.0f;
	
	public delegate void OnTintCleaned();
	private OnTintCleaned onTintCleaned;
	
	private bool _blur = false;
	private Logic _logic;
	public float cleanSpeed = 0.5f;
	
	private Promise _pendentCleanCheck;
	
	public bool Finished {
		get {
			return 	_cleanPercent > 0.9f;
		}
	}
	
	override protected void Start()
	{
		_logic = Logic.instance;
	}
	
	override protected void OnDisable()
	{
		DestroyImmediate(accumTexture);
		DestroyImmediate(cleanedCheckTexture);
		DestroyImmediate(downSampledTexture);
		base.OnDisable();
	}
	
	void Update() {
		float target = 0.0f;
		_blur = false;
		
		if (_logic.IsRainy()) {
			target = _accumTarget;
			_blur = true;
		}
		
		_accumOrig = Mathf.MoveTowards(_accumOrig, target, Time.deltaTime*cleanSpeed);
		if (_blurTime > 0) {
			_blurTime -= Time.deltaTime;	
		}
	}
	
	public void Tint(OnTintCleaned onTintCleaned) {
		this.onTintCleaned = onTintCleaned;
		overlayCamera.camera.enabled = enabled;
		camera.enabled = true;
		_blurTime = _blurRate;
		_accumOrig = 0.0f;
		_cleanPercent = 0.0f;
		
		CheckCleaned();
	}
	
	void CheckCleaned() {
		_pendentCleanCheck = _.Wait(2.0f).Done(CheckCleaned);
		
		if (cleanedCheckTexture != null) {
			RenderTexture buffer = RenderTexture.GetTemporary(cleanedCheckTexture.width, cleanedCheckTexture.height, 0);
			Graphics.Blit(downSampledTexture, buffer);
			
			RenderTexture.active = buffer;
			cleanedCheckTexture.ReadPixels(new Rect(0, 0, cleanedCheckTexture.width, cleanedCheckTexture.height), 0, 0);
			cleanedCheckTexture.Apply();
			
			RenderTexture.ReleaseTemporary(buffer);
			
			Color[] colors = cleanedCheckTexture.GetPixels();
			int nColors = 0;
			for (int i = 0; i < colors.Length; ++i) {
				Color c = colors[i];
				if (c.r > 0.5f) {
					nColors++;	
				}
			}
			_cleanPercent = ((float)nColors)/ colors.Length;
		}
		
		if (Finished) {
			_pendentCleanCheck.Cancel();
			if (onTintCleaned != null) {
				onTintCleaned();
			}
		}
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height) {
			// We compute accumTexture
			DestroyImmediate(accumTexture);
			accumTexture = new RenderTexture(source.width, source.height, 0);
			accumTexture.hideFlags = HideFlags.HideAndDontSave;
			RenderTexture.active = accumTexture;
			GL.Clear(false, true, Color.white);
			
			// Downsampled version to create a blur effect
			DestroyImmediate(downSampledTexture);
			downSampledTexture = new RenderTexture(source.width/2, source.height/2, 0);
			downSampledTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit(null, downSampledTexture);
			
			// reduced version to check if it has been cleaned
			DestroyImmediate(cleanedCheckTexture);
			cleanedCheckTexture = new Texture2D(downSampledTexture.width/32, downSampledTexture.height/32);
			cleanedCheckTexture.hideFlags = HideFlags.HideAndDontSave;
		}
		
		material.SetFloat("_AccumOrig", _accumOrig);
		Graphics.Blit (source, accumTexture, material, 0);
		
		if (_blurTime < 0 && _blur) {
			Graphics.Blit(accumTexture, downSampledTexture);
			Graphics.Blit(downSampledTexture, accumTexture);
			_blurTime = _blurRate;
		}
	}
	
	
}
