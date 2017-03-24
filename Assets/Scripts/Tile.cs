using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    EMPTY = '.',
    WALL = '0',
    HOUSE = '1' // Might not be used
}

public class Tile {
	
    public TileType tileType { get; set; }
    public Vector2 position { get; set; }
}
