using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour {

	public Player Target;
	private Vector3 CameraOffset;

	private bool StoppingCoroutine = false;
	private bool LockCamera = false;

	// Use this for initialization
	void Start () {
		CameraOffset = this.transform.position;
		CameraOffset.z = 0;

		/*
		Shader rps = Resources.Load<Shader>("Shaders/WalkableEnemyReplacement");
		if(rps != null) {
			Camera enemyCamera = null;
			foreach( Camera c in GetComponentsInChildren<Camera>() ) {
				if( c.gameObject != this.gameObject)
					enemyCamera = c;
			}
			if(enemyCamera != null) {
				enemyCamera.SetReplacementShader(rps, "");
			}
		}
		*/
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(!LockCamera) {
			SetPosition();
		}
	}

	private void SetPosition() {
		transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, this.transform.position.z) + CameraOffset;
	}

	IEnumerator AnimateFloat(string property, Material m, float start, float target, float journeyTime) {
		float startTime = Time.time;
		while (Time.time - startTime < journeyTime + Time.deltaTime) {
			if (StoppingCoroutine)
				yield break;

			float jTime = (Time.time - startTime) / journeyTime;
			m.SetFloat(property, Mathf.Lerp(start, target, jTime));

			yield return new WaitForEndOfFrame();
		}
	}

	//public void OnRenderImage(RenderTexture src, RenderTexture dst) {	
	//	if(BlindEffectMaterial != null) {
	//		Graphics.Blit(src,dst, BlindEffectMaterial);
	//	}
	//	else {
	//		Graphics.Blit(src, dst);
	//	}
	//}
}
