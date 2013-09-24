using UnityEngine;
using System.Collections;

public class InputController : Singleton<InputController> {
	InputProcessor[] _inputs;

	void Awake () {
		_inputs = new InputProcessor[1];
		_inputs[0] = (new MouseInput()) as InputProcessor;
	}
	
	void LateUpdate () {
		ExecuteInputs();
	}

	void ExecuteInputs() {
		foreach (InputProcessor processor in _inputs) {
			processor.Process();
		}
	}
	
	public bool HitLayerTagSendMessage(InputInfo inputInfo, int layer, string tag, string message) {
		bool result = false;
		foreach (InputProcessor processor in _inputs) {
			result = result || processor.HitLayerTagSendMessage(inputInfo, layer, tag, message);
		}
		
		return result;
	}
}
