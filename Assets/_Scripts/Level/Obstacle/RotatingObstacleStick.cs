using Character;
using Misc;
using Pooling;
using UnityEngine;

namespace Level.Obstacle
{
    public class RotatingObstacleStick : PooledMonoBehaviour, IObstacle
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<ICharacter>(out var character))
                return;

            var dir = (character.Transform.position - other.contacts[0].point).normalized;
            character.KnockBack(new Vector3(dir.x, 0, dir.z) * ConstValues.KnockBackPower);
        }
    }
}