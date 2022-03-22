using Character;
using InputSystem;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : CharacterMovementControllerBase
    {
        private float _sideMovement;
        private Vector3 _targetPosition;
        private Vector3 _forward = new Vector3(0, .2f, 1f);

        public PlayerMovementController(Transform transform, Rigidbody rigidbody, float movementSpeed) : base(transform, rigidbody, movementSpeed)
        {
            RunnerInputController.Swerve += OnSwerve;
        }

        public override void Reset()
        {
            base.Reset();
            _sideMovement = 0;
        }

        public override void Move()
        {
            if (!IsActive)
                return;

            MoveToPosition();
            CheckFall();
        }

        private void MoveToPosition()
        {
            MoveForward();
            MoveSides();
            _rigidbody.MovePosition(_targetPosition);
        }

        private void MoveForward()
        {
            _targetPosition = _transform.position + Time.fixedDeltaTime * _movementSpeed * _forward;
        }

        private void MoveSides()
        {
            if (!_sideMovement.AreEqual(0))
            {
                _targetPosition.x += _sideMovement;
                _sideMovement = 0;
            }
        }

        private void OnSwerve(float movementPosition)
        {
            if (!IsActive)
                return;

            _sideMovement = Mathf.Clamp(_sideMovement + movementPosition * _movementSpeed, -0.5f, 0.5f);
        }

        public override void Dispose()
        {
            RunnerInputController.Swerve -= OnSwerve;
        }
    }
}