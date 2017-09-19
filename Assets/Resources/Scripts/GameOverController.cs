using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverController : MonoBehaviour {

	TMP_Text [] texts;
	
	// Update is called once per frame
	void OnEnable () {
		StartCoroutine(ShowGameOver());
	}

	IEnumerator ShowGameOver() {
		texts = GetComponentsInChildren<TMP_Text>();
		float startTime = Time.time;
		float journeyTime = 1f;
		while( Time.time - startTime < journeyTime+Time.deltaTime ) {
			float jTime = (Time.time - startTime) / journeyTime;
			foreach(TMP_Text t in texts) {
				t.color = Color.Lerp( Color.clear, Color.white, jTime);
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
