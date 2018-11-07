using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInventory {
	public class DataManagement {
		protected static DataManagement instance = new DataManagement ();
		public static DataManagement Instance { get { return instance; } }

		Dictionary<string, PrototypeItem> itemData = new Dictionary<string, PrototypeItem> ();

		public Dictionary<string, PrototypeItem> ItemData { get { return itemData; } }

		public PrototypeItem GetItem (string name) {
			string temp = name.ToLowerInvariant ().Replace (" ", "-");
			if (itemData.ContainsKey (temp)) {
				PrototypeItem i = itemData [temp];
				return i;
			}
			Debug.LogWarningFormat ("Trying to get prototype : <color=red>{0}</color> , but it's missing!", temp);
			return null;
		}

		public void Load () {
			List<PrototypeItem> items = new List<PrototypeItem> ();

			SaveLoad.LoadFromAssets (ref items, "items.json");

			foreach (PrototypeItem i in items) {
				string temp = i.Name.ToLowerInvariant ().Replace (" ", "-");
				if (!itemData.ContainsKey (i.Name)) {
					itemData.Add (temp, i);
					Debug.LogFormat ("Loaded prototype : <color=brown>{0}</color>", itemData[temp].Name);
				}
				else
					Debug.LogWarningFormat ("Trying to load a prototype : <color=red>{0}</color> , but a duplicate already exists!", temp);
			}
		}
	}
}