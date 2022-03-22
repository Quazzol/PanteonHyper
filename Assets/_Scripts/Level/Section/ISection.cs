using Pooling;

namespace Level.Section
{
    public interface ISection : IPooledMonoBehaviour
    {
        float Length { get; }
    }
}