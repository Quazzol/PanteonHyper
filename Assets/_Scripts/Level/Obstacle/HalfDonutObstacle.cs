using Character;
using Misc;
using Pooling;
using UnityEngine;

namespace Level.Obstacle
{
    public class HalfDonutObstacle : PooledMonoBehaviour, IObstacle
    {
        public override int InitialPoolSize => 10;

        [SerializeField]
        private float _pushSpeed = 8f;
        [SerializeField]
        private float _withdrawSpeed = 1f;
        private float _pushX = -.12f;
        private float _withdrawX = .15f;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private bool _isReversed;
        private Vector3 _targetDirection;
        private bool _pushing = false;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _isReversed = _transform.parent.rotation.y > float.Epsilon;
            _pushSpeed *= Random.Range(1.0f, 2.0f);
            _withdrawSpeed *= Random.Range(1.0f, 2.0f);
            ChangeSigns();
            SetTargetToWithdraw();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            ChangeTarget();
            _rigidbody.MovePosition(_transform.position + Time.fixedDeltaTime * _targetDirection);
        }

        private void ChangeSigns()
        {
            if (_isReversed)
            {
                _pushSpeed *= -1;
                _withdrawSpeed *= -1;
            }
        }

        private void ChangeTarget()
        {
            if (_isReversed)
            {
                if (_transform.localPosition.x >= _withdrawX)
                {
                    SetTargetToPush();
                }
                else if (_transform.localPosition.x <= _pushX)
                {
                    SetTargetToWithdraw();
                }
                return;
            }

            if (_transform.localPosition.x >= _withdrawX)
            {
                SetTargetToPush();
            }
            else if (_transform.localPosition.x <= _pushX)
            {
                SetTargetToWithdraw();
            }
        }

        private void SetTargetToWithdraw()
        {
            _targetDirection = Vector3.right * _withdrawSpeed;
            _pushing = false;
        }

        private void SetTargetToPush()
        {
            _targetDirection = Vector3.left * _pushSpeed;
            _pushing = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            // if (!_pushing)
            //     return;

            if (!other.gameObject.TryGetComponent<ICharacter>(out var character))
                return;

            var dir = (character.Transform.position - other.contacts[0].point).normalized;
            character.KnockBack(new Vector3(dir.x, 0, 0) * ConstValues.KnockBackPower);
        }

    }
}