using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PoolOwner
    {
        private static Dictionary<PooledMonoBehaviour, Pool> _pools = null;
        private static GameObject _poolParent = null;

        public static Pool GetPool(PooledMonoBehaviour prefab)
        {
            CreatePools();

            if (_pools.ContainsKey(prefab))
                return _pools[prefab];

            var pool = new GameObject("Pool-" + (prefab as Component).name).AddComponent<Pool>();
            pool.transform.SetParent(_poolParent.transform);
            pool.SetPrefab(prefab);

            pool.GrowPool();
            _pools.Add(prefab, pool);
            return pool;
        }

        private static void CreatePools()
        {
            if (_pools != null)
                return;

            _pools = new Dictionary<PooledMonoBehaviour, Pool>();
            _poolParent = new GameObject("PoolParent");
        }

        public static void ClearPool()
        {
            _pools = null;
            _poolParent = null;
        }
    }
}