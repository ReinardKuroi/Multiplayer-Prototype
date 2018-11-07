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
}