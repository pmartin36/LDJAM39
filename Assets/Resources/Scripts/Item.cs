using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public static GameObject Particles;
	
	public virtual void Start () {
		StartCoroutine(Bounce());
	}

	public virtual void Awake() {
		
	}

	public virtual void Update() {
		transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3( 0, 0, 45 * Time.deltaTime));
	}

	public virtual void PickupItem(GameObject picker) {

	}

	public virtual void OnTriggerEnter(Collider c) {
		if(c.tag == "Player") {
			PickupItem(c.gameObject);
			Destroy(this.gameObject);
		}	
	}

	IEnumerator Bounce() {
		Vector3 sourcePosition = transform.localPosition;
		Vector3 targetPosition = transform.localPosition + new Vector3(0, 0, -0.5f);
		float journeyTime = 2f;
		while (true) {
			float startTime = Time.time;
			while( Time.time - startTime < journeyTime + Time.deltaTime ) {
				float jTime = (Time.time - startTime ) / journeyTime;
				transform.localPosition = Vector3.Lerp(sourcePosition, targetPosition, jTime);
				yield return new WaitForEndOfFrame();
			}

			Vector3 temp = sourcePosition;
			sourcePosition = targetPosition;
			targetPosition = temp;
		}
	}
}
