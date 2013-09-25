using UnityEngine;
using System.Collections;

public class CameraController : Singleton<CameraController> {
	private Camera _camera;
	
	void Awake () {
		_camera = Camera.main;
	}
	
	public Camera Camera {
		get { return _camera;}	
	}
	
	public Rect WorldRect {
		get {
			Rect pixelRect = _camera.pixelRect;
			Vector3 bottomLeft = ToWorld(new Vector3(pixelRect.x, pixelRect.y, _camera.nearClipPlane));
			Vector3 topRight = ToWorld (new Vector3(pixelRect.xMax, pixelRect.yMax, _camera.nearClipPlane));
			
			Rect worldRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
			
			return worldRect;
		}
	}
			
	private Vector3 ToWorld(Vector3 pixel) {
		return _camera.ScreenToWorldPoint(pixel);	
	}
}
