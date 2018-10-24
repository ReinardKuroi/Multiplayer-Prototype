using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : Spawner {

	public override void Spawn () {
		Position = new Vector3 (Random.Range (-5f, 5f), 10f + Random.Range (-1f, 1f), Random.Range (-5f, 5f));
		base.Spawn ();
	}
}