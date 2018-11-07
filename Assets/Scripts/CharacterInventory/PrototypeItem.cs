using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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