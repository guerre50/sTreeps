using UnityEngine;
using System.Collections;

public class StripCamera : MonoBehaviour {
	private Camera[] _cameras;
	private float _sizeX;
	private int _layer;
	private int _pivot;
	
	public int Layer {
		get {
			return _layer;		
		}
		set {
			SetLayer(value);
		}
	}
	
	void SetLayer(int layer) {
		_layer = layer;
		
		foreach (Camera camera in _cameras) {
			SetCameraCullingMask(camera, layer);
		}
	}
	
	public void Right(int layer) {
		_cameras[_pivot].transform.localPosition = Vector3.right*_sizeX;
		SetCameraCullingMask(_cameras[_pivot], layer);
	}
	
	public void Left(int layer) {
		_cameras[_pivot].transform.localPosition = -Vector3.right*_sizeX;
		SetCameraCullingMask(_cameras[_pivot], layer);
	}
	
	public void Change() {
		int layer = _cameras[_pivot].cullingMask;
		SetCameraCullingMask(_cameras[_pivot], _cameras[1-_pivot].cullingMask);
		SetCameraCullingMask(_cameras[1 - _pivot], layer);
	}
	
	void SetCameraCullingMask(Camera camera, int layer) {
		camera.cullingMask = 1 << layer | 1 << LayerMask.NameToLayer("Weather");	
	}
	
	void Awake () {
		int cameras = transform.childCount;
		_cameras = new Camera[cameras];
		_pivot = 0;
		
		foreach (Transform t in transform) {
			_cameras[--cameras] = t.camera;
			t.camera.transparencySortMode = TransparencySortMode.Orthographic;
		}
	}
	
	public void SetViewport(Rect rect, float orthographicSize, Vector3 size) {
		_sizeX = size.x;
		float offset = -_sizeX*(_cameras.Length/2);
		
		foreach(Camera cam in _cameras) {
			cam.rect = rect;
			cam.orthographicSize = orthographicSize;
			cam.transform.Translate(offset, 0, 0);
			offset += _sizeX;
		}
	}
}
