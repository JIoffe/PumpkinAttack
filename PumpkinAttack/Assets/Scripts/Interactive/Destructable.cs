using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Interactive
{
    /// <summary>
    /// Destroy on impact with a bullet
    /// </summary>
    public class Destructable : MonoBehaviour
    {
        [Tooltip("The tag objects should have in order to destroy this object")]
        public string destroyerTag = "Bullet";

        [Tooltip("The mimumum number of damage points required to destroy this object")]
        public int minimumDamagePoints = 1;

        [Tooltip("The maximum number of damage points required to destroy this object")]
        public int maximumDamagePoints = 3;

        [Tooltip("Settings for the sound effect(s) to play when this object is destroyed")]
        public AudioClipSettings destructionSoundSettings;

        private int damagePoints = 0;
        private Rigidbody rigidBody;
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            if (minimumDamagePoints < 1)
                minimumDamagePoints = 1;

            damagePoints = Random.Range(minimumDamagePoints, maximumDamagePoints + 1);
        }

        public void OnDamage(DamageInfo damageInfo)
        {
            damagePoints -= damageInfo.amount;

            //Still alive...
            if (damagePoints > 0)
            {
                Knockback(ref damageInfo);
                return;
            }

            BroadcastMessage("OnDestruction", SendMessageOptions.DontRequireReceiver);
        }
        public void OnDestruction()
        {
            //Todo - Incorporate the sound modulation settings without impacting frame rate too much
            if (destructionSoundSettings.HasAudioClip)
                AudioSource.PlayClipAtPoint(destructionSoundSettings.Clip, transform.position);

            Destroy(gameObject);
        }

        private void Knockback(ref DamageInfo damageInfo)
        {
            if (rigidBody == null)
                return;

            rigidBody.AddForceAtPosition(damageInfo.direction * 500f, damageInfo.location);
        }
    }
}