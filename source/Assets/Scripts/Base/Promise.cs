using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Promise {
	private Deferred _deferred;
	
	public Promise(Deferred deferred) {
		_deferred = deferred;
	}

	public Promise Done(Callback result) {
		_deferred.Callback = result;
		
		return this;
	}
	
	public Promise Cancel(Callback callback = null) {
		_deferred.Cancel(callback);
		
		return this;
	}
};

public class WaitAndExecuteItem {
	private float _time;
	private Deferred _deferred;
	
	public WaitAndExecuteItem(float time, Deferred deferred) {
		_time = time;
		_deferred = deferred;
	}

	public bool UpdateTime(float deltaTime) {
		_time -= deltaTime;
		
		if (_time < 0) {
			_deferred.Resolve();
			return true;
		}
		
		return false;
	}	
};

public delegate void EventHandler(object sender, System.EventArgs e);

public class _ : Singleton<_> {
	private List<WaitAndExecuteItem> _waitAndExecutePool = new List<WaitAndExecuteItem>();
	private List<WaitAndExecuteItem> _removePool = new List<WaitAndExecuteItem>();
	private static Dictionary<string, EventHandler> _eventSystem = new Dictionary<string, EventHandler>();
	
	
	public static Promise When(Deferred deferred) {
		return new Promise(deferred);
	}
	
 	public static Promise Wait(float seconds) {
		Deferred deferred = new Deferred();
		
		_.instance.WaitAndExecute(seconds, deferred);
		
		return new Promise(deferred);
	}
	
	public static void SetLayerRecursively(GameObject gameobject, int layer) {
		gameobject.layer = layer;
		
		foreach(Transform transform in gameobject.transform) {
			_.SetLayerRecursively(transform.gameObject, layer);	
		}
	}
	
	void LateUpdate() {
		float deltaTime = Time.deltaTime;
		_removePool.Clear();
		
		for(int i = 0; i < _waitAndExecutePool.Count; ++i) {
			WaitAndExecuteItem waitAndExecute = _waitAndExecutePool[i];
			
			if (waitAndExecute.UpdateTime(deltaTime)) {
				_removePool.Add(waitAndExecute);
			}
		}
		
		foreach(WaitAndExecuteItem waitAndExecute in _removePool) {
			_waitAndExecutePool.Remove(waitAndExecute);
		}
	}
	
	public static void On(string id, EventHandler callback) {
		if (!_eventSystem.ContainsKey(id)) {
			EventHandler eventHandler = delegate {};
			_eventSystem.Add (id, eventHandler);
		}
		_eventSystem[id] += callback;
	}
	
	public static void Off(string id, EventHandler callback) {
		EventHandler eventHandler;
		
		if (_eventSystem.TryGetValue(id, out eventHandler)) {
			eventHandler -= callback;
		}
	}
	
	public static void Trigger(string id, object sender, System.EventArgs events = null) {
		EventHandler eventHandler;
		
		if (_eventSystem.TryGetValue(id, out eventHandler)) {
			eventHandler(sender, events);
		}
	}
	
	private void WaitAndExecute(float seconds, Deferred deferred) {
		_waitAndExecutePool.Add(new WaitAndExecuteItem(seconds, deferred));
	}
};

public delegate void Callback();

public class Deferred {
	private Callback _callback;
	private bool resolved = false;
	private bool canceled = false;
	
	public Callback Callback {
		set {
			_callback = value;
			if (resolved) {
				Resolve();
			}
		}
	}

	public void Resolve() {
		if (_callback != null && !canceled) {
			_callback();
		}
	}
	
	public void Cancel(Callback callback = null) {
		if (!canceled && callback != null) {
			callback();
		}
		canceled = true;	
	}
};
