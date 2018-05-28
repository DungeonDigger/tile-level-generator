/// <summary>
/// Represents a state, action pair in an expert demonstration
/// </summary>
public class Step {

    public DiggerAction Action { get; set; }
    public State State { get; set; }

    public Step(DiggerAction action, State state)
    {
        Action = action;
        State = state;
    }

    public override string ToString()
    {
        return string.Format("{0}   {1}", Action.ToString(), State.ToString());
    }
}
