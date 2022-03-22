using Character;
using Pooling;
using UnityEngine;

namespace Level.Platform
{
    public class StaticPlatform : PooledMonoBehaviour, IPlatform
    {
        public float Length => _length;
        public float Width => _width;
        public Vector3 Position => _transform.position;
        public bool IsMoving => false;
        public override int InitialPoolSize => 15;
        [SerializeField]
        private float _length = 18f;
        private float _width;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            _width = GetComponent<Collider>().bounds.size.x;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<ICharacter>(out var character))
                return;

            other.transform.parent = null;
        }
    }
}