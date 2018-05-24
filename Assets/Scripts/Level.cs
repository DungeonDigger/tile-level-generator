
using System.Collections.Generic;

/// <summary>
/// A object for saving and loading level tilesets to a file.
/// </summary>
public class Level {

    public int[,] Tiles;

    public Level(int[,] tiles)
    {
        Tiles = tiles;
    }

    public override string ToString()
    {
        var iMax = Tiles.GetLength(0);
        var jMax = Tiles.GetLength(1);
        List<string> rows = new List<string>();

        for(var j = jMax - 1; j >= 0; j--)
        {
            string row = "";
            for (var i = 0; i < iMax; i++)
            {
                row += Tiles[i, j] + " ";
            }
            rows.Add(row);
        }

        return string.Join("\n", rows.ToArray());
    }
}
