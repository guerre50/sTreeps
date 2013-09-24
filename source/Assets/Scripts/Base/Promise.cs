using UnityEngine;
using System.Collections;

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

public class _ : Singleton<_> {
	public static Promise When(Deferred deferred) {
		return new Promise(deferred);
	}
	
	private void Wait(float seconds, Deferred deferred) {
		StartCoroutine(WaitAndExecute(seconds, deferred));
	}
	
 	public static Promise Wait(float seconds) {
		Deferred deferred = new Deferred();
		
		_.instance.Wait(seconds, deferred);
		
		return new Promise(deferred);
	}
	
	private IEnumerator WaitAndExecute(float seconds, Deferred deferred) {
		yield return new WaitForSeconds(seconds);
		deferred.Resolve();
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
