using Character;
using Misc;
using Pooling;
using UnityEngine;

namespace Level.Obstacle
{
    public class RotatingObstacle : PooledMonoBehaviour, IObstacle
    {
        public override int InitialPoolSize => 10;

        [SerializeField]
        private float _rotationSpeed = 70f;
        private Transform _transform;
        private Vector3 _rotationDirection;

        private void Awake()
        {
            _transform = transform;
            _rotationDirection = Random.value >= 0.5 ? Vector3.up : Vector3.down;
            _rotationSpeed *= Random.Range(.7f, 1.5f);
        }

        private void Update()
        {
            _transform.Rotate(Time.deltaTime * _rotationSpeed * _rotationDirection, Space.Self);
        }
    }
}