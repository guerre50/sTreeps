using UnityEngine;


public abstract class Reactable : MonoBehaviour {
	protected bool selected;
	
	public void Select() {
		selected = true;
	}
	
	public void Deselect() {
		selected = false;
	}
}
