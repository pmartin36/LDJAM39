using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileSpawnedEventArgs : EventArgs {
	public List<Vector3> Positions;

	public TileSpawnedEventArgs() : this( new List<Vector3>() ) { }
	public TileSpawnedEventArgs(List<Vector3> p) {
		Positions = p;
	}
}

