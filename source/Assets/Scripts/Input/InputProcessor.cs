using UnityEngine;
using System.Collections;

public struct InputInfo {
	public Vector3 screenPosition;
	public Vector3 worldPosition;
	public GameObject target;
	public Vector3 screenMove;
	public Vector3 worldMove;
	public GameObject pressedGameObject;
}

public abstract class InputProcessor {
	public abstract void Process();
	public bool Shaking = false;
	
	public bool HitLayerTagSendMessage(InputInfo inputInfo, int layer, string tag, string message) {
		GameObject target = GetHitObject(inputInfo, layer);
		
		if (target != null && tag != null && target.tag == tag) {
			return SendMessage(message, target, inputInfo);
		} else {
			return false;	
		}
	}
	
	protected bool SendMessage(string message, GameObject target, InputInfo inputInfo) {
		if (target) {
			target.SendMessage(message, (object)inputInfo, SendMessageOptions.DontRequireReceiver);
		}
		
		return target != null;
	}
	
	protected virtual GameObject GetHitObject(InputInfo inputInfo, int layer) {
		return null;
	}
	 
};
