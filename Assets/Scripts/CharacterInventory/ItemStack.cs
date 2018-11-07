using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterInventory{
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
}