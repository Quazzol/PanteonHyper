using Pooling;
using Character;
using UnityEngine;
using Misc;

namespace Level.Obstacle
{
    public class HorizontalMovingObstacle : PooledMonoBehaviour, IObstacle
    {
        public override int InitialPoolSize => 10;
        [SerializeField]
        private float _distance = 2f;
        [SerializeField]
        private float _movementSpeed = 3f;

        private Transform _transform;
        private Vector3 _startingPosition;
        private Vector3 _targetPosition;

        private void Start()
        {
            _transform = transform;
            _startingPosition = _transform.localPosition;
            _movementSpeed *= Random.Range(1f, 1.5f);
            ChangeDirection();
        }

        private void Update()
        {
            if (_targetPosition.AreEqual(_transform.localPosition, .1f))
            {
                ChangeDirection();
            }

            MoveToTarget();
        }

        private void ChangeDirection()
        {
            var x = _targetPosition.x > _startingPosition.x ? _startingPosition.x - _distance : _startingPosition.x + _distance;
            _targetPosition = new Vector3(x, _startingPosition.y, _startingPosition.z);
        }

        private void MoveToTarget()
        {
            var direction = (_targetPosition - _transform.localPosition).normalized;
            _transform.localPosition += _movementSpeed * Time.deltaTime * direction;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<ICharacter>(out var character))
                return;

            character.FallOver();
        }

    }
}