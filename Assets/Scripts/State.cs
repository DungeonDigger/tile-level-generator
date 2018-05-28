using UnityEngine;
/// <summary>
/// A representation of the world state during the construction of
/// a level
/// </summary>
public class State {
    public int DiggerX { get; set; }
    public int DiggerY { get; set; }
    public int DistanceFromStart { get; set; }

    public State(int diggerX, int diggerY, int distanceFromStart)
    {
        DiggerX = diggerX;
        DiggerY = diggerY;
        DistanceFromStart = distanceFromStart;
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

        return new State((int)playerLocation.x, (int)playerLocation.y, dist);
    }

    public override string ToString()
    {
        return string.Format("x={0},y={1},d={2}", DiggerX, DiggerY, DistanceFromStart);
    }
}
