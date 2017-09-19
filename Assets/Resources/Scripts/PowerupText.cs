using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerupText : MonoBehaviour {

	float startTime;
	TMP_Text Text;

	Vector2 initSize;
	Vector2 endSize;

	public void Init(string text) {
		Text = GetComponent<TMP_Text>();
		Text.text = text;

		initSize = Vector2.one * 0.6f;
		transform.localScale = initSize;

		endSize = Vector2.one * 0.8f;

		startTime = Time.time;
	}

	// Use this for initialization
	void Start () {
		Text = GetComponent<TMP_Text>();
	}

	void Update() {
		float tdiff = Time.time - startTime;
		if(tdiff < 0.5f) {
			//grow
			float jTime = tdiff / 0.5f;
			transform.localScale = Vector2.Lerp( initSize, endSize, jTime );
			Text.color = Color.Lerp( Color.gray, Color.white, jTime );
		}
		else if(tdiff < 4.5f){
			//fade
			float jTime = (tdiff - 0.5f);
			Text.color = Color.Lerp(Color.white, Color.clear, jTime);
		}
		else {
			Destroy(this.gameObject);
		}
	}
}
