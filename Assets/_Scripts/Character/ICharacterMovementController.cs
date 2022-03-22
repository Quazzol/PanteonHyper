using System;
using UnityEngine;

namespace Character
{
    public interface ICharacterMovementController : IDisposable
    {
        public event Action Fall;
        public bool IsActive { get; }
        public void Move();
        public void KnockBack(Vector3 power);
        public void Reset();
        public void SetActive(bool active);
    }
}