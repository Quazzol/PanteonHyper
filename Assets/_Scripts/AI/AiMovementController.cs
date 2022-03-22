using System.Collections.Generic;
using System.Linq;
using Character;
using Level.Obstacle;
using Level.Platform;
using Misc;
using UnityEngine;

namespace AI
{
    public class AiMovementController : CharacterMovementControllerBase
    {
        private Vector3 _movePosition;
        private Vector3 _forward = new Vector3(0, .2f, 1f);
        private Collider _collider;
        private float _characterWidth;
        private LayerMask _colliderLayerMask;
        private PlatformInfo _platformInfo;

        public AiMovementController(Transform transform, Rigidbody rigidbody, Collider collider, float movementSpeed) : base(transform, rigidbody, movementSpeed)
        {
            _collider = collider;
            _characterWidth = _collider.bounds.size.x;
            _colliderLayerMask = LayerMask.GetMask("Obstacle");
            _platformInfo = new PlatformInfo();
        }

        public override void Move()
        {
            if (!IsActive)
                return;

            MoveToPosition();
            CheckFall();
        }

        public void OnPlatform(IPlatform platform)
        {
            _platformInfo.Position = platform.Position;
            _platformInfo.Width = platform.Width;
            _platformInfo.IsRotating = platform.IsMoving;
        }

        private void MoveToPosition()
        {
            MoveForward();
            MoveSides();
            _rigidbody.MovePosition(_movePosition);
        }

        private void MoveForward()
        {
            _movePosition = _transform.position + Time.fixedDeltaTime * _movementSpeed * _forward;
        }

        private void MoveSides()
        {
            if (_platformInfo.IsRotating)
            {
                MoveSides(0);
                return;
            }

            var targetPosition = FindBestPossibleGap();
            MoveSides(targetPosition);
        }

        private void MoveSides(float target)
        {
            if (target.Aproximity(_movePosition.x, .05f))
                return;

            target = Mathf.Clamp(target - _movePosition.x, -_movementSpeed * Time.fixedDeltaTime, _movementSpeed * Time.deltaTime);
            _movePosition.x += target;
        }

        private float FindBestPossibleGap()
        {
            var obstacles = DetectObstacles().ToList();
            if (obstacles.Count() == 0)
                return _movePosition.x;

            var walkableBounds = WalkableBounds();
            var bestPosition = 1000f;
            bool found = false;

            while (obstacles.Count > 0 && !found)
            {
                var rangeSet = new RangeSet();
                rangeSet.Add(walkableBounds.MinValue, walkableBounds.MaxValue);

                foreach (var obstacle in obstacles)
                {
                    rangeSet.Remove(obstacle.bounds.min.x, obstacle.bounds.max.x);
                }

                foreach (var range in rangeSet.Ranges)
                {
                    float rangeGap = range.Gap();
                    if (rangeGap >= _characterWidth)
                    {
                        var lastObstacle = obstacles.Last();
                        var possibleTarget = range.Mid();
                        var direction = new Vector3(possibleTarget, lastObstacle.transform.position.y, lastObstacle.transform.position.z) - _transform.position;

                        if (_rigidbody.SweepTest(direction, out var hit, direction.magnitude))
                        {
                            if (hit.transform.TryGetComponent<IObstacle>(out var obstacle))
                                continue;
                        }

                        if (Mathf.Abs(possibleTarget - _movePosition.x) < Mathf.Abs(bestPosition - _movePosition.x))
                        {
                            bestPosition = possibleTarget;
                            found = true;
                        }
                    }
                }

                obstacles.RemoveAt(obstacles.Count - 1);
            }

            if (bestPosition.AreEqual(1000f))
                bestPosition = 0;

            return bestPosition;
        }

        private IEnumerable<Collider> DetectObstacles()
        {
            var obstacles = Physics.OverlapSphere(_transform.position, 20f, _colliderLayerMask);
            var frontObstacles = new List<Collider>();

            foreach (var obstacle in obstacles)
            {
                if (Vector3.Dot(_transform.TransformDirection(Vector3.forward), obstacle.transform.position - _transform.position) > 0)
                {
                    frontObstacles.Add(obstacle);
                }
            }

            return frontObstacles.OrderBy(q => q.ClosestPoint(_transform.position).z);
        }

        private Range WalkableBounds()
        {
            return new Range(_platformInfo.Position.x - _platformInfo.Width / 2, _platformInfo.Position.x + _platformInfo.Width / 2);
        }

        public override void Dispose()
        {
        }

    }

    public struct PlatformInfo
    {
        public Vector3 Position;
        public float Width;
        public bool IsRotating;
    }
}