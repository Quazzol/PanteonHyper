using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Character
{
    public static class CharacterFactory
    {
        private static List<ICharacter> _aiCharacters = new List<ICharacter>();
        private static List<ICharacter> _playerCharacters = new List<ICharacter>();

        static CharacterFactory()
        {
            _aiCharacters.AddRange(Resources.LoadAll<CharacterBase>("AI"));
            _playerCharacters.AddRange(Resources.LoadAll<CharacterBase>("Player"));
        }

        public static ICharacter CreateCharacter(CharacterType type, Vector3 position)
        {
            switch (type)
            {
                case CharacterType.AI: return _aiCharacters[Random.Range(0, _aiCharacters.Count)].Get<ICharacter>(null, position, Quaternion.identity);
                case CharacterType.Player: return _playerCharacters[Random.Range(0, _playerCharacters.Count)].Get<ICharacter>(null, position, Quaternion.identity);
            }

            throw new System.Exception("Undefined character type");
        }
    }
}