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
    public int AvailableKeys { get; set; }

    public State(int diggerX, int diggerY, int distanceFromStart, int[,] tiles, int availableKeys)
    {
        DiggerX = diggerX;
        DiggerY = diggerY;
        DistanceFromStart = distanceFromStart;
        Tiles = (int[,])tiles.Clone();
        AvailableKeys = availableKeys;
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

        return new State((int)playerLocation.x, (int)playerLocation.y, dist, LevelManager.instance.GetLevel(), digger.availableKeys);
    }

    /// <summary>
    /// Gets the current state of the world
    /// </summary>
    /// <param name="diggerX">x pos of the digger</param>
    /// <param name="diggerY">y pos of the digger</param>
    /// <returns>The current state</returns>
    public static State GetCurrentState(int diggerX, int diggerY)
    {
        var digger = GameObject.FindObjectOfType<Digger>();
        var dist = LevelManager.instance.GetShortestPathDistance(digger.startX, digger.startY, diggerX, diggerY);

        return new State(diggerX, diggerY, dist, LevelManager.instance.GetLevel(), digger.availableKeys);
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

        return string.Format("x={0};y={1};k={2};t={3}", DiggerX, DiggerY, AvailableKeys, levelString);
    }
}
