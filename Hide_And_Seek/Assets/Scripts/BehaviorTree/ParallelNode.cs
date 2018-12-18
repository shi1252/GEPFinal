using System.Collections.Generic;

public class ParallelNode<T> : ICompositeNode<T>
{
    List<INode<T>> _nodes = new List<INode<T>>();

    public BTStatus Tick(float time, T context)
    {
        BTStatus result = BTStatus.Running;
        int count = 0;
        foreach (INode<T> node in _nodes)
        {
            BTStatus status = node.Tick(time, context);
            switch(status)
            {
                case BTStatus.Success:
                    count++;
                    break;
                case BTStatus.Failed:
                    result = BTStatus.Failed;
                    break;
                default:
                    break;
            }
        }

        return result;
    }

    public void Add(INode<T> node)
    {
        _nodes.Add(node);
    }
}
