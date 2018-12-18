using System.Collections.Generic;

public class SelectorNode<T> : ICompositeNode<T>
{
    List<INode<T>> _nodes = new List<INode<T>>();

    public BTStatus Tick(float time, T context)
    {
        foreach (INode<T> node in _nodes)
        {
            BTStatus status = node.Tick(time, context);
            if (status != BTStatus.Failed)
                return status;
        }

        return BTStatus.Failed;
    }

    public void Add(INode<T> node)
    {
        _nodes.Add(node);
    }
}
