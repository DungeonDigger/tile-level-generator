using UnityEngine;

/// <summary>
/// A digger controller by a human user.
/// </summary>
public class HumanDigger : Digger
{
    public GameObject aiFriend;

    GameObject activeFriend;

    /// <summary>
    /// Uses input controls to determine the next action
    /// that should be taken.
    /// </summary>
    /// <returns>The next action to take based on current
    /// input.</returns>
    protected override DiggerAction GetNextAction()
    {
        if (Input.GetButton("Up"))
            return DiggerAction.Up;
        if (Input.GetButton("Down"))
            return DiggerAction.Down;
        if (Input.GetButton("Left"))
            return DiggerAction.Left;
        if (Input.GetButton("Right"))
            return DiggerAction.Right;
        if (Input.GetButtonDown("RoomSmall"))
            return DiggerAction.RoomSmall;
        if (Input.GetButtonDown("RoomMedium"))
            return DiggerAction.RoomMedium;
        if (Input.GetButtonDown("RoomLarge"))
            return DiggerAction.RoomLarge;
        if (Input.GetButtonDown("PlaceTreasure"))
            return DiggerAction.PlaceTreasure;
        if (Input.GetButtonDown("PlaceEnemy"))
            return DiggerAction.PlaceEnemy;
        if (Input.GetButtonDown("PlaceExit"))
            return DiggerAction.PlaceExit;
        if (Input.GetButtonDown("PlaceKey"))
            return DiggerAction.PlaceKey;
        if (Input.GetButtonDown("PlaceDoor"))
            return DiggerAction.PlaceDoor;
        if (Input.GetButtonDown("LoadAi"))
        {
            if (activeFriend == null)
            {
                activeFriend = Instantiate(aiFriend, new Vector3(transform.position.x, transform.position.y, 0f),
                Quaternion.identity);
            }
        }
        if (Input.GetButtonDown("UnloadAi"))
        {
            if (activeFriend != null)
            {
                Destroy(activeFriend);
            }
        }

        return DiggerAction.None;
    }
}
