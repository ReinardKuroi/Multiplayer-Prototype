using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMotor;
using IManager;

public interface IPrototypeItem {
	string Name { get; }
	bool Stackable { get; }
	int StackSize { get; }

	IPrototypeEntity EntityOnPlace { get; }
}

public interface IStack {
	int Size { get; }
	bool IsFull { get; }
	 
	IPrototypeItem Item { get; }

	ICharacterInventory Inventory { get; }
}

public interface IPrototypeEntity {
	string Name { get; }
	string Object { get; }

	IPrototypeItem ItemOnPickup { get; }
}

public interface ICharacterInventory {
	List<IStack> Contents { get; }

	int Add (IPrototypeItem item, int amount);
}

public interface IDataManagement {
	Dictionary<string, IPrototypeItem> ItemData { get; }
	Dictionary<string, IPrototypeEntity> EntityData { get; }

	IPrototypeItem GetItem (string name);
	IPrototypeEntity GetEntity (string name);
	void Load ();
}