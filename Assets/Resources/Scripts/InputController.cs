using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	void Start() {
	}

	// Update is called once per frame
	void Update() {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		InputPackage p = new InputPackage() {
			Horizontal = horizontal,
			Vertical = vertical,
			Pause = Input.GetButtonDown("Pause"),
			Select = Input.GetButtonDown("Select"),
			Fire = Input.GetButton("Select"),
			Up = Input.GetButtonDown("Up"),
			Down = Input.GetButtonDown("Down"),
			Left = Input.GetButtonDown("Left"),
			Right = Input.GetButtonDown("Right")
		};

		GameManager.Instance.ProcessInputs(p);
	}

}
