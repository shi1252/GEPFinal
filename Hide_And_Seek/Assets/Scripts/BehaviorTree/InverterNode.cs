public class InverterNode<T> : INode<T>
{
    INode<T> _node;

    public InverterNode(INode<T> node)
    {
        _node = node;
    }

    public BTStatus Tick(float time, T context)
    {
        BTStatus status = _node.Tick(time, context);
        switch(status)
        {
            case BTStatus.Success:
                return BTStatus.Failed;
            case BTStatus.Failed:
                return BTStatus.Success;
            default:
                return status;
        }
    }
}
