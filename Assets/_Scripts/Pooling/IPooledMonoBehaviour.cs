using System;
using UnityEngine;

namespace Pooling
{
    public interface IPooledMonoBehaviour
    {
        public int InitialPoolSize { get; }
        public event Action OnDestroyEvent;
        public Transform transform { get; }
        public GameObject gameObject { get; }

        public T Get<T>(bool enable = true) where T : IPooledMonoBehaviour;
        public T Get<T>(Transform parent, bool resetTransform = false, bool enable = true) where T : IPooledMonoBehaviour;
        public T Get<T>(Transform parent, Vector3 relativePosition, Quaternion relativeRotation, bool enable = true) where T : IPooledMonoBehaviour;
        public void DisableWithTime(float time);
    }
}