using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInventory {
	public class Inventory {
		int size;
		public string Type { get; protected set; }
		List<ItemStack> contents = new List<ItemStack> ();
		public List<ItemStack> Contents { get { return contents; } }
		public int Size { get { return size; } }

		public Inventory (int i, string type = "") {
			size = i;
			Type = type;
		}

		public int Insert (ItemStack stack) { //return amount actually inserted
			Cleanup ();
			if (!CanInsert (stack))
				return 0;
			if (stack && (stack.Item != null) && (stack.Item.Length > 0) & (stack.Size > 0)) {
				int accumulate = 0;
				ItemStack similar = contents.FindLast (x => x.Item == stack.Item);
				if ((similar && (similar.Size >= similar.Prototype.StackSize)) || !similar) {
					if (contents.Count < size) {
						Contents.Add (stack);
						accumulate += stack.Size;
					}
				} else {
					int i = similar.Combine (stack);
					accumulate += stack.Size - i;
					stack.Size = i;
					accumulate += Insert (stack);
				}
				return accumulate;
			}
			return 0;
		}

		public int Remove (ItemStack stack) { //return amount actually removed
			Cleanup ();
			if (stack && (stack.Item != null) && (stack.Item.Length > 0) & (stack.Size > 0)) {
				int accumulate = 0;
				ItemStack similar = contents.FindLast (x => x.Item == stack.Item);
				if (similar) {
					stack.Size -= similar.Subtract (stack);
					accumulate += Remove (stack);
				}
				return accumulate;
			}
			return 0;
		}

		public bool CanInsert (ItemStack stack) {
			Cleanup ();
			if ((stack.Prototype.Type == this.Type) || (this.Type.Length == 0))
				return true;
			Debug.LogFormat ("Cannot insert stack of {0} into {1}", stack.Item, this);
			return false;
		}

		void Cleanup () {
			contents.RemoveAll (x => (!x || (x.Item == null) || (x.Item.Length == 0) || (x.Size == 0)));
		}
	}

	[Serializable]
	public class PrototypeItem {
		[SerializeField]
		protected string name;
		[SerializeField]
		protected string type;
		[SerializeField]
		protected bool stackable;
		[SerializeField]
		protected int stacksize;
		[SerializeField]
		protected string entity;

		public string Name { get { return name; } }
		public string Type { get { return type; } }
		public bool Stackable { get { return stackable; } }
		public int StackSize { get { if (stackable)
					return stacksize;
				else
					return 1;} }
		public string Entity { get { return entity; } }

		public PrototypeItem (string Name = "", string Type = "", bool Stackable = true, int StackSize = 64, string Entity = "") {
			name = Name;
			type = Type;
			stackable = Stackable;
			stacksize = StackSize;
			entity = Entity;
		}

		public override int GetHashCode () {
			int hash = 0;
			System.Int32.TryParse (name, out hash);
			return hash;
		}

		public override bool Equals (object obj) {
			if (!(obj is PrototypeItem))
				return false;
			var other = obj as PrototypeItem;
			if (this.GetHashCode () != other.GetHashCode ())
				return false;
			return true;
		}

		public static bool operator ==(PrototypeItem x, PrototypeItem y) {
			if (object.ReferenceEquals (null, x))
				return object.ReferenceEquals (null, y);
			return x.Equals(y);
		}

		public static bool operator !=(PrototypeItem x, PrototypeItem y) {
			return !(x == y);
		}

		public static implicit operator bool (PrototypeItem me) {
			return me != null;
		}
	}

	public class ItemStack {
		int size;
		string item;

		public string Item { get {return item; } }
		public int Size {
			get { return size; }
			set {
				if (value > Prototype.StackSize)
					size = Prototype.StackSize;
				else if (value < 0)
					size = 0;
				else
					size = value;
			}
		}
		public PrototypeItem Prototype {
			get {
				PrototypeItem i = DataManagement.Instance.GetItem(item);
				if (i != null)
					return i;
				else
					throw new Exception ();
			}
		}

		public ItemStack (string Item = "", int Size = 0) {
			this.item = Item;
			this.Size = Size;
			Debug.LogFormat ("Created new stack of <color=brown>{0} x {1}</color>", item, size);
		}

		public int Combine (ItemStack stack) { //returns whats left to add
			int total = size + stack.Size;
			size = (total > Prototype.StackSize) ? Prototype.StackSize : total;
			return total - size;
		}

		public int Subtract (ItemStack stack) { //returns whats was removed
			int result = (size < stack.Size) ? 0 : size - stack.Size;
			int removed = size - result;
			size = result;
			return removed;
		}

		public bool SetStack (ItemStack stack) {
			item = stack.Item;
			size = stack.Size;
			return true;
		}

		public bool SwapStack (ref ItemStack stack) {
			ItemStack temp = new ItemStack();
			temp.SetStack (this);
			this.SetStack (stack);
			stack.SetStack (temp);
			return true;
		}

		public override int GetHashCode () {
			return base.GetHashCode ();
		}

		public override bool Equals (object obj) {
			if (!(obj is ItemStack))
				return false;
			var other = obj as ItemStack;
			if (!((this.item == other.Item) && (this.size == other.Size)))
				return false;
			return true;
		}

		public static bool operator ==(ItemStack x, ItemStack y) {
			if (object.ReferenceEquals (null, x))
				return object.ReferenceEquals (null, y);
			return x.Equals (y);
		}

		public static bool operator !=(ItemStack x, ItemStack y) {
			return !(x == y);
		}

		public static implicit operator bool (ItemStack me) {
			return me != null;
		}
	}

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