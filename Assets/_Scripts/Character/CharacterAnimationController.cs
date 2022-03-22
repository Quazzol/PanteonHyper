using UnityEngine;
using Misc;
using Particle;

namespace Character
{
    public class CharacterAnimationController
    {
        private Animator _animator;

        public CharacterAnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void Idle()
        {
            SetTrigger(CharacterAnimatorParameter.Idle);
        }

        public void Run()
        {
            SetTrigger(CharacterAnimatorParameter.Run);
        }

        public void Victory()
        {
            ParticleCreator.Instance.PlayCelebrationParticle(_animator.transform.position);
            SetTrigger(CharacterAnimatorParameter.Victory);
        }

        public void Defeat()
        {
            SetTrigger(CharacterAnimatorParameter.Defeat);
        }

        public void Fall()
        {
            SetTrigger(CharacterAnimatorParameter.Fall);
        }

        public void FallOver()
        {
            ParticleCreator.Instance.PlayObstacleParticle(_animator.transform.position);
            SetTrigger(CharacterAnimatorParameter.FallOver);
        }

        private void SetTrigger(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }
    }
}