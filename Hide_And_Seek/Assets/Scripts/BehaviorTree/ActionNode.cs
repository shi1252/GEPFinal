using System;

public class ActionNode<T> : INode<T>
{
    Func<float, T, BTStatus> _func;

    public ActionNode(Func<float, T, BTStatus> func)
    {
        _func = func;
    }

    public BTStatus Tick(float time, T context)
    {
        return _func(time, context);
    }
}