using UnityEngine;
using System.Collections;

public class MouseInput : InputProcessor {
	private Vector3 _previousMouseWorldPosition = Vector3.zero;
	private Vector3 _previousMouseScreenPosition = Vector3.zero;
	private GameObject _pressedGameObject;
	private GameObject _hoverGameObject;
	
	public override void Process() {
		// Getting data
		Vector3 screenPosition = Input.mousePosition;
		//Debug.Log (screenPosition);
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
		GameObject target = GetHitObject(worldPosition, 1 << LayerMask.NameToLayer("Strips"));
		
		if (_previousMouseWorldPosition == Vector3.zero) {
			_previousMouseWorldPosition = worldPosition;
			_previousMouseScreenPosition = screenPosition;
		}
		
		// Set of InputInfo
		InputInfo info = new InputInfo();
		info.screenPosition = screenPosition;
		info.worldPosition = worldPosition;
		info.target = target;
		
		if (_pressedGameObject) {
			info.screenMove = screenPosition - _previousMouseScreenPosition;
			info.worldMove = worldPosition - _previousMouseWorldPosition;
		}
		
		// We run processors
		ProcessPress(info);
		ProcessMove (info);
		ProcessHover(info);
		
		// We update parameters
		_previousMouseScreenPosition = screenPosition;
		_previousMouseWorldPosition = worldPosition;
		_hoverGameObject = target;
	}
	
	private void ProcessPress(InputInfo inputInfo) {
		bool mouseDown = Input.GetMouseButtonDown(0);
		bool mouseUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0) && false;
			
		if (mouseDown) {
			_pressedGameObject = inputInfo.target;
			SendMessage("OnPressDown", inputInfo.target, inputInfo);
		} else if (mouseUp) {
			SendMessage("OnPressUp", _pressedGameObject, inputInfo);
			_pressedGameObject = null;
		}
		
		if (mouseButton) {
			SendMessage("OnPress", inputInfo.target, inputInfo);
		}	
	}
		
	private void ProcessMove(InputInfo inputInfo) {
		if (inputInfo.screenMove.magnitude > 0) {
			SendMessage("OnMove", _pressedGameObject, inputInfo);	
		}
	}
	
	private void ProcessHover(InputInfo inputInfo) {
		if (inputInfo.target != _hoverGameObject) {
			SendMessage("OnEnter", inputInfo.target, inputInfo);
			SendMessage("OnLeave", _hoverGameObject, inputInfo);
		}
		SendMessage("OnHover", inputInfo.target, inputInfo);
	}
	
	override public bool  HitLayerTagSendMessage(InputInfo inputInfo, int layer, string tag, string message) {
		GameObject target = GetHitObject(inputInfo.worldPosition, layer);
		
		if (target != null && tag != null && target.tag == tag) {
			return SendMessage(message, target, inputInfo);
		} else {
			return false;	
		}
	}
	
	private bool SendMessage(string message, GameObject target, InputInfo inputInfo) {
		if (target) {
			target.SendMessage(message, (object)inputInfo, SendMessageOptions.DontRequireReceiver);
		}
		
		return target != null;
	}
	
	private GameObject GetHitObject(Vector3 position, int layer) {
		RaycastHit hitInfo;
		GameObject target = null;
		
		if (Physics.Raycast(position, Camera.main.transform.forward, out hitInfo, 10000, layer)) {
			target = hitInfo.transform.gameObject;
		}
		
		return target;
	}
}
