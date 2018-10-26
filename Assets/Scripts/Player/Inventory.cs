using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInventory {
	public class Inventory : ICharacterInventory {
		int size;
		public string Type { get; protected set; }
		List<IStack> contents = new List<IStack> ();
		public List<IStack> Contents { get { return contents; } }
		public int Size { get { return size; } }

		public Inventory (int i, string type = null) {
			size = i;
			Type = type;
		}

		public int Insert (IStack stack) { //return amount actually inserted
			Cleanup ();
			if (!CanInsert (stack))
				return 0;
			if ((stack != null) && (stack.Item != null) && (stack.Size > 0)) {
				int accumulate = 0;
				IStack similar = contents.FindLast (x => x.Item == stack.Item);
				if (((similar != null) && (similar.Size >= similar.Prototype.StackSize)) || (similar == null)) {
					if (contents.Count < size) {
						Contents.Add (stack);
						accumulate += stack.Size;
					}
				} else {
					stack.Size = similar.Combine (stack);
					accumulate += Insert (stack);
				}
				return accumulate;
			}
			return 0;
		}

		public int Remove (IStack stack) { //return amount actually removed
			Cleanup ();
			if ((stack != null) && (stack.Item != null) && (stack.Size > 0)) {
				int accumulate = 0;
				IStack similar = contents.FindLast (x => x.Item == stack.Item);
				if ((similar != null)) {
					stack.Size -= similar.Subtract (stack);
					accumulate += Remove (stack);
				}
				return accumulate;
			}
			return 0;
		}

		public bool CanInsert (IStack stack) {
			Cleanup ();
			if (stack.Prototype.Type == this.Type || this.Type == null)
				return true;
			return false;
		}

		void Cleanup () {
			contents.RemoveAll (x => x.Item == null || x.Size == 0);
		}
	}

	public class PrototypeItem : IPrototypeItem {
		readonly string name;
		readonly bool stackable;
		readonly int stacksize;

		public string Name { get { return name; } }
		public string Type { get; protected set; }
		public bool Stackable { get { return stackable; } }
		public int StackSize { get { if (stackable)
					return stacksize;
				else
					return 1;} }
		public IPrototypeEntity EntityOnPlace { get { return DataManagement.Instance.GetEntity (name) as PrototypeEntity; } }

		public PrototypeItem (string Name = null, bool Stackable = true, int StackSize = 64) {
			name = Name;
			stackable = Stackable;
			stacksize = StackSize;
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
			return x.Equals(y);
		}

		public static bool operator !=(PrototypeItem x, PrototypeItem y) {
			return !(x == y);
		}

		public static implicit operator bool (PrototypeItem me) {
			return me != null;
		}
	}

	public class PrototypeEntity : IPrototypeEntity {
		string name;
		string obj;

		public string Name { get { return name; } }
		public string Object { get { return obj; } }

		public IPrototypeItem ItemOnPickup { get { return DataManagement.Instance.GetItem (name) as PrototypeItem; } }

		public PrototypeEntity (string Name = null, string Object = null) {
			name = Name;
			obj = Object;
		}

		public override int GetHashCode () {
			int hash = 0;
			System.Int32.TryParse (name, out hash);
			return hash;
		}

		public override bool Equals (object obj) {
			if (!(obj is PrototypeEntity))
				return false;
			var other = obj as PrototypeEntity;   
			if (this.GetHashCode () != other.GetHashCode ())
				return false;
			return true;
		}

		public static bool operator ==(PrototypeEntity x, PrototypeEntity y) {
			return x.Equals(y);
		}

		public static bool operator !=(PrototypeEntity x, PrototypeEntity y) {
			return !(x == y);
		}

		public static implicit operator bool (PrototypeEntity me) {
			return me != null;
		}
	}

	public class ItemStack : IStack {
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
		public IPrototypeItem Prototype {
			get {
				IPrototypeItem i = DataManagement.Instance.GetItem(item);
				if (i != null)
					return i;
				else
					throw new Exception ();
			}
		}

		public ItemStack (string Item = null, int Size = 0) {
			this.item = Item;
			this.Size = Size;
			Debug.LogFormat ("Created new stack of <color=brown>{0} x {1}</color>", item, size);
		}

		public int Combine (IStack stack) { //returns whats left to add
			int total = size + stack.Size;
			size = (total > Prototype.StackSize) ? Prototype.StackSize : total;
			return total - size;
		}

		public int Subtract (IStack stack) { //returns whats was removed
			int result = (size < stack.Size) ? 0 : size - stack.Size;
			int removed = size - result;
			size = result;
			return removed;
		}

		public bool SetStack (IStack stack) {
			item = stack.Item;
			size = stack.Size;
			return true;
		}

		public bool SwapStack (ref IStack stack) {
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
			if (!(this.item == other.Item && this.size == other.Size))
				return false;
			return true;
		}

		public static bool operator ==(ItemStack x, ItemStack y) {
			return x.Equals (y);
		}

		public static bool operator !=(ItemStack x, ItemStack y) {
			return !x.Equals (y);
		}

		public static implicit operator bool (ItemStack me) {
			return me != null;
		}
	}
}