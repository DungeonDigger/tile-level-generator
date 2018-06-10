using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A digger controlled by the computer which
/// executes a sequence of actions in order and then
/// does nothing.
/// </summary>
public class ActionListDigger : Digger
{
    public float waitTime = 0.2f; // Time to wait between actions
    public TextAsset actionFile;

    Queue<DiggerAction> actions;
    float timer = 1f;

    public override void Start()
    {
        var actionStrings = new List<string>();
        actionStrings.AddRange(actionFile.text.Split('\n'));
        var actionEnums = actionStrings.Where(n => n != "")
            .Select(n => (DiggerAction)Enum.Parse(typeof(DiggerAction), n))
            .ToList();
        actions = new Queue<DiggerAction>(actionEnums);

        base.Start();
    }

    protected override DiggerAction GetNextAction()
    {
        timer += Time.deltaTime;
        if(timer >= waitTime)
        {
            timer = 0;
            if (actions.Count > 0)
                return actions.Dequeue();
            return DiggerAction.None;
        }
        return DiggerAction.None;
    }
}
