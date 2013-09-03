using UnityEngine;
using System.Collections;

public class Promise {
	private Deferred _deferred;

	public Promise(Deferred deferred) {
		_deferred = deferred;
	}

	public void Done(Callback result) {
		_deferred.Callback = result;
	}
	
};

public static class _ {
	public static Promise When(Deferred deferred) {
		return new Promise(deferred);
	}
};

public delegate void Callback();

public class Deferred {
	private Callback _callback;
	private bool resolved = false;

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
		if (_callback != null) {
			_callback();
		}
	}
};
