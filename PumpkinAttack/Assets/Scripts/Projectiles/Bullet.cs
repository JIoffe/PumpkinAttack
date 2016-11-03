using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Projectiles
{
    /// <summary>
    /// A projectile that moves forward. In this example it ignores gravity.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class Bullet : MonoBehaviour
    {
        [Tooltip("Speed of this projectile in m/s")]
        public float speed = 2.0f;

        [Tooltip("Maximum lifespan of the bullet if nothing is struck")]
        public float lifespan = 10f;

        [Tooltip("The prefab to use to create impact effects from this projectile")]
        public GameObject explosionPrefab;

        private float runtime = 0f;

        private SphereCollider sphereCollider;
        private float radius = 0.5f;

        void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            radius = sphereCollider.radius;
        }

        void FixedUpdate()
        {
            //Hypothetically, the bullet may move too fast for the discrete collision checks
            //to actually capture everything, so use a "thick" raycast
            var currentPosition = transform.position;
            var rayDir = transform.forward;
            var d = speed * Time.deltaTime;
            var targetPosition = currentPosition + rayDir * d;

            RaycastHit hitInfo;
            if (Physics.SphereCast(currentPosition, radius, rayDir, out hitInfo, d, ~(1 << 2)))
            {
                var damageInfo = new DamageInfo(1, hitInfo.point, rayDir);
                hitInfo.collider.SendMessage("OnDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
                Explode();
                return;
            }

            transform.position = targetPosition;


            //Bullet went too long without hitting anything. Probably flying out of bounds
            runtime += Time.deltaTime;
            if (runtime >= lifespan)
            {
                Destroy(gameObject);
                return;
            }


        }

        //This helper method is used because the internal collision system will only react to rigid bodies
        private bool hasCollision()
        {
            //Ignore "Ignore Raycast" layer objects
            var colliders = Physics.OverlapSphere(transform.position, radius, ~(1 << 2));
            if (colliders.Length == 0)
                return false;

            foreach (var struckCollider in colliders)
            {
                struckCollider.SendMessage("OnDestruction", SendMessageOptions.DontRequireReceiver);
            }
            return true;
        }

        /// <summary>
        /// Kablooey. Remove from scene and spawn an explosion
        /// </summary>
        private void Explode()
        {
            if (explosionPrefab != null)
            {
                var explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
    }
}