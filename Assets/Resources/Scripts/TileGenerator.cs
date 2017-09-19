using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour {

	public GameObject tilePrefab;
	public Player player;
	public List<TileColumn> TileColumns;

	public int MostNegativeColumn;

	public event EventHandler<TileSpawnedEventArgs> NewTilesSpawned;

	// Use this for initialization
	void Start () {
		TileColumns = new List<TileColumn>();
		for(int x = -10; x < 10; x++) {
			AddNewColumn(x);
		}

		NewTilesSpawned += GameManager.Instance.GetComponent<EnemySpawner>().c_newTilesSpawnedEvent;
	}

	public List<Vector3> AddNewColumn(int x, int y = 0) {
		GameObject newcolumn = new GameObject("C" + x);
		newcolumn.transform.parent = this.transform;
		TileColumn tc = newcolumn.AddComponent<TileColumn>();
		tc.Init(new Vector2(x, y));

		if( TileColumns.Count == 0 || x < TileColumns.First().ColumnPosition ) {
			TileColumns.Insert(0, tc);
			MostNegativeColumn = x;
		}
		else {
			TileColumns.Add(tc);
		}	

		var r = new List<Vector3>();
		for(int i = -10; i < 10; i++) {
			r.Add( new Vector2( x, y+i ) );
		}
		return r;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int rightMostColumn = (int) (player.transform.position.x + 10);
		int leftMostColumn = (int)(player.transform.position.x - 10);

		List<Vector3> NewTiles = new List<Vector3>();

		//columns to the right
		if ( rightMostColumn - MostNegativeColumn > TileColumns.Count - 1 ) {
			//column has never been spawned
			int currentMax = TileColumns.Last().ColumnPosition;
			for(int i = currentMax+1; i <= rightMostColumn; i++) {
				NewTiles.AddRange( AddNewColumn(i, (int)player.transform.position.y) );

				if(i - MostNegativeColumn - 21 >= 0) {
					TileColumn removed = TileColumns [ i - MostNegativeColumn - 21 ];
					if (removed.Spawned ) {
						removed.DespawnColumn();
					}
				}
			}
		}
		else if( !TileColumns [rightMostColumn - MostNegativeColumn].Spawned ) {
			//need to respawn the column
			TileColumns[rightMostColumn - MostNegativeColumn].SpawnColumn((int)player.transform.position.y);

			TileColumn removed = TileColumns[rightMostColumn - MostNegativeColumn - 21];
			if (removed.Spawned) {
				removed.DespawnColumn();
			}
		}

		//columns to the left
		if(  leftMostColumn - MostNegativeColumn < 0 ) {
			//column has never been spawned
			int currentMin = TileColumns.First().ColumnPosition;
			for(int i = currentMin-1; i >= leftMostColumn; i--) {
				NewTiles.AddRange( AddNewColumn(i, (int)player.transform.position.y) );

				TileColumn removed = TileColumns[i - MostNegativeColumn + 21];
				if (removed.Spawned) {
					removed.DespawnColumn();
				}
			}
		}
		else if (!TileColumns[leftMostColumn - MostNegativeColumn].Spawned) {
			//need to respawn the column
			TileColumns[leftMostColumn - MostNegativeColumn].SpawnColumn((int)player.transform.position.y);

			TileColumn removed = TileColumns[leftMostColumn - MostNegativeColumn + 21];
			if (removed.Spawned) {
				removed.DespawnColumn();
			}
		}

		foreach(TileColumn c in TileColumns.Where( tc => tc.Spawned )) {
			NewTiles.AddRange(
				c.ChangeSpawnedTiles((int)player.transform.position.y)
			);
		}

		if(NewTiles.Count > 0) {
			EventHandler<TileSpawnedEventArgs> handler = NewTilesSpawned;
			if (handler != null) {
				handler(this, new TileSpawnedEventArgs(NewTiles));
			}
		}
	}
}
