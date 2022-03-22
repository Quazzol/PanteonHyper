using Pooling;
using UnityEngine;

namespace Particle
{
    public class Particle : PooledMonoBehaviour, IParticle
    {
        private ParticleSystem _system;

        private void Awake()
        {
            _system = GetComponentInChildren<ParticleSystem>();
        }

        public void Play()
        {
            _system.Play();
            DisableWithTime(_system.main.duration);
        }

    }
}