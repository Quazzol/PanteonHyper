using System.Collections.Generic;
using UnityEngine;
using Misc;

namespace Pooling
{
    public class Pool : MonoBehaviour, IPool
    {
        private Queue<IPooledMonoBehaviour> _objects = new Queue<IPooledMonoBehaviour>();
        private List<IPooledMonoBehaviour> _disabledObjects = new List<IPooledMonoBehaviour>();
        private IPooledMonoBehaviour _prefab = null;

        public T Get<T>() where T : IPooledMonoBehaviour
        {
            if (_objects.Count == 0)
                GrowPool();
            return (T)_objects.Dequeue();
        }

        public void GrowPool()
        {
            for (int i = 0; i < _prefab.InitialPoolSize; i++)
            {
                var pooledObject = Instantiate(_prefab as MonoBehaviour) as IPooledMonoBehaviour;
                (pooledObject as Component).gameObject.name += " " + i;

                pooledObject.OnDestroyEvent += () => AddObjectToAvailable(pooledObject);
                pooledObject.Disable();
            }
        }

        public void SetPrefab(IPooledMonoBehaviour prefab)
        {
            _prefab = prefab;
        }

        private void Update()
        {
            MakeDisabledObjectsChildren();
        }

        private void MakeDisabledObjectsChildren()
        {
            if (_disabledObjects.Count > 0)
            {
                foreach (var pooledObject in _disabledObjects)
                {
                    if (pooledObject.gameObject.activeInHierarchy == false)
                        (pooledObject as Component).transform.SetParent(transform);
                }

                _disabledObjects.Clear();
            }
        }

        private void AddObjectToAvailable(IPooledMonoBehaviour pooledObject)
        {
            _disabledObjects.Add(pooledObject);
            _objects.Enqueue(pooledObject);
        }

    }
}