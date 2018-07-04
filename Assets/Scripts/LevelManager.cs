using System.Collections.Generic;
using System.Linq;
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
    public GameObject keyTile;
    public GameObject lockedDoorTile;
    public GameObject enemy;
    public GameObject treasure;
    public GameObject door;
    public GameObject key;
    public GameObject exit;

    int[,] level;

    public const int CELL_BLOCK = 0;
    public const int CELL_OPEN = 1;
    public const int CELL_TREASURE = 2;
    public const int CELL_ENEMY = 3;
    public const int CELL_EXIT = 4;
    public const int CELL_KEY = 5;
    public const int CELL_DOOR = 6;

    // This transform allows us to collect all the tiles
    // under a single parent object
    private Transform levelHolder;
    GameObject[,] placedTiles;
    private List<GameObject> levelItems;

	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        levelHolder = new GameObject("Board").transform;
        InitializeEmptyLevel();
        levelItems = new List<GameObject>();
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
                case CELL_KEY:
                    toInstantiate = keyTile;
                    break;
                case CELL_DOOR:
                    toInstantiate = lockedDoorTile;
                    break;
            }

            if(toInstantiate != null)
            {
                // Don't allow keys to be overwritten, as this could make levels unplayable
                if (level[x, y] == CELL_KEY)
                    return;
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
                neighbors.Add((x - 1) + "_" + y);
            if (x + 1 < width && level[x + 1, y] != CELL_BLOCK)
                neighbors.Add((x + 1) + "_" + y);
            if (y - 1 > 0 && level[x, y - 1] != CELL_BLOCK)
                neighbors.Add(x + "_" + (y - 1));
            if (y + 1 < height && level[x, y + 1] != CELL_BLOCK)
                neighbors.Add(x + "_" + (y + 1));

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

    /// <summary>
    /// Sets up the scene based on a text-file tilemap
    /// </summary>
    /// <param name="levelTileMap">The tilemap to use in assembling the level</param>
    public void SetupScene(TextAsset levelTileMap)
    {
        // Clear out any existing tiles
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (placedTiles[i, j] != null)
                    Destroy(placedTiles[i, j]);
            }
        }

        var levelRows = levelTileMap.text.Split('\n').Where(n => n != null && n != "").ToList();
        var rowCount = levelRows.Count;
        var columnCount = levelRows.First().Split(' ').Where(n => n != null && n != "").ToList().Count;
        level = new int[columnCount, rowCount];
        placedTiles = new GameObject[columnCount, rowCount];

        height = rowCount;
        width = columnCount;
        InitializeEmptyLevel();

        int row = rowCount - 1;
        foreach(var levelRow in levelRows)
        {
            var rowItems = levelRow.Split(' ').ToList();
            for(var col = 0; col < columnCount; col++)
            {
                
                // Only BLOCK, OPEN, and EXIT form the actual tiles
                // of the map. All other "tiles" will be added as separate
                // elements into the scene.
                var tile = int.Parse(rowItems[col]);

                if (tile == CELL_BLOCK)
                {
                    SetTileAt(col, row, CELL_BLOCK);
                }
                else if (tile == CELL_EXIT)
                {
                    SetTileAt(col, row, CELL_EXIT);
                }
                else
                {
                    SetTileAt(col, row, CELL_OPEN);
                }

                if(tile == CELL_ENEMY)
                {
                    levelItems.Add(Instantiate(enemy, new Vector3(col, row, 0f), Quaternion.identity));
                }
                else if(tile == CELL_DOOR)
                {
                    levelItems.Add(Instantiate(door, new Vector3(col, row, 0f), Quaternion.identity));
                }
                else if(tile == CELL_KEY)
                {
                    levelItems.Add(Instantiate(key, new Vector3(col, row, 0f), Quaternion.identity));
                }
                else if(tile == CELL_TREASURE)
                {
                    levelItems.Add(Instantiate(treasure, new Vector3(col, row, 0f), Quaternion.identity));
                }
                else if(tile == CELL_EXIT)
                {
                    levelItems.Add(Instantiate(exit, new Vector3(col, row, 0f), Quaternion.identity));
                }
            }
            row--;
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject go in levelItems)
        {
            Destroy(go);
        }
        for(int i = 0; i < placedTiles.GetLength(0); i++)
        {
            for(int j = 0; j < placedTiles.GetLength(1); j++)
            {
                if(placedTiles[i,j] != null)
                {
                    Destroy(placedTiles[i, j]);
                }
            }
        }
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
