namespace Pooling
{
    public interface IPool
    {
        public T Get<T>() where T : IPooledMonoBehaviour;
    }
}