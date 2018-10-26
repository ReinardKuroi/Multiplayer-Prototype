using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterInventory;

public class Test : MonoBehaviour {

	List<ItemSerialized> items = new List<ItemSerialized> ();
	List<EntitySerialized> entities = new List<EntitySerialized> ();

	Inventory inv;

	void Awake () {
		ItemSerialized itemS = new ItemSerialized ("Test Item");
		EntitySerialized entityS = new EntitySerialized ("Test Item");
		items.Add (itemS);
		entities.Add (entityS);

		Debug.LogFormat ("Added a new item {0}", itemS);
		Debug.LogFormat ("Added a new entity {0}", entityS);

		SaveLoad.SaveToAssets (ref items, "items.json");
		SaveLoad.SaveToAssets (ref entities, "entities.json");

		DataManagement.Instance.Load ();

		Debug.Log ("Loaded data:");
		foreach (KeyValuePair<string, IPrototypeItem> i in DataManagement.Instance.ItemData)
			Debug.LogFormat ("Item: {0}, type: {1}, stackable: {2}, stacksize: {3}", i.Value.Name, i.Value.Type, i.Value.Stackable, i.Value.StackSize);
		foreach (KeyValuePair<string, IPrototypeEntity> e in DataManagement.Instance.EntityData)
			Debug.LogFormat ("Entity {0}, object: {1}", e.Value.Name, e.Value.Object);
		
		inv = new Inventory (5);

		Debug.Log ("Created a new inventory");

		ItemStack stack = new ItemStack ("Test Item", 25);
		Insert (inv, stack);
		foreach (IStack i in inv.Contents)
			Debug.LogFormat ("Inventory contents: <color=blue>{0} x {1}</color>", i.Item, i.Size);
		stack = new ItemStack ("Test Item", 58);
		Insert (inv, stack);
		foreach (IStack i in inv.Contents)
			Debug.LogFormat ("Inventory contents: <color=blue>{0} x {1}</color>", i.Item, i.Size);
		stack = new ItemStack ("Test Item", 50);
		Remove (inv, stack);
		foreach (IStack i in inv.Contents)
			Debug.LogFormat ("Inventory contents: <color=blue>{0} x {1}</color>", i.Item, i.Size);
	}

	void Insert (ICharacterInventory inv, IStack s) {
		int inserted = inv.Insert (s);
		Debug.LogFormat ("Inserted <color=green>{0} x {1}</color> in inventory", s.Item, inserted);
		if (inserted < s.Size) {
			s.Size -= inserted;
			Insert (inv, s);
		}
	}

	void Remove (ICharacterInventory inv, IStack s) {
		int removed = inv.Remove (s);
		Debug.LogFormat ("Removed <color=red>{0} x {1}</color> in inventory", s.Item, removed);
		if (removed < s.Size) {
			s.Size -= removed;
			Remove (inv, s);
		}
	}
}