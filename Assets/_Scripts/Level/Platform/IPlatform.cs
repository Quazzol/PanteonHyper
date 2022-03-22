
using Pooling;
using UnityEngine;

namespace Level.Platform
{
    public interface IPlatform : IPooledMonoBehaviour
    {
        public float Length { get; }
        public float Width { get; }
        public Vector3 Position { get; }
        public bool IsMoving { get; }

    }
}