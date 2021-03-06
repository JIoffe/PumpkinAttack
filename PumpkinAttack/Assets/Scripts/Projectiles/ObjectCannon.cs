﻿using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Projectiles
{
    /// <summary>
    /// Extremely simple version of an object cannon that just creates a new instance
    /// and orients it towards the direction of the cannon. In other projects, I implement
    /// an objet cannon with varying spread and other parameters
    /// </summary>
    public class ObjectCannon : MonoBehaviour
    {
        [Tooltip("Prefab to use for the projectiles launched by this cannon")]
        public GameObject projectilePrefab;

        [Tooltip("Settings for sound to use when firing from this cannon")]
        public AudioClipSettings fireSoundSettings;

        private AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void Launch()
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.forward = transform.forward;
            projectile.transform.position = transform.position;

            PlayFireSound();
        }

        private void PlayFireSound()
        {
            fireSoundSettings.Play(audioSource);
        }
    }
}