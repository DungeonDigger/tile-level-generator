﻿using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public int width = 100;
    public int height = 100;
    public static LevelManager instance;
    public GameObject blockTile;
    public GameObject openTile;
    public GameObject treasureTile;
    public GameObject enemyTile;
    public GameObject exitTile;

    int[,] level;

    public const int CELL_BLOCK = 0;
    public const int CELL_OPEN = 1;
    public const int CELL_TREASURE = 2;
    public const int CELL_ENEMY = 3;
    public const int CELL_EXIT = 4;

    // This transform allows us to collect all the tiles
    // under a single parent object
    private Transform levelHolder;
    GameObject[,] placedTiles;

	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        levelHolder = new GameObject("Board").transform;
        InitializeEmptyLevel();
	}

    public int GetTileAt(int x, int y)
    {
        return level[x, y];
    }

    public void SetTileAt(int x, int y, int tile)
    {
        // If the tile has changed, instantiate a new sprite
        // for it
        if(level[x, y] != tile)
        {
            GameObject toInstantiate = null;
            switch(tile)
            {
                case CELL_BLOCK:
                    toInstantiate = blockTile;
                    break;
                case CELL_OPEN:
                    toInstantiate = openTile;
                    break;
                case CELL_TREASURE:
                    toInstantiate = treasureTile;
                    break;
                case CELL_ENEMY:
                    toInstantiate = enemyTile;
                    break;
                case CELL_EXIT:
                    toInstantiate = exitTile;
                    break;
            }

            if(toInstantiate != null)
            {
                // Destroy the old tile, if there is one
                if (placedTiles[x, y] != null)
                    Destroy(placedTiles[x, y]);
                GameObject newTile = Instantiate(toInstantiate,
                    new Vector3(x, y, 0f),
                    Quaternion.identity);
                newTile.transform.SetParent(levelHolder);
                placedTiles[x, y] = newTile;
            }
        }
        level[x, y] = tile;
    }

    /// <summary>
    /// Gets the shortest path distance between two locations in the level
    /// using Dijkstra's algorithm.
    /// 
    /// Precondition is that the starting and ending location must be non-block
    /// tiles (i.e. they must be traversable)
    /// </summary>
    /// <param name="startX">x value of the starting tile</param>
    /// <param name="startY">y value of the starting tile</param>
    /// <param name="destX">x value of the destination tile</param>
    /// <param name="destY">y value of the destination tile</param>
    /// <returns>Shortest path distance between the two points, or -1 if
    /// the destination could not be reached.</returns>
    public int GetShortestPathDistance(int startX, int startY, int destX, int destY)
    {
        List<string> unvisited = new List<string>();
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (level[i, j] != CELL_BLOCK) unvisited.Add(i + "_" + j);

        Dictionary<string, int> distances = new Dictionary<string, int>();

        foreach (var v in unvisited)
        {
            distances[v] = int.MaxValue;
        }
        distances[startX + "_" + startY] = 0;
        var targetLoc = destX + "_" + destY;
        while(unvisited.Count > 0)
        {
            // Find the unvisited node with the shortest distance
            string toVisit = null;
            foreach (var v in unvisited)
            {
                if(toVisit == null || distances[v] < distances[toVisit])
                {
                    toVisit = v;
                }
            }
            unvisited.Remove(toVisit);

            // We've reach our destination! Return the distance
            if (toVisit == targetLoc)
                return distances[toVisit];

            // Visit all of the node's neighbors and update their distances
            int x = int.Parse(toVisit.Split('_')[0]);
            int y = int.Parse(toVisit.Split('_')[1]);
            List<string> neighbors = new List<string>();
            if (x - 1 > 0 && level[x - 1, y] != CELL_BLOCK)
                neighbors.Add(x - 1 + "_" + y);
            if (x + 1 < width && level[x + 1, y] != CELL_BLOCK)
                neighbors.Add(x + 1 + "_" + y);
            if (y - 1 > 0 && level[x, y - 1] != CELL_BLOCK)
                neighbors.Add(x + "_" + (y - 1));
            if (y + 1 < height && level[x - 1, y] != CELL_BLOCK)
                neighbors.Add(x - 1 + "_" + y);

            int altDistance = distances[toVisit] + 1;
            foreach (var neighbor in neighbors)
            {
                if (altDistance < distances[neighbor])
                    distances[neighbor] = altDistance;
            }
        }

        // Our destination was not found
        return -1;
    }

    private void InitializeEmptyLevel()
    {
        level = new int[width, height];
        placedTiles = new GameObject[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                level[i, j] = CELL_BLOCK;
                var newTile = Instantiate(blockTile,
                    new Vector3(i, j, 0f),
                    Quaternion.identity);
                newTile.transform.SetParent(levelHolder);
                placedTiles[i, j] = newTile;
            }
        }      
    }

    public int[,] GetLevel()
    {
        return level;
    }
}
