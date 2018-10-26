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
		if (itemData.ContainsKey (name))
			return itemData [name];
		Debug.LogWarningFormat ("Trying to get prototype: <color=red>{0}</color>, but it's missing!", name);
		return null;
	}

	public IPrototypeEntity GetEntity (string name) {
		if (entityData.ContainsKey (name))
			return entityData [name];
		Debug.LogWarningFormat ("Trying to get prototype: <color=red>{0}</color>, but it's missing!", name);
		return null;
	}

	public void Load () {
		List<ItemSerialized> items = new List<ItemSerialized> ();
		List<EntitySerialized> entities = new List<EntitySerialized> ();

		SaveLoad.LoadFromAssets (ref items, "items.json");
		SaveLoad.LoadFromAssets (ref entities, "entities.json");

		foreach (ItemSerialized i in items) {
			if (!itemData.ContainsKey (i.Name))
				itemData.Add (i.Name, i);
			else
				Debug.LogWarningFormat ("Trying to load an item : <color=red>{0}</color> , but a duplicate already exists!", i.Name.ToUpper ());
		}

		foreach (EntitySerialized e in entities) {
			if (!entityData.ContainsKey (e.Name))
				entityData.Add (e.Name, e);
			else
				Debug.LogWarningFormat ("Trying to load an entity : <color=red>{0}</color> , but a duplicate already exists!", e.Name.ToUpper ());
		}
	}
}

[System.Serializable]
public class ItemSerialized : IPrototypeItem {
	[SerializeField]
	string name;
	public string Name { get { return name; } set {name = value; } }
	[SerializeField]
	string type;
	public string Type { get { return type; } set { type = value; } }
	[SerializeField]
	bool stackable;
	public bool Stackable { get { return stackable; } set { stackable = value; } }
	[SerializeField]
	int stacksize;
	public int StackSize { get { return stacksize; } set { stacksize = value; } }
	public IPrototypeEntity EntityOnPlace { get; }

	public ItemSerialized (string Name = null, string Type = null, bool Stackable = true, int StackSize = 64) {
		this.Name = Name;
		this.Type = Type;
		this.Stackable = Stackable;
		this.StackSize = StackSize;
	}
}

[System.Serializable]
public class EntitySerialized : IPrototypeEntity {
	[SerializeField]
	string name;
	public string Name { get { return name; } set { name = value; } }
	[SerializeField]
	string obj;
	public string Object { get { return obj; } set { obj = value; } }
	public IPrototypeItem ItemOnPickup { get; }

	public EntitySerialized (string Name, string Object = null) {
		this.Name = Name;
		this.Object = Object;
	}
}