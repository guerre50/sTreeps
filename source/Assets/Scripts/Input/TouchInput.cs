using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : InputProcessor {
	private Dictionary<int, InputInfo> _inputs = new Dictionary<int, InputInfo>();
	
	public override void Process() {
		Touch[] touches = Input.touches;
		Dictionary<int, InputInfo> newInputs = new Dictionary<int, InputInfo>();
		
		foreach(Touch touch in touches) {
			InputInfo inputInfo = ProcessInput(touch, newInputs);	
			newInputs.Add (touch.fingerId, inputInfo);
			_inputs.Remove(touch.fingerId);
		}
		
		foreach(InputInfo info in _inputs.Values) {
			ProcessRelease(info);	
		}
		
		ProcessShake();
		
		_inputs.Clear();
		_inputs = newInputs;
	}
	
	private InputInfo ProcessInput(Touch touch, Dictionary<int, InputInfo> newInputs) {
		Vector3 screenPosition = touch.position;
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
		Vector3 previousScreenPosition;
		Vector3 previousWorldPosition;
		
		InputInfo inputInfo = new InputInfo();
		inputInfo.worldPosition = worldPosition;
		GameObject target = GetHitObject(inputInfo, LayerMask.NameToLayer(inputController.InteractiveLayer));
		
		if (!_inputs.TryGetValue(touch.fingerId, out inputInfo)) {
			inputInfo.pressedGameObject = target;
			previousScreenPosition = screenPosition;
			previousWorldPosition = worldPosition;
		} else {
			previousScreenPosition = inputInfo.screenPosition;
			previousWorldPosition = inputInfo.worldPosition;
		}
		
		// Set of InputInfo
		inputInfo.screenPosition = screenPosition;
		inputInfo.worldPosition = worldPosition;
		inputInfo.screenMove = screenPosition - previousScreenPosition;
		inputInfo.worldMove = worldPosition - previousWorldPosition;
		inputInfo.target = target;
		
		// We run processors
		ProcessPress(inputInfo);
		ProcessMove (inputInfo);
		
		return inputInfo;
	}
	
	private float shakeThreshold = 5.0f;
	private float lowPassFilter = 1.0f/60f; 
	private Vector3 lowPassValue = Vector3.zero;
	private float touchSensitivity = 5.0f;
	
	
	private void ProcessShake() {
		Vector3 acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilter);
		
		Vector3 deltaAcceleration = (acceleration - lowPassValue);
		if (deltaAcceleration.sqrMagnitude >= shakeThreshold) {
			Shaking = true;
		} else {
			Shaking = false;	
		}
	}
	
	private void ProcessPress(InputInfo inputInfo) {
		if (inputInfo.screenMove.magnitude <= touchSensitivity) {
			SendMessage("OnPressDown", inputInfo.pressedGameObject, inputInfo); 
		}
	}
		
	private void ProcessMove(InputInfo inputInfo) {
		if (inputInfo.screenMove.magnitude > touchSensitivity) {
			SendMessage("OnMove", inputInfo.pressedGameObject, inputInfo);	
		}
	}
	
	private void ProcessRelease(InputInfo inputInfo) {
		SendMessage("OnPressUp", inputInfo.pressedGameObject, inputInfo);
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
