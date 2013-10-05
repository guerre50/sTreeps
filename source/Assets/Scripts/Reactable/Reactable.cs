using UnityEngine;


public abstract class Reactable : MonoBehaviour {

	public bool selected;
	public Strip strip;
	private StripController _stripController;
	private bool crossingStrips = false;
	protected bool _onSelectFired = false;
	protected bool _onDeselectFired = false;
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
	}
	
	public void Deselect() {
		if (crossingStrips) return;
		BroadcastSelectedValue(false);
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
		bool valueChange = false;
		foreach (Reactable reactable in reactables) {
			valueChange = valueChange || reactable.selected != value;
			reactable.selected = value;	
		}
		if (valueChange) {
			foreach (Reactable reactable in reactables) {
				if (reactable.enabled ) {
					if (value) {
						reactable.OnSelect();	
					} else {
						reactable.OnDeselect();	
					}
				}
			}
		}
	}
	
	public void OnTriggerEnter(Collider collider) {
		Strip strip = stripController.GameObjectToStrip(collider.gameObject);
		
		if (strip != null) {
			this.strip = strip;
		}
	} 
}
