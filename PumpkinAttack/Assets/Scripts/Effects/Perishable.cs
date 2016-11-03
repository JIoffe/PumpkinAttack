using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Effects
{
    /// <summary>
    /// Remove from scene when the lifespan runs out
    /// </summary>
    public class Perishable : MonoBehaviour
    {
        [Tooltip("The time in seconds before this perishable vanishes")]
        public float lifespan = 3.0f;

        public void LateUpdate()
        {
            lifespan -= Time.deltaTime;

            if (lifespan <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}