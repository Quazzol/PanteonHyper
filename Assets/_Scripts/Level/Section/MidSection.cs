using System.Linq;
using Level.Platform;
using Pooling;

namespace Level.Section
{
    public class MidSection : PooledMonoBehaviour, ISection
    {
        public float Length => _length;

        float _length;

        private void Awake()
        {
            var surfaces = GetComponentsInChildren<IPlatform>();
            _length = surfaces.Sum(q => q.Length);
        }
    }
}