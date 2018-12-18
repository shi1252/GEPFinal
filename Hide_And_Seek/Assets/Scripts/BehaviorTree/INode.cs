public interface INode<T>
{
    BTStatus Tick(float time, T context);
}