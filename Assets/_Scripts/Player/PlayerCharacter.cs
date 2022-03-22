using Character;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerCharacter : CharacterBase
    {
        public override int InitialPoolSize => 1;
        public override CharacterType CharacterType => CharacterType.Player;

        protected override ICharacterMovementController CreateMovementController(Transform transform, Rigidbody rigidbody)
        {
            return new PlayerMovementController(transform, rigidbody, ConstValues.MovementSpeed);
        }
    }
}