using System.Collections;
using UnityEngine;

public abstract class Digger : MonoBehaviour {

    public float moveTime = 0.1f; // Time it takes the digger to move to a new position (seconds)

    private Rigidbody2D rb2d;
    private float inverseMoveTime;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}
	
	// Update is called once per frame
	void Update () {
        var nextAction = GetNextAction();
        switch(nextAction)
        {
            case DiggerAction.Up:
            case DiggerAction.Down:
            case DiggerAction.Left:
            case DiggerAction.Right:
                Move(nextAction);
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
    /// <param name="dir"></param>
    /// <returns></returns>
    protected void Move (DiggerAction dir)
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

        // TODO Don't allow the digger to move outside the
        // maximum size of the area

        // Trigger the coroutine to move the agent
        StartCoroutine(SmoothMovement(end));
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
    }
}
