﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IManager {
	public class InputManager {
		protected static InputManager instance = new InputManager ();
		public static InputManager Instance {
			get {
				return instance;
			}
		}

		Dictionary<KeyCode, Command> CommandList = new Dictionary<KeyCode, Command> ();

		public void HandleInput () {
			foreach (KeyValuePair<KeyCode, Command> kvp in CommandList) {
				if (Input.GetKeyDown (kvp.Key)) {
					kvp.Value ();
					Debug.LogFormat ("{0} was called by {1}", kvp.Value.Method, kvp.Key);
				}
			}
		}

		public void Init () {
			CommandList = new Dictionary<KeyCode, Command> ();
		}

		public void AddCommand (KeyCode key, Command command) {
			if (!CommandList.ContainsKey (key)) {
				CommandList.Add (key, command);
				Debug.LogFormat ("Added command {0} called on key {1}", command.Method, key);
			} else {
				Debug.LogWarningFormat ("Trying to add a command to key : <color=red>{0}</color> , but there is already another command assigned!", key);
			}
		}
	}

	public delegate void Command ();
}