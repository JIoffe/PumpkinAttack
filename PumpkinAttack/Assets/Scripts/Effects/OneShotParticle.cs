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
        [Tooltip("The sound effect(s) to use for this effect, if any")]
        public AudioClipSettings effectSoundSettings;

        void Start()
        {
            var particleSystem = GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(gameObject, particleSystem.duration);

            PlaySoundEffect();
        }

        private void PlaySoundEffect()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            effectSoundSettings.Play(audioSource);
        }
    }
}