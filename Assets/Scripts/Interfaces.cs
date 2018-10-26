using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMotor;
using IManager;

public interface IPrototypeItem {
	string Name { get; }
	string Type { get; }
	bool Stackable { get; }
	int StackSize { get; }

	IPrototypeEntity EntityOnPlace { get; }
}

public interface IStack {
	int Size { get; set; }
	string Item { get; }
	IPrototypeItem Prototype { get; }

	int Combine (IStack stack);
	int Subtract (IStack stack);
	bool SetStack (IStack stack);
	bool SwapStack (ref IStack stack);
}

public interface IPrototypeEntity {
	string Name { get; }
	string Object { get; }

	IPrototypeItem ItemOnPickup { get; }
}

public interface ICharacterInventory {
	string Type { get; }
	List<IStack> Contents { get; }

	bool CanInsert (IStack stack);
	int Insert (IStack stack);
	int Remove (IStack stack);
}

public interface IDataManagement {
	Dictionary<string, IPrototypeItem> ItemData { get; }
	Dictionary<string, IPrototypeEntity> EntityData { get; }

	IPrototypeItem GetItem (string name);
	IPrototypeEntity GetEntity (string name);
	void Load ();
}