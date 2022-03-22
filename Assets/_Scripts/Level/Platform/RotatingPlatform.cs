using Character;
using Pooling;
using UnityEngine;

namespace Level.Platform
{
    public class RotatingPlatform : PooledMonoBehaviour, IPlatform
    {
        public float Length => _length;
        public float Width => _width;
        public Vector3 Position => _transform.position;
        public bool IsMoving => true;
        public override int InitialPoolSize => 5;

        [SerializeField]
        private float _length = 10f;
        private float _width;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private float _rotationSpeed = 50f;
        private Vector3 _rotationDirection;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rotationDirection = (Random.value >= 0.5 ? Vector3.back : Vector3.forward) * _rotationSpeed;
            _rotationSpeed *= Random.Range(.8f, 1.3f);
        }

        private void OnEnable()
        {
            _width = GetComponent<Collider>().bounds.size.x;
        }

        private void FixedUpdate()
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_rotationDirection * Time.fixedDeltaTime));
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<ICharacter>(out var character))
                return;

            other.transform.parent = _transform;
        }
    }
}