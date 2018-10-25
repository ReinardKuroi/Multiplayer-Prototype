using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagement : IDataManagement {
	protected static DataManagement instance = new DataManagement ();
	public static DataManagement Instance { get { return instance; } }

	Dictionary<string, IPrototypeItem> itemData = new Dictionary<string, IPrototypeItem> ();
	Dictionary<string, IPrototypeEntity> entityData = new Dictionary<string, IPrototypeEntity> ();

	public Dictionary<string, IPrototypeItem> ItemData { get { return itemData; } }
	public Dictionary<string, IPrototypeEntity> EntityData { get { return entityData; } }

	public IPrototypeItem GetItem (string name) {
		if (itemData.ContainsKey (name)) {
			IPrototypeItem item = ItemData [name];
			return item;
		}
		return null;
	}

	public IPrototypeEntity GetEntity (string name) {
		if (entityData.ContainsKey (name)) {
			IPrototypeEntity entity = entityData [name];
			return entity;
		}
		return null;
	}

	public void Load () {
		List<IPrototypeItem> items = new List<IPrototypeItem> ();
		List<IPrototypeEntity> entities = new List<IPrototypeEntity> ();

		SaveLoad.LoadFromAssets (ref items, "items.json");
		SaveLoad.LoadFromAssets (ref entities, "entities.json");

		foreach (IPrototypeItem i in items) {
			if (!itemData.ContainsKey (i.Name))
				itemData.Add (i.Name, i);
			else
				Debug.LogWarningFormat ("Trying to load an item : <color=red>{0}</color> , but a duplicate already exists!", i.Name.ToUpper ());
		}

		foreach (IPrototypeEntity e in entities) {
			if (!entityData.ContainsKey (e.Name))
				entityData.Add (e.Name, e);
			else
				Debug.LogWarningFormat ("Trying to load an entity : <color=red>{0}</color> , but a duplicate already exists!", e.Name.ToUpper ());
		}
	}
}