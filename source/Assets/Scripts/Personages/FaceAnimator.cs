using UnityEngine;
using System.Collections;
using Radical;

public class FaceAnimator : Reactable {
	private PersonageController _personageController;
	private AnimationState[] _animationHorizontal;
	private AnimationState[] _animationVertical;
	private PersonageType _personage;
	private SmoothVector3 _position;
	private GameObject _eyesightTarget;
	private float eyesightRadius = 50;
	protected Logic _logic;
	public Vector2 Position {
		set {
			Vector3 vector = Vector3.ClampMagnitude(new Vector3(value.x, value.y), 1);
			if (_position.Target != vector) {
				Vector3 currentPosition = _position;
				_position.Value = vector;
				_position.Duration = Mathf.Lerp(0, 1.0f, (currentPosition - vector).magnitude);
			}
		}
	}
	
	public void Init (PersonageType personage) {
		_logic = Logic.instance;
		_personage = personage;
		
		SetAnimations();
		SetListeners();
	}
	
	private void SetListeners() {
		_.On (Triggers.EyesightFollow, OnEyesightFollow);
		_.On (Triggers.EyesightUnfollow, OnEyesightUnfollow);
	}
	
	private void SetAnimations() {
		_position = Vector3.up;
		_position.Duration = 0.1f;
		_personageController = PersonageController.instance;
		
		Animation animat = _personageController.sTreeps.animation;
		string personageId = _personage + "";
		
		_animationHorizontal = new AnimationState[2];
		_animationHorizontal[0] = SetAnimationConfig(animat[personageId+"Left"], 10);
		_animationHorizontal[1] = SetAnimationConfig(animat[personageId+"Right"], 10);
		
		_animationVertical = new AnimationState[2];
		_animationVertical[0] = SetAnimationConfig(animat[personageId+"Down"], 11);
		_animationVertical[1] = SetAnimationConfig(animat[personageId+"Up"], 11);	
	}
	
	private AnimationState SetAnimationConfig(AnimationState animationState, int layer) {
		animationState.layer = layer;
		animationState.blendMode = AnimationBlendMode.Additive;
		animationState.wrapMode = WrapMode.ClampForever;
		animationState.weight = 1.0f;
		animationState.enabled = true;
		
		return animationState;
	}
	
	public virtual void LateUpdate () {
		IdleEyesight();
		
		if(!_logic.IsNightTime() ) {
			AnimateEyesight();	
		} else {
			// TO-DO Move to another place
			CheckBothering();
		}
		AnimatePosition();
	}
	
	private void CheckBothering() {
		if (_eyesightTarget != null && _eyesightTarget.collider.bounds.Intersects(gameObject.collider.bounds)) {
			_.Trigger ("BotherSleep", _personage);
			_eyesightTarget = null;
		}
	}
	
	private void IdleEyesight() {
		Position = Vector2.zero;	
	}
	
	private void AnimatePosition() {
		Vector3 position = _position;
		_animationHorizontal[0].normalizedTime = -position.x;
		_animationHorizontal[1].normalizedTime = position.x;
		_animationVertical[0].normalizedTime = -position.y;
		_animationVertical[1].normalizedTime = position.y;	
	}
	
	private bool AnimateEyesight() {
		if (_eyesightTarget != null) {
			Vector3 position = _eyesightTarget.transform.position - transform.position;
			
			if (position.magnitude < eyesightRadius) {
				Vector3 pos = position.normalized;
				Position = new Vector2(pos.x, pos.y);
				
				return true;
			}
		}
		
		return false;
	}
		
	public void OnEyesightFollow(object followObject, System.EventArgs events) {
		_eyesightTarget = (GameObject)followObject;
	}
	
	public void OnEyesightUnfollow(object followObject, System.EventArgs events) {
		GameObject target = (GameObject)followObject;
		
		if (target == _eyesightTarget) {
			_eyesightTarget = null;	
		}
	}
}
