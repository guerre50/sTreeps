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
		InputInfo info = new InputInfo();
		info.worldPosition = worldPosition;
		
		GameObject target = GetHitObject(info, LayerMask.NameToLayer("Strips"));
		
		if (_previousMouseWorldPosition == Vector3.zero) {
			_previousMouseWorldPosition = worldPosition;
			_previousMouseScreenPosition = screenPosition;
		}
		
		// Set of InputInfo
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
		ProcessShake();
		
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
	
	private void ProcessShake() {
		
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
	
	protected override GameObject GetHitObject(InputInfo inputInfo, int layer) {
		RaycastHit hitInfo;
		GameObject target = null;
		
		if (Physics.Raycast(inputInfo.worldPosition, Camera.main.transform.forward, out hitInfo, 10000, 1 << layer)) {
			target = hitInfo.transform.gameObject;
		}
		
		return target;
	}
}
