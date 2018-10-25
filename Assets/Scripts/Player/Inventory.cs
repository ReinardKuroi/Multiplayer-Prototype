using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInventory

public class Inventory {

}

public class Item {
	
}

public class InventoryItem {
	Item item;
	int amount;

	public int StackSize { get { return 64; } }
	public int Stack { get { return amount; } }

	public InventoryItem (Item i) {
		
	}
}