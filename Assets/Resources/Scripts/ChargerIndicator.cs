using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargerIndicator : MonoBehaviour {

	public Transform player;
	public Transform charger;
	
	Canvas canvas;
	Vector2 canvasSize;

	RectTransform rectTransform;
	Image image;

	void Start() {
		canvas = transform.parent.gameObject.GetComponent<Canvas>();
		canvasSize = new Vector2(755,410);//canvas.pixelRect.size;

		rectTransform = GetComponent<RectTransform>();
		Debug.Log(canvasSize);

		image = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 vector = charger.position - player.position;
		float distance = vector.magnitude;

		Color c = image.color;
		c.a = Mathf.Clamp01((distance - 10) / 90)*0.75f;	
		image.color = c;

		Vector3 n = vector.normalized;

		transform.localRotation = Quaternion.Euler(0f, 0f, VectorToAngle(n));
		Vector3 offset = Vector3.one * (Mathf.Sign(rectTransform.rect.yMin) * rectTransform.rect.size.x * 0.05f);
		rectTransform.localPosition = n * Mathf.Min( canvasSize.x, canvasSize.y);// + offset;
	}

	public float xyToAngle(float x, float y) {
		return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
	}

	public float VectorToAngle(Vector2 vector) {
		return xyToAngle(vector.x, vector.y);
	}
}
