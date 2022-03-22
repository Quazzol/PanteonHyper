using Pooling;
using Character;
using UnityEngine;

namespace Level.Obstacle
{
    public class FixedObstacle : PooledMonoBehaviour, IObstacle
    {
        public override int InitialPoolSize => 30;

        private void OnCollisionEnter(Collision other)
        {

            if (!other.gameObject.TryGetComponent<ICharacter>(out var character))
                return;

            character.FallOver();
        }
    }
}