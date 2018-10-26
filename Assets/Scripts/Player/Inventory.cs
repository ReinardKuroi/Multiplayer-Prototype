using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInventory {

	public class Inventory : ICharacterInventory {
		int size;
		List<IStack> contents = new List<IStack> ();
		public List<IStack> Contents { get { return contents; } }
		public int Size { get { return size; } }

		public Inventory (int i) {
			size = i;
		}

		public int Insert (IStack stack) { //return amount actually inserted
			contents.RemoveAll (x => x.Item == null || x.Size == 0);
			if (stack != null) {
				IStack similar = contents.FindLast (x => x.Item == stack.Item);
				if (((similar != null) && similar.IsFull) || (similar == null)) {
					if (contents.Count < size) {
						Contents.Add (stack);
						return stack.Size;
					}
				} else
					return similar.Combine (stack);
			}
			return 0;
		}
		/*
		public int Remove (IStack stack) {
			if (stack != null) {
				IStack similar = contents.FindLast (x => x.Item == stack.Item);
				if ((similar != null)) {
					
				}
			}
		}*/

		void Cleanup () {
			contents.RemoveAll (x => x.Item == null || x.Size == 0);
		}
	}

	public class PrototypeItem : IPrototypeItem {
		readonly string name;
		readonly bool stackable;
		readonly int stacksize;

		public string Name { get { return name; } }
		public bool Stackable { get { return stackable; } }
		public int StackSize { get { if (stackable)
					return stacksize;
				else
					return 1;} }
		public IPrototypeEntity EntityOnPlace { get { return DataManagement.Instance.GetEntity (name); } }

		public PrototypeItem (string Name, bool Stackable = true, int StackSize = 64) {
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

		public IPrototypeItem ItemOnPickup { get { return DataManagement.Instance.GetItem (name); } }

		public PrototypeEntity (string Name, string Object = "") {
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

	public class Stack : IStack {
		IPrototypeItem item;
		int size;

		public IPrototypeItem Item { get { return item; } }
		public int Size { get { return size; } }
		public bool IsFull { get { return size >= item.StackSize; } }

		public Stack (IPrototypeItem Item = null, int Size = 0) {
			this.item = Item;
			if (Size > item.StackSize)
				this.size = item.StackSize;
			else
				this.size = Size;
		}

		public int Combine (IStack stack) {
			int total = this.size + stack.Size;
			if (total > this.item.StackSize)
				this.size = item.StackSize;
			else
				this.size = total;
			return total - this.size;
		}

		public bool SetStack (IStack stack) {
			this.item = stack.Item;
			this.size = stack.Size;
			return true;
		}

		public bool SwapStack (ref IStack stack) {
			Stack temp = new Stack();
			temp.SetStack (this);
			this.SetStack (stack);
			stack.SetStack (temp);
			return true;
		}

		public override int GetHashCode () {
			return base.GetHashCode ();
		}

		public override bool Equals (object obj) {
			if (!(obj is Stack))
				return false;
			var other = obj as Stack;
			if (!(this.item == other.Item && this.size == other.Size))
				return false;
			return true;
		}

		public static bool operator ==(Stack x, Stack y) {
			return x.Equals (y);
		}

		public static bool operator !=(Stack x, Stack y) {
			return !x.Equals (y);
		}

		public static implicit operator bool (Stack me) {
			return me != null;
		}
	}
}