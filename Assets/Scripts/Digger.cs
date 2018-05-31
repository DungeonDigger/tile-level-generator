using System.Collections;
using UnityEngine;

public abstract class Digger : MonoBehaviour {

    public float moveTime = 0.1f; // Time it takes the digger to move to a new position (seconds)
    public int startX = 0;
    public int startY = 0;

    private Rigidbody2D rb2d;
    private float inverseMoveTime;
    // Keeps track of whether the digger is moving so that
    // they can't receive moves while already in motion
    private bool isMoving = false;

    // Keeps track of the number of keys the digger has placed for which no door has
    // yet been placed. This constraints helps ensure playability of the created levels
    private int availableKeys = 0;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        // Ensure the starting point is valid
        if (startX < 0 || startX >= LevelManager.instance.width)
            startX = 0;
        if (startY < 0 || startY >= LevelManager.instance.height)
            startY = 0;
        transform.position = new Vector2(startX, startY);

        // Clear the square that the digger starts on
        LevelManager.instance.SetTileAt(startX, startY, LevelManager.CELL_OPEN);
	}
	
	// Update is called once per frame
	void Update () {
        // Don't process any actions while the digger is moving
        if (isMoving)
            return;
        var nextAction = GetNextAction();
        if (nextAction == DiggerAction.None)
            return;
        var currentState = State.GetCurrentState();
        GameManager.instance.demonstration.Add(new Step(nextAction, currentState));
        switch(nextAction)
        {
            case DiggerAction.Up:
            case DiggerAction.Down:
            case DiggerAction.Left:
            case DiggerAction.Right:
                var newLoc = Move(nextAction);
                break;
            case DiggerAction.RoomSmall:
                CreateRoom(new Vector2(transform.position.x, transform.position.y), 3);
                break;
            case DiggerAction.RoomMedium:
                CreateRoom(new Vector2(transform.position.x, transform.position.y), 5);
                break;
            case DiggerAction.RoomLarge:
                CreateRoom(new Vector2(transform.position.x, transform.position.y), 7);
                break;
            case DiggerAction.PlaceEnemy:
                LevelManager.instance.SetTileAt((int)transform.position.x, (int)transform.position.y, LevelManager.CELL_ENEMY);
                break;
            case DiggerAction.PlaceTreasure:
                LevelManager.instance.SetTileAt((int)transform.position.x, (int)transform.position.y, LevelManager.CELL_TREASURE);
                break;
            case DiggerAction.PlaceExit:
                LevelManager.instance.SetTileAt((int)transform.position.x, (int)transform.position.y, LevelManager.CELL_EXIT);
                break;
            case DiggerAction.PlaceKey:
                availableKeys++;
                LevelManager.instance.SetTileAt((int)transform.position.x, (int)transform.position.y, LevelManager.CELL_KEY);
                break;
            case DiggerAction.PlaceDoor:
                if(availableKeys == 0)
                {
                    GameManager.LogToGui("Another key needs to be placed before creating another locked door");
                    break;
                }
                availableKeys--;
                LevelManager.instance.SetTileAt((int)transform.position.x, (int)transform.position.y, LevelManager.CELL_DOOR);
                break;
        }
	}

    /// <summary>
    /// Returns the next action that should be executed
    /// by the digger.
    /// </summary>
    /// <returns>The next action to execute</returns>
    protected abstract DiggerAction GetNextAction();

    /// <summary>
    /// Moves the digger up, down, left, or right,
    /// clearing a path if it was not already clear.
    /// </summary>
    /// <param name="dir">An action specifying the direction
    /// to move in</param>
    /// <returns>The new location of the digger</returns>
    protected Vector2 Move (DiggerAction dir)
    {
        // Calculate the new location of the digger
        Vector2 end = transform.position;
        switch(dir)
        {
            case DiggerAction.Up:
                end += Vector2.up;
                break;
            case DiggerAction.Down:
                end += Vector2.down;
                break;
            case DiggerAction.Left:
                end += Vector2.left;
                break;
            case DiggerAction.Right:
                end += Vector2.right;
                break;
            default:
                throw new System.Exception(dir + " is not a valid directional action.");
        }

        // Don't allow the digger to move outside the
        // maximum size of the area
        if(end.x < 0 || end.x >= LevelManager.instance.width
            || end.y < 0 || end.y >= LevelManager.instance.height)
        {
            return transform.position;
        }

        // Trigger the coroutine to move the agent
        isMoving = true;
        StartCoroutine(SmoothMovement(end));

        // Clear out the tile if it was a block
        if(LevelManager.instance.GetTileAt((int)end.x, (int)end.y) == LevelManager.CELL_BLOCK)
            LevelManager.instance.SetTileAt((int)end.x, (int)end.y, LevelManager.CELL_OPEN);

        return end;
    }

    /// <summary>
    /// Creates a room centered around a location by clearing out a square area
    /// </summary>
    /// <param name="center">The location that should be at the center of the room</param>
    /// <param name="roomSize">The dimension of the room along each side</param>
    protected void CreateRoom(Vector2 center, int roomSize)
    {
        var left = Mathf.FloorToInt(center.x - (roomSize / 2));
        var right = Mathf.FloorToInt(center.x + (roomSize / 2));
        var top = Mathf.FloorToInt(center.y + (roomSize / 2));
        var bottom = Mathf.FloorToInt(center.y - (roomSize / 2));

        if (left < 0)
            left = 0;
        if (right >= LevelManager.instance.width)
            right = LevelManager.instance.width - 1;
        if (bottom < 0)
            bottom = 0;
        if (top >= LevelManager.instance.height)
            top = LevelManager.instance.height - 1;

        for (int x = left; x <= right; x++)
            for (int y = bottom; y <= top; y++)
                LevelManager.instance.SetTileAt(x, y, LevelManager.CELL_OPEN);
    }

    private void DigTile(int x, int y)
    {
        if(LevelManager.instance.GetTileAt(x, y) == LevelManager.CELL_BLOCK)
        {
            LevelManager.instance.SetTileAt(x, y, LevelManager.CELL_OPEN);
        }
    }

    // Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    // Hoisted from the Unity Roguelike tutorial
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        // Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        Debug.Log(sqrRemainingDistance);
        // While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            // Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);

            // Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2d.MovePosition(newPostion);

            // Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            // Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        isMoving = false;
    }
}
