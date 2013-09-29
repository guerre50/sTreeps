using UnityEngine;


public abstract class Reactable : MonoBehaviour {
	[HideInInspector]
	public bool selected;
	public Strip strip;
	private StripController _stripController;
	private bool crossingStrips = false;
	private StripController stripController {
		get {
			if (_stripController == null) {
				_stripController = StripController.instance;	
			}
			
			return _stripController;
		}
	}
	
	public int Layer {
		get {
			if (strip != null) {
				return strip.Layer;
			}
			return gameObject.layer;	
		}
	}
	
	public void Select() {
		if (crossingStrips) return;
		BroadcastSelectedValue(true);
		if (enabled) gameObject.BroadcastMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
	}
	
	public void Deselect() {
		if (crossingStrips) return;
		BroadcastSelectedValue(false);
		if (enabled) gameObject.BroadcastMessage("OnDeselect", SendMessageOptions.DontRequireReceiver);
	}
	
	public virtual void OnSelect() {
		
	}
	
	public virtual void OnDeselect() {
		
	}
	
	public void StartCrossStrip() {
		if (!crossingStrips) {
			_.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Overlay"));	
		}
		BroadcastCrossingStripsValue(true);
	}
	
	public void EndCrossStrip() {
		if (crossingStrips) {
			_.SetLayerRecursively(gameObject, strip.Layer);	
		}
		BroadcastCrossingStripsValue(false);
	}
	
	private void BroadcastCrossingStripsValue(bool value) {
		Reactable[] reactables = GetComponents<Reactable>();
		foreach (Reactable reactable in reactables) {
			reactable.crossingStrips = value;	
		}
	}
	
	private void BroadcastSelectedValue(bool value) {
		Reactable[] reactables = gameObject.GetComponents<Reactable>();
		foreach (Reactable reactable in reactables) {
			reactable.selected = value;	
		}
	}
	
	public void OnTriggerEnter(Collider collider) {
		Strip strip = stripController.GameObjectToStrip(collider.gameObject);
		
		if (strip != null) {
			this.strip = strip;
		}
	} 
}
