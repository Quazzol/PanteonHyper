using System.Linq;
using Level.Platform;
using Pooling;

namespace Level.Section
{
    public class StartSection : PooledMonoBehaviour, ISection
    {
        public override int InitialPoolSize => 1;

        public float Length => _length;

        float _length;

        private void Awake()
        {
            var surfaces = GetComponentsInChildren<IPlatform>();
            _length = surfaces.Sum(q => q.Length);
        }
    }
}