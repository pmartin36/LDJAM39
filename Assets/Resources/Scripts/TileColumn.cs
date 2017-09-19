using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileColumn : MonoBehaviour{

	public List<Tile> tiles;
	public List<int> PreviouslySpawned;
	List<Color> ColorOptions;

	public bool JustSpawned = true;
	public bool Spawned = true;

	public int ColumnPosition;
	public int index;

	public static Tile TilePrefab;

	// Use this for initialization
	public void Init (Vector2 position) {
		if(TilePrefab == null) {
			TilePrefab = Resources.Load<Tile>("Prefabs/tile");
		}

		tiles = new List<Tile>();
		PreviouslySpawned = new List<int>() { 0 };

		int numColors = Random.Range(1, 3);
		ColorOptions = new List<Color>();
		for (int i = 0; i < numColors; i++) {
			float r = Random.value;
			ColorOptions.Add(new Color(r, r, r));
		}
		ColumnPosition = (int)position.x;

		SpawnColumn((int)position.y);	
	}

	public Color GetColorByDistanceAway(int y) {
		int comparer = Mathf.Max( Mathf.Abs(y), Mathf.Abs(ColumnPosition) );
		if(comparer < 75) {
			return Color.white;
		}
		else if(comparer < 150) {
			return Color.green;
		}
		else if(comparer < 300) {
			return Color.blue;
		}
		else {
			return new Color(0.3f, 0, 0.51f);
		}
	}

	public void DespawnColumn() {
		for(int i = tiles.Count - 1; i >= 0; i--) {
			DespawnTile(i);
		}
		Spawned = false;
	}

	public void SpawnColumn(int player_y) {
		for (int y = -10; y < 10; y++) {
			int yPos = player_y + y;
			SpawnTile( yPos );
		}

		JustSpawned = true;
		Spawned = true;
	}

	public void SpawnTile(int yPos) {
		Tile o = Instantiate<Tile>(TilePrefab, new Vector3(ColumnPosition, yPos, 0), Quaternion.identity, this.transform);
		o.gameObject.name = "R"+yPos;
		o.RowPosition = yPos;
		o.Spawned = true;

		if (tiles.Count == 0 || yPos < tiles.First().RowPosition) {
			tiles.Insert(0, o);
		}
		else {
			tiles.Add(o);
		}

		o.color = ColorOptions[Mathf.Abs(yPos) % ColorOptions.Count] * GetColorByDistanceAway(yPos);
		o.GetComponent<SpriteRenderer>().color = o.color;
	}

	public void DespawnTile( int i ) {
		DestroyImmediate(tiles[i].gameObject);
		tiles.RemoveAt(i);
	}

	public List<Vector3> ChangeSpawnedTiles(int y) {
		List<Vector3> newTiles = new List<Vector3>();
		if(!JustSpawned) {
			int numTilesToSpawn = (y + 10) - (tiles.Last().RowPosition + 1);
			if(numTilesToSpawn > 0) {
				for(int i = tiles.Last().RowPosition + 1; i < y + 10; i++) {
					SpawnTile(i);
					DespawnTile(0);

					if(i > PreviouslySpawned.Last()) {
						PreviouslySpawned.Add(i);
						newTiles.Add( new Vector2(ColumnPosition, i) );
					}
				}
			}
			else {
				numTilesToSpawn = ( y - 10 ) - (tiles.First().RowPosition - 1);
				for (int i = tiles.First().RowPosition - 1; i > y - 10; i--) {
					SpawnTile(i);
					DespawnTile( tiles.Count - 1 );

					if (i < PreviouslySpawned.First()) {
						PreviouslySpawned.Insert(0, i);
						newTiles.Add(new Vector2(ColumnPosition, i));
					}
				}
			}
		}
		//return which tiles have never been spawned before
		return newTiles;
	}
	
	// Update is called once per frame
	void Update () {
		JustSpawned = false;
	}
}
