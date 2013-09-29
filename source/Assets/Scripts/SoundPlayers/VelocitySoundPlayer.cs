using UnityEngine;
using System.Collections;

public class VelocitySoundPlayer : Reactable {
	public Vector2 VelocityRange;
	public Vector2 VolumeRange;
	
	void Update () {
		float velocity = (rigidbody.velocity.magnitude - VelocityRange.x)/VelocityRange.magnitude;
		audio.volume = Mathf.Clamp01(Mathf.Lerp(VolumeRange.x, VolumeRange.y, velocity));
	}
	
	public override void OnSelect() {
		audio.Play();
	}
	
	public override void OnDeselect() {
		audio.Pause();	
	}
}
