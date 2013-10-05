using UnityEngine;
using System.Collections;

public class InputController : Singleton<InputController> {
	InputProcessor[] _inputs;
	public bool Shaking;

	void Awake () {
		_inputs = new InputProcessor[1];


#if UNITY_ANDROID
		_inputs[0] = (new TouchInput()) as InputProcessor;
#endif
		
#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		_inputs[0] = (new MouseInput()) as InputProcessor;
#endif
		
#if UNITY_EDITOR
		_inputs[0] = (new MouseInput()) as InputProcessor;
#endif	
		
	}
	
	void LateUpdate () {
		ExecuteInputs();
	}

	void ExecuteInputs() {
		foreach (InputProcessor processor in _inputs) {
			processor.Process();
			Shaking = processor.Shaking;
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
