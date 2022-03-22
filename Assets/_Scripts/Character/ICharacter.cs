using System;
using Misc;
using Pooling;
using UnityEngine;

namespace Character
{
    public interface ICharacter : IPooledMonoBehaviour
    {
        public event Action<ICharacter> Finished;
        public bool IsFinished { get; }
        public Transform Transform { get; }
        public CharacterType CharacterType { get; }
        public void Run();
        public void FallOver();
        public void KnockBack(Vector3 force);
        public void Victory();
    }
}