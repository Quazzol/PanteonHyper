using System;
using System.Collections;
using UnityEngine;
using Misc;

namespace Pooling
{
    public abstract class PooledMonoBehaviour : MonoBehaviour, IPooledMonoBehaviour
    {
        public virtual int InitialPoolSize { get { return 5; } }

        public event Action OnDestroyEvent;

        protected virtual void OnDisable()
        {
            this.OnDestroyEvent?.Invoke();
        }

        public T Get<T>(bool enable = true) where T : IPooledMonoBehaviour
        {
            var pool = PoolOwner.GetPool(this);
            var pooledObject = pool.Get<T>();

            if (enable)
                pooledObject.gameObject.SetActive(true);

            return pooledObject;
        }

        public T Get<T>(Transform parent, bool resetTransform = false, bool enable = true) where T : IPooledMonoBehaviour
        {
            var pooledObject = Get<T>(enable);
            pooledObject.transform.SetParent(parent);

            if (resetTransform)
            {
                pooledObject.transform.localPosition = Vector3.zero;
                pooledObject.transform.localRotation = Quaternion.identity;
                pooledObject.transform.localScale = Vector3.one;
            }

            return pooledObject;
        }

        public T Get<T>(Transform parent, Vector3 relativePosition, Quaternion relativeRotation, bool enable = true) where T : IPooledMonoBehaviour
        {
            var pooledObject = Get<T>(enable);
            pooledObject.transform.SetParent(parent);

            pooledObject.transform.localPosition = relativePosition;
            pooledObject.transform.localRotation = relativeRotation;
            pooledObject.transform.localScale = Vector3.one;

            return pooledObject;
        }

        public void DisableWithTime(float seconds)
        {
            StartCoroutine(DisableWithTimeInternal(seconds));
        }

        private IEnumerator DisableWithTimeInternal(float seconds)
        {
            yield return GetWaiter(seconds);
            this.Disable();
        }

        private float _waitTime = 1f;
        private WaitForSeconds _waitForSeconds;

        private WaitForSeconds GetWaiter(float seconds)
        {
            if (_waitForSeconds == null || !_waitTime.AreEqual(seconds))
            {
                _waitForSeconds = new WaitForSeconds(seconds);
                _waitTime = seconds;
            }

            return _waitForSeconds;
        }
    }
}