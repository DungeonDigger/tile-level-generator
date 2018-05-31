using UnityEngine;
/// <summary>
/// A representation of the world state during the construction of
/// a level
/// </summary>
public class State {
    public int DiggerX { get; set; }
    public int DiggerY { get; set; }
    public int DistanceFromStart { get; set; }
    public int[,] Tiles { get; set; }

    public State(int diggerX, int diggerY, int distanceFromStart, int[,] tiles)
    {
        DiggerX = diggerX;
        DiggerY = diggerY;
        DistanceFromStart = distanceFromStart;
        Tiles = tiles;
    }

    /// <summary>
    /// Gets the current state of the world
    /// </summary>
    /// <returns>The current state</returns>
    public static State GetCurrentState()
    {
        var digger = GameObject.FindObjectOfType<Digger>();
        var playerLocation = digger.transform.position;
        var dist = LevelManager.instance.GetShortestPathDistance(digger.startX, digger.startY, (int)playerLocation.x, (int)playerLocation.y);

        return new State((int)playerLocation.x, (int)playerLocation.y, dist, LevelManager.instance.GetLevel());
    }

    public override string ToString()
    {
        return string.Format("x={0},y={1},d={2}", DiggerX, DiggerY, DistanceFromStart);
    }

    /// <summary>
    /// Gets a string representation of the complete state, i.e.
    /// the digger location and the entire map
    /// </summary>
    /// <returns>The full state string</returns>
    public string GetFullStateString()
    {
        var levelString = "";
        for(var i = 0; i < Tiles.GetLength(0); i++)
        {
            var rowString = "";
            for(var j = 0; j < Tiles.GetLength(1); j++)
            {
                rowString += (Tiles[i, j] + ",");
            }
            levelString += (rowString.Substring(0, rowString.Length - 1) + "_");
        }
        levelString = levelString.Substring(0, levelString.Length - 1);

        return string.Format("x={0};y={1};t={2}", DiggerX, DiggerY, levelString);
    }
}
