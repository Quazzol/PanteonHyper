using System;
using UnityEngine;

namespace Character
{
    public abstract class CharacterMovementControllerBase : ICharacterMovementController
    {
        public event Action Fall;

        public bool IsActive { get; protected set; }

        protected Transform _transform;
        protected Rigidbody _rigidbody;
        protected float _movementSpeed;
        private bool _falling = false;
        private Vector3 _startingPosition;

        public CharacterMovementControllerBase(Transform transform, Rigidbody rigidbody, float movementSpeed)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _movementSpeed = movementSpeed;
            _startingPosition = transform.position;
        }

        public abstract void Dispose();

        public abstract void Move();

        public void KnockBack(Vector3 power)
        {
            _rigidbody.AddForce(power, ForceMode.Impulse);
        }

        public virtual void Reset()
        {
            _transform.position = _startingPosition;
            _rigidbody.velocity = Vector3.zero;
            _falling = false;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        protected void CheckFall()
        {
            if (_falling)
                return;

            if (Math.Abs(_rigidbody.velocity.y) > 10f)
            {
                _falling = true;
                Fall?.Invoke();
            }
        }
    }
}