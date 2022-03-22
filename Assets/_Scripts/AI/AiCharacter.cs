using Character;
using Level.Platform;
using Misc;
using UnityEngine;

namespace AI
{
    public class AiCharacter : CharacterBase
    {
        public override int InitialPoolSize => 10;
        public override CharacterType CharacterType => CharacterType.AI;
        private AiMovementController _aiMovement;

        protected override ICharacterMovementController CreateMovementController(Transform transform, Rigidbody rigidbody)
        {
            _aiMovement = new AiMovementController(transform, rigidbody, GetComponent<Collider>(), ConstValues.MovementSpeed);
            return _aiMovement;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent<IPlatform>(out var platform))
            {
                _aiMovement.OnPlatform(platform);
            }
        }
    }
}