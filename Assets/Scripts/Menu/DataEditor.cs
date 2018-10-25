using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class DataEditor : EditorWindow {

	bool TrueIfItems = true;
	Vector2 scrollPosition;

	List<Item> items = new List<Item> ();
	List<Entity> entities = new List<Entity> ();

	[MenuItem("Window/Inventory/Data Editor")]
	public static void ShowWindow () {
		EditorWindow.GetWindow<DataEditor> ();
	}

	void Awake () {
		LoadItems ();
		LoadEntities ();
	}

	void OnGUI () {
		if (TrueIfItems)
			DrawItemMenu ();
		else
			DrawEntitiesMenu ();
	}

	void DrawItemMenu () {
		GUILayout.Label ("Item Prototypes", EditorStyles.boldLabel);

		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
		if (items != null) {
			foreach (IPrototypeItem i in items) {
				if (GUILayout.Button (i.Name)) {
					Edit (i);
				}
			}
		}
		if (GUILayout.Button ("Add"))
			AddItem ();
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Switch to entities")) {
			TrueIfItems = false;
		}
		EditorGUILayout.EndHorizontal ();
	}

	void DrawEntitiesMenu () {
		GUILayout.Label ("Entity Prototypes", EditorStyles.boldLabel);

		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
		if (entities != null) {
			foreach (IPrototypeEntity i in entities) {
				if (GUILayout.Button (i.Name)) {
					Edit (i);
				}
			}
		}

		if (GUILayout.Button ("Add"))
			AddEntity ();
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Switch to items")) {
			TrueIfItems = true;
		}
		EditorGUILayout.EndHorizontal ();
	}

	void Edit<T> (T t) {
		
	}

	void AddItem () {
		items.Add(new Item ("Test"));
		SaveItems ();
		LoadItems ();
	}

	void AddEntity () {
		entities.Add(new Entity ("Test"));
		SaveEntities ();
		LoadEntities ();
	}

	void LoadItems () {
		SaveLoad.LoadFromAssets (ref items, "items.json");
	}

	void SaveItems () {
		SaveLoad.SaveToAssets (ref items, "items.json");
	}

	void LoadEntities () {
		SaveLoad.LoadFromAssets (ref entities, "entities.json");
	}

	void SaveEntities () {
		SaveLoad.SaveToAssets (ref items, "entities.json");
	}

	[Serializable]
	class Item : IPrototypeItem {
		[SerializeField]
		public string Name { get; set; }
		[SerializeField]
		public bool Stackable { get; set; }
		[SerializeField]
		public int StackSize { get; set ;}
		public IPrototypeEntity EntityOnPlace { get; }

		public Item (string Name, bool Stackable = true, int StackSize = 64) {
			this.Name = Name;
			this.Stackable = Stackable;
			this.StackSize = StackSize;
		}
	}

	[Serializable]
	class Entity : IPrototypeEntity {
		[SerializeField]
		public string Name { get; set; }
		[SerializeField]
		public string Object { get; set; }
		public IPrototypeItem ItemOnPickup { get; }

		public Entity (string Name, string Object = "") {
			this.Name = Name;
			this.Object = Object;
		}
	}
}