using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Effects
{
    /// <summary>
    /// Thin layer that triggers a particle system to play as soon as it
    /// is loaded and then removes the system from the screen once it is complete
    /// </summary>
    public class OneShotParticle : MonoBehaviour
    {
        void Start()
        {
            var particleSystem = GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(gameObject, particleSystem.duration);
        }
    }
}