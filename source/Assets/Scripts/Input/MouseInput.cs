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
		info.screenMove = screenPosition - _previousMouseScreenPosition;
		info.worldMove = worldPosition - _previousMouseWorldPosition;
		
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
		if (inputInfo.worldPosition != _previousMouseWorldPosition) {
			SendMessage("OnMove", _pressedGameObject, inputInfo);	
		}
	}
	
	private void ProcessHover(InputInfo inputInfo) {
		if (inputInfo.target != _hoverGameObject) {
			SendMessage("OnEnter", inputInfo.target, inputInfo);
			SendMessage("OnLeave", _hoverGameObject, inputInfo);
		}
	}
	
	override public void  HitLayerSendMessage(InputInfo inputInfo, int layer, string message) {
		GameObject target = GetHitObject(inputInfo.worldPosition, layer);
		
		SendMessage(message, target, inputInfo);
	}
	
	private void SendMessage(string message, GameObject target, InputInfo inputInfo) {
		if (target) {
			target.SendMessage(message, (object)inputInfo, SendMessageOptions.DontRequireReceiver);
		}
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
