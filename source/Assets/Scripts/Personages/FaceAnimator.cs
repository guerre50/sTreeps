using UnityEngine;
using System.Collections;
using Radical;

public class FaceAnimator : MonoBehaviour {
	Personage personage;
	private PersonageController _personageController;
	AnimationState[] animationHorizontal;
	AnimationState[] animationVertical;
	Vector2 position;
	
	void Start () {
		personage = GetComponent<Personage>();
	
		_personageController = PersonageController.instance;
		Animation animat = _personageController.sTreeps.animation;
		string personageId = personage.type +"";
		
		animationHorizontal = new AnimationState[2];
		animationHorizontal[0] = SetAnimationConfig(animat[personageId+"Left"], 10);
		animationHorizontal[1] = SetAnimationConfig(animat[personageId+"Right"], 10);
		
		animationVertical = new AnimationState[2];
		animationVertical[0] = SetAnimationConfig(animat[personageId+"Down"], 11);
		animationVertical[1] = SetAnimationConfig(animat[personageId+"Up"], 11);
	}
	
	private AnimationState SetAnimationConfig(AnimationState animationState, int layer) {
		animationState.layer = layer;
		animationState.blendMode = AnimationBlendMode.Additive;
		animationState.wrapMode = WrapMode.ClampForever;
		animationState.weight = 1.0f;
		animationState.enabled = true;
		
		return animationState;
	}
	
	
	void Update () {
		float delta = 0.1f;
		position += new Vector2((Input.GetKey(KeyCode.LeftArrow)? -delta : 0)+ (Input.GetKey(KeyCode.RightArrow)? delta: 0),
								(Input.GetKey(KeyCode.DownArrow)? -delta : 0)+ (Input.GetKey(KeyCode.UpArrow)? delta: 0));
		
		position = Vector2.ClampMagnitude(position, 1);
		animationHorizontal[0].normalizedTime = -position.x;
		animationHorizontal[1].normalizedTime = position.x;
		animationVertical[0].normalizedTime = -position.y;
		animationVertical[1].normalizedTime = position.y;
	}
}
