using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour {

    #region TileString
    private const string TILE_STRING = @"0000000000000000000000000000
0............00............0
0.0000.00000.00.00000.0000.0
0.0000.00000....00000.0000.0
0.0000.00000.00.00000.0000.0
0............00............0
0.0000.00.00000000.00.0000.0
0.0000.00.00000000.00.0000.0
0......00....00....00......0
000.00.00000.00.00000.00.000
000.00.00000.00.00000.00.000
0...00................00...0
0.0000.00.00000000.00.0000.0
0.0000.00.01111110.00.0000.0
0......00.01111110.00......0
0.0000.00.01111110.00.0000.0
0.0000.00.00000000.00.0000.0
0...00.00..........00.00...0
000.00.00.00000000.00.00.000
000.00.00.00000000.00.00.000
0..........................0
0.0000.00000.00.00000.0000.0
0.0000.00000.00.00000.0000.0
0...00.......00.......00...0
000.00.00.00000000.00.00.000
000.00.00.00000000.00.00.000
0......00....00....00......0
0.0000.00000.00.00000.0000.0
0.0000.00000.00.00000.0000.0
0..........................0
0000000000000000000000000000";
    #endregion TileString

    /// <summary>
    /// This should be the difference between top left of maze and Unity origin
    /// </summary>
    public Vector2 originOffset = Vector2.zero;
    public float speed = 0.3f;

    private GhostTargeter targeter;
    private Vector2 target = Vector2.zero;
    private Vector2 curDir = Vector2.zero;
    private Vector2 dest = Vector2.zero;
    private Tile[,] tiles;
    private float maxDiagonalDistance;

    void Start () {
        target = transform.position;
        dest = transform.position;
        LoadTiles();
        maxDiagonalDistance = Vector2.Distance(Vector2.zero, new Vector2(tiles.GetLength(0), tiles.GetLength(1))); // I think this is the max (should be size of tile grid)
        targeter = GetComponent<GhostTargeter>();
        if (targeter == null) {
            throw new Exception("GameObject is missing a GhostTargeter.");
        }
    }

    void Update () {
        target = targeter.GetTarget();
    }

    void OnDrawGizmos() {
        Gizmos.DrawCube(target, Vector3.one);
    }

    void FixedUpdate() {
        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        if ((Vector2) transform.position == dest) {
            dest = GetNextDest();
            curDir = (dest - (Vector2) transform.position).normalized;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            col.gameObject.GetComponent<PacmanMove>().Die();
        }
    }

    Vector2 GetTilePosition() {
        Vector2 pos = transform.position;
        pos = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        return pos - originOffset;
    }

    /// <summary>
    /// Returns the position of the tile in world space
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    Vector2 GetWorldPosition(Tile tile) {
        return tile.position + originOffset;
    }

    /// <summary>
    /// Gets the tiles surrounding the ghost
    /// </summary>
    /// <returns>Array of Tiles in order Up, Left, Down, Right</returns>
    List<Tile> GetSurroundingTiles() {
        var surroundingTiles = new List<Tile>();

        var tilePos = GetTilePosition();

        surroundingTiles.Add(tiles[(int) tilePos.x,     (int) tilePos.y + 1]); // top
        surroundingTiles.Add(tiles[(int) tilePos.x - 1, (int) tilePos.y]);     // left
        surroundingTiles.Add(tiles[(int) tilePos.x,     (int) tilePos.y - 1]); // down
        surroundingTiles.Add(tiles[(int) tilePos.x + 1, (int) tilePos.y]);     // right

        return surroundingTiles;
    }

    /// <summary>
    /// Returns the next direction for the Ghost
    /// </summary>
    /// <returns>Top, Left, Down, or Right</returns>
    Vector2 GetNextDest() {
        // It's important to preserve the order of validMoves 
        // so that we give the correct priority to up, left, down, right
        var validMoves = GetSurroundingTiles();

        // Remove backwards
        if (curDir == Vector2.up) {
            validMoves.RemoveAt(2);
        } else if (curDir == Vector2.left) {
            validMoves.RemoveAt(3);
        } else if (curDir == Vector2.down) {
            validMoves.RemoveAt(0);
        } else if (curDir == Vector2.right) {
            validMoves.RemoveAt(1);
        }

        validMoves.RemoveAll(t => t.tileType == TileType.WALL);

        // Set curDir
        if (validMoves.Count < 1) { 
            // Less than 2 moves means there's a dead end, and the only move is backwards (which is invalid)
            throw new Exception("wow this probably should not happen, huh.");
        } else if (validMoves.Count > 4) {
            throw new Exception("really I have no clue how this is possible.");
        } else {
            var tileDistance = new List<KeyValuePair<Tile, float>>();
            
            for (int i = 0; i < validMoves.Count; i++) {
                var dist = Vector2.Distance(target, validMoves[i].position);
                tileDistance.Add(new KeyValuePair<Tile, float>(validMoves[i], dist));
            }

            // Get tile closest to target
            var min = new KeyValuePair<Tile, float>(null, maxDiagonalDistance);
            for (int i = 0; i < tileDistance.Count; i++) {
                if (tileDistance[i].Value < min.Value) {
                    min = tileDistance[i];
                }
            }

            if (min.Key == null) {
                throw new Exception("well, guess I was wrong about maxDiagonalDistance.");
            }

            return GetWorldPosition(min.Key);
        }
    }

    bool IsValidMove(Vector2 dir) {
        var pos = GetTilePosition();
        var dest = dir + pos;
        var tile = tiles[(int) dest.x, (int) dest.y];
        return tile.tileType == TileType.EMPTY;
    }

    void LoadTiles() {
        var lines = TILE_STRING.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        tiles = new Tile[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++) {
            var line = lines[y];
            var chars = line.ToCharArray();
            for (int x = 0; x < chars.Length; x++) {
                Tile t = new Tile() {
                    tileType = (TileType) chars[x],
                    position = new Vector2(x, lines.Length - 1 - y)
                };
                tiles[x, lines.Length - 1 - y] = t;
            }
        }
    }
}
