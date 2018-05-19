using UnityEngine;

/// <summary>
/// A digger controller by a human user.
/// </summary>
public class HumanDigger : Digger
{
    /// <summary>
    /// Uses input controls to determine the next action
    /// that should be taken.
    /// </summary>
    /// <returns>The next action to take based on current
    /// input.</returns>
    protected override DiggerAction GetNextAction()
    {
        if (Input.GetButtonDown("Up"))
            return DiggerAction.Up;
        if (Input.GetButtonDown("Down"))
            return DiggerAction.Down;
        if (Input.GetButtonDown("Left"))
            return DiggerAction.Left;
        if (Input.GetButtonDown("Right"))
            return DiggerAction.Right;
        if (Input.GetButtonDown("RoomSmall"))
            return DiggerAction.RoomSmall;
        if (Input.GetButtonDown("RoomMedium"))
            return DiggerAction.RoomMedium;
        if (Input.GetButtonDown("RoomLarge"))
            return DiggerAction.RoomLarge;

        return DiggerAction.None;
    }
}
