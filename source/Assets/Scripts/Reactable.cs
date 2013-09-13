using UnityEngine;


public abstract class Reactable : MonoBehaviour {
	public void Select() {
		Debug.Log ("Select");	
	}
	
	public void Deselect() {
		Debug.Log ("Deselect");	
	}
}
