using System;
using System.Collections;
using Level.Section;
using Misc;
using Pooling;
using UnityEngine;

namespace Character
{
    public class CharacterBase : PooledMonoBehaviour, ICharacter
    {
        public event Action<ICharacter> Finished;
        public virtual CharacterType CharacterType => throw new NotImplementedException();
        public Transform Transform => _transform;
        public bool IsFinished { get; private set; }


        private CharacterAnimationController _animator;
        private ICharacterMovementController _movement;
        private CharacterState _state;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private WaitForSeconds _waiter = new WaitForSeconds(2f);

        #region "MonoBehaviour Methods"
        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = new CharacterAnimationController(GetComponentInChildren<Animator>());
        }

        private void Start()
        {
            _movement = CreateMovementController(transform, _rigidbody);
            _movement.Fall += OnFall;
            SetState(CharacterState.Idle);
        }

        private void FixedUpdate()
        {
            if (!_movement.IsActive)
                return;

            _movement.Move();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent<IEndSection>(out var endZone))
            {
                IsFinished = true;
                Finished?.Invoke(this);
                SetState(CharacterState.Victory);
                return;
            }
        }

        private void OnDestroy()
        {
            _movement?.Dispose();
        }
        #endregion

        public void Reset()
        {
            _movement.Reset();

            ResetModel();
        }

        public void Run()
        {
            SetState(CharacterState.Running);
        }

        public void FallOver()
        {
            StartCoroutine(FallingOver());
        }

        public void KnockBack(Vector3 power)
        {
            _movement.KnockBack(power);
        }

        public void Victory()
        {
            SetState(CharacterState.Victory);
        }

        private void Die()
        {
            Reset();
            Run();
        }

        protected virtual ICharacterMovementController CreateMovementController(Transform transform, Rigidbody rigidbody)
        {
            throw new NotImplementedException();
        }

        private void OnFall()
        {
            StartCoroutine(Falling());
        }

        private void ResetModel()
        {
            // when restarted, xbot object position parameters stuck
            var xbot = _transform.GetChild(0);
            xbot.rotation = Quaternion.identity;
            xbot.localPosition = Vector3.zero;
        }

        private void SetState(CharacterState state)
        {
            ResetModel();
            _state = state;

            switch (_state)
            {
                case CharacterState.Idle: _animator.Idle(); _movement.SetActive(false); break;
                case CharacterState.Running: _animator.Run(); _movement.SetActive(true); break;
                case CharacterState.Victory: _animator.Victory(); _movement.SetActive(false); break;
                case CharacterState.Defeat: _animator.Defeat(); _movement.SetActive(false); break;
                case CharacterState.Fall: _animator.Fall(); _movement.SetActive(false); break;
                case CharacterState.FallOver: _animator.FallOver(); _movement.SetActive(false); break;
            }
        }

        private IEnumerator Falling()
        {
            SetState(CharacterState.Fall);
            yield return _waiter;
            Die();
        }

        private IEnumerator FallingOver()
        {
            SetState(CharacterState.FallOver);
            yield return _waiter;
            Die();
        }
    }
}
