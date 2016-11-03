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

        private int damagePoints = 0;
        private Rigidbody rigidBody;
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            if (minimumDamagePoints < 1)
                minimumDamagePoints = 1;

            damagePoints = Random.Range(minimumDamagePoints, maximumDamagePoints + 1);
        }
        //void OnCollisionEnter(Collision collision)
        //{
        //    if (!collision.collider.tag.Equals(destroyerTag))
        //        return;

        //    BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        //}

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