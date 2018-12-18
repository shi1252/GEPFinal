public interface ICompositeNode<T> : INode<T>
{
    void Add(INode<T> node);
}