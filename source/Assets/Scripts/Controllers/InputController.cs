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
	
	public void HitLayerSendMessage(InputInfo inputInfo, int layer, string message) {
		foreach (InputProcessor processor in _inputs) {
			processor.HitLayerSendMessage(inputInfo, layer, message);
		}
	}
}
