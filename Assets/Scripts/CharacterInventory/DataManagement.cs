using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagement : IDataManagement {
	protected static DataManagement instance = new DataManagement ();
	public static DataManagement Instance { get { return instance; } }

	Dictionary<string, IPrototypeItem> itemData = new Dictionary<string, IPrototypeItem> ();

	public Dictionary<string, IPrototypeItem> ItemData { get { return itemData; } }

	public IPrototypeItem GetItem (string name) {
		string temp = name.ToLowerInvariant ().Replace (" ", "-");
		if (itemData.ContainsKey (temp))
			return itemData [temp];
		Debug.LogWarningFormat ("Trying to get prototype : <color=red>{0}</color> , but it's missing!", temp);
		return null;
	}

	public void Load () {
		List<ItemSerialized> items = new List<ItemSerialized> ();

		SaveLoad.LoadFromAssets (ref items, "items.json");

		foreach (ItemSerialized i in items) {
			string temp = i.Name.ToLowerInvariant ().Replace (" ", "-");
			if (!itemData.ContainsKey (i.Name)) {
				IPrototypeItem _i = i as IPrototypeItem;
				itemData.Add (temp, _i);
				Debug.LogFormat ("Loaded prototype : <color=brown>{0}</color>", itemData[temp].Name);
			}
			else
				Debug.LogWarningFormat ("Trying to load a prototype : <color=red>{0}</color> , but a duplicate already exists!", temp);
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
	[SerializeField]
	string entity;
	public string Entity { get { return entity; } set { entity = value; } }

	public ItemSerialized (string Name = null, string Type = null, bool Stackable = true, int StackSize = 64, string Entity = null) {
		this.Name = Name;
		this.Type = Type;
		this.Stackable = Stackable;
		this.StackSize = StackSize;
		this.Entity = (Entity == null) ? Name : Entity;
	}
}