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

    void OnDrawGizmos()
    {
        Vector3 pos;

        // Draw the level
        if (level != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Determine the color of the square
                    Color cellColor;
                    switch (level[x, y])
                    {
                        case CELL_OPEN:
                            cellColor = Color.white;
                            break;
                        case CELL_TREASURE:
                            cellColor = Color.yellow;
                            break;
                        case CELL_ENEMY:
                            cellColor = Color.red;
                            break;
                        case CELL_BLOCK:
                        default:
                            cellColor = Color.black;
                            break;
                    }

                    // Render the gizmo at the appropriate location
                    Gizmos.color = cellColor;
                    pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }

        // Draw grid lines
        pos = Camera.current.transform.position;
        Gizmos.color = Color.white;
        for (float y = 0 - 100; y < 0 + 100; y++)
        {
            Gizmos.DrawLine(new Vector3(-1000000f, y, 0), new Vector3(1000000f, y, 0));
        }
        for (float x = 0 - 100; x < 0 + 100; x++)
        {
            Gizmos.DrawLine(new Vector3(x, -1000000f, 0), new Vector3(x, 1000000f, 0));
        }

    }
}
