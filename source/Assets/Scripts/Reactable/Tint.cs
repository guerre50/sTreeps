using UnityEngine;
using System.Collections;

public class Tint : MonoBehaviour {
	public GameObject[] tints;
	public GameObject tintCleanerPrefab;
	public TintCleaner tintCleaner;
	public SquidTint tintCamera;
	private float _time = -Mathf.Infinity;
	public float timeToStartClean = 2.0f;
	private Logic _logic;
	
	void Start () {
		tintCleaner = (Instantiate(tintCleanerPrefab, transform.position, transform.rotation) as GameObject).GetComponent<TintCleaner>();
		_logic = Logic.instance;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) {
			AnimateTint();	
		}
		
		if (_time >= 0) {
			_time -= Time.deltaTime;
			
			if (_time < 0) {
				tintCleaner.enabled = true;	
			}
		}
	}
	
	void AnimateTint() {
		Activate(true);
		_time = timeToStartClean;
		
		int first = 0;
		foreach(GameObject go in tints) {
			go.animation.Stop();
			go.transform.Translate(transform.position.x + first*Random.Range (-2.0f, 2.0f), 0, 0);
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
		
		tintCleaner.enabled = false;
		tintCleaner.renderer.material.color = Color.blue;
	}
	
	public void OnPressDown(InputInfo input) {
		if (tintCamera.Finished && _logic.Spit()) {
			_.Wait(1.8f).Done (AnimateTint);
		}
	}
}
