using UnityEngine;

public class BlackBoard
{
    public float timer;
    public float passedTime;
    public Transform from;
    public Transform to;
    public float speed;
}

public static class NodeBuilder
{
    public static ActionNode<BlackBoard> WaitNode()
    {
        return new ActionNode<BlackBoard>((time, context) => 
        {
            if (context.passedTime >= context.timer)
                return BTStatus.Success;
            context.passedTime += time;
            return BTStatus.Running;
        });
    }

    public static ActionNode<BlackBoard> MoveFromTo()
    {
        return new ActionNode<BlackBoard>((time, context) =>
        {
            context.from.Translate((context.to.position - context.from.position).normalized * context.speed * time);
            if (Vector3.Distance(context.from.position, context.to.position) <= 0.1f)
                return BTStatus.Success;
            return BTStatus.Running;
        });
    }
}