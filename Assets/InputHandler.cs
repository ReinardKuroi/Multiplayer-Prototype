using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IManager;

public class InputHandler : MonoBehaviour {

	InputManager iManager;

	void Start () {
		iManager = InputManager.Instance;
	}

	void Update () {
		iManager.HandleInput ();
	}
}