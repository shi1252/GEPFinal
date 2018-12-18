using System.Collections.Generic;

public class SequenceNode<T> : ICompositeNode<T>
{
    List<INode<T>> _nodes = new List<INode<T>>();

    public BTStatus Tick(float time, T context)
    {
        foreach (INode<T> node in _nodes)
        {
            BTStatus status = node.Tick(time, context);
            if (status != BTStatus.Success)
                return status;
        }

        return BTStatus.Success;
    }

    public void Add(INode<T> node)
    {
        _nodes.Add(node);
    }
}