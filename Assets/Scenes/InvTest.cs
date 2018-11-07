using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterInventory;

public class InvTest : MonoBehaviour {

	List<ItemSerialized> items = new List<ItemSerialized> ();

	Inventory inv;

	void Awake () {
		ItemSerialized itemS = new ItemSerialized ("Test Item");
		items.Add (itemS);

		Debug.LogFormat ("Added a new item {0}", itemS);

		SaveLoad.SaveToAssets (ref items, "items.json");

		DataManagement.Instance.Load ();

		Debug.Log ("Loaded data:");
		foreach (KeyValuePair<string, IPrototypeItem> i in DataManagement.Instance.ItemData)
			Debug.LogFormat ("Item: {0}, type: {1}, stackable: {2}, stacksize: {3}", i.Value.Name, i.Value.Type, i.Value.Stackable, i.Value.StackSize);


		inv = new Inventory (5);

		Debug.Log ("Created a new inventory");

		ItemStack stack = new ItemStack ("Test Item", 25);
		inv.Insert (stack);
		foreach (IStack i in inv.Contents)
			Debug.LogFormat ("Inventory contents: <color=blue>{0} x {1}</color>", i.Item, i.Size);
		stack = new ItemStack ("Test Item", 58);
		inv.Insert (stack);
		foreach (IStack i in inv.Contents)
			Debug.LogFormat ("Inventory contents: <color=blue>{0} x {1}</color>", i.Item, i.Size);
		stack = new ItemStack ("Test Item", 50);
		inv.Remove (stack);
		foreach (IStack i in inv.Contents)
			Debug.LogFormat ("Inventory contents: <color=blue>{0} x {1}</color>", i.Item, i.Size);
	}
}