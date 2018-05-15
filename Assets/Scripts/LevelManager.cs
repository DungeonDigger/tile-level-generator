using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public int width = 100;
    public int height = 100;

    int[,] level;

    private const int CELL_BLOCK = 0;
    private const int CELL_OPEN = 1;
    private const int CELL_TREASURE = 2;
    private const int CELL_ENEMY = 3;

	// Use this for initialization
	void Start () {
        InitializeEmptyLevel();
	}

    private void InitializeEmptyLevel()
    {
        level = new int[width, height];
        for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                level[i,j] = CELL_BLOCK;
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
