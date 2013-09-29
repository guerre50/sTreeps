using UnityEngine;
using System.Collections;

public class Tint : MonoBehaviour {
	public GameObject[] tints;
	public GameObject tintCleanerPrefab;
	public TintCleaner tintCleaner;
	public SquidTint tintCamera;
	private float timeToStartClean = 2.2f;
	private Logic _logic;
	private Promise _pendentClean;
	public Bubble bubble;
	public SoundPressPlayer spitSound;
	
	void Start () {
		tintCleaner = (Instantiate(tintCleanerPrefab, transform.position, transform.rotation) as GameObject).GetComponent<TintCleaner>();
		_logic = Logic.instance;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) {
			AnimateTint();	
		}
	}
	
	void AnimateTint() {
		Activate(true);
		
		int first = 0;
		foreach(GameObject go in tints) {
			go.animation.Stop();
			go.transform.position = transform.position + new Vector3(first*Random.Range (-2.0f, 2.0f), first*Random.Range (-2.0f, 2.0f), 0);
			float scale = Random.Range (0.3f, 0.8f);
			go.transform.localScale = new Vector3(scale, scale, scale);
			foreach (AnimationState state in go.animation) {
	            state.speed = Random.Range (0.8f, 2.0f);
	        }
			go.animation.Play("Grow", PlayMode.StopAll);
			first = 1;
		}
	}
	
	void FinishTint() {
	}
	
	void Activate(bool enabled) {
		tintCamera.Tint(FinishTint);
		tintCleaner.renderer.enabled = false;
		if (_pendentClean != null) {
			_pendentClean.Cancel();	
		}
		_pendentClean = _.Wait(timeToStartClean).Done (EnableCleaner);
		tintCleaner.renderer.material.color = Color.blue;
	}
	
	private void EnableCleaner() {
		tintCleaner.renderer.enabled = true;
	}
	
	public void OnPressDown(InputInfo input) {
		if (_logic.Spit()) {
			_.Wait(1.8f).Done (AnimateTint);
			spitSound.Play();
		} else {
			bubble.Explode();	
		}
	}
}
