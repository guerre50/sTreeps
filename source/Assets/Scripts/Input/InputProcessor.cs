using UnityEngine;
using System.Collections;

public struct InputInfo {
	public Vector3 screenPosition;
	public Vector3 worldPosition;
	public GameObject target;
	public Vector3 screenMove;
	public Vector3 worldMove;
}

public abstract class InputProcessor {
	public abstract void HitLayerSendMessage(InputInfo info, int layer, string message);
	
	public abstract void Process();
};
