using UnityEngine;
using System.Collections;

public class CameraController : Singleton<CameraController> {
	public Camera camera;
	
	void Awake () {
		camera = Camera.main;
	}
	
	public Rect WorldRect {
		get {
			Rect pixelRect = camera.pixelRect;
			Vector3 bottomLeft = ToWorld(new Vector3(pixelRect.x, pixelRect.y, camera.nearClipPlane));
			Vector3 topRight = ToWorld (new Vector3(pixelRect.xMax, pixelRect.yMax, camera.nearClipPlane));
			
			Rect worldRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
			
			return worldRect;
		}
	}
			
	private Vector3 ToWorld(Vector3 pixel) {
		return camera.ScreenToWorldPoint(pixel);	
	}
}
