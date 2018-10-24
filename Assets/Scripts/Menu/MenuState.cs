using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MenuState {
	enum MState {
		Menu,
		Game
	}

	MState State { get; set; }
	public bool InGame { get { return State == MState.Game; } }
	GameObject UI;

	public MenuState (GameObject _UI) {
		this.UI = _UI;
		Exit ();
		Debug.LogFormat ("<color=red>NEW MENU</color> {0}", this);
	}

	public void Toggle () {
		if (State == MState.Game)
			Enter ();
		else if (State == MState.Menu)
			Exit ();
	}

	public void Enter () {
		State = MState.Menu;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
		UI.SetActive (true);
	}

	public void Exit () {
		State = MState.Game;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		UI.SetActive (false);
	}
}