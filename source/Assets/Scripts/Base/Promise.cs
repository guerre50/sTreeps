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
	
	public Promise Cancel() {
		_deferred.Cancel();
		
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

public class _ : Singleton<_> {
	private List<WaitAndExecuteItem> _waitAndExecutePool = new List<WaitAndExecuteItem>();
	private List<WaitAndExecuteItem> _removePool = new List<WaitAndExecuteItem>();
	
	public static Promise When(Deferred deferred) {
		return new Promise(deferred);
	}
	
 	public static Promise Wait(float seconds) {
		Deferred deferred = new Deferred();
		
		_.instance.WaitAndExecute(seconds, deferred);
		
		return new Promise(deferred);
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
			if (resolved) {
				value();
			} else {
				_callback = value;
			}
		}
	}

	public void Resolve() {
		if (_callback != null && !canceled) {
			_callback();
		}
	}
	
	public void Cancel() {
		canceled = true;	
	}
};
