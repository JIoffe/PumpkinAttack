using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Effects
{
    /// <summary>
    /// Giblets are fragments of an object that should appear when an object explodes
    /// </summary>
    public class Giblets : MonoBehaviour
    {
        [System.Serializable]
        public class GibletSettings
        {

            [Tooltip("The minimum number of pieces that will be produced")]
            public int minimumFragments = 5;

            [Tooltip("The maximum number of pieces that will be produced")]
            public int maximumFragments = 10;

            [Tooltip("The amount of force with which gibs will be propelled")]
            public float gibletForce = 2f;
        }

        [Tooltip("The prefab to use for each giblet")]
        public GameObject gibletPrefab;

        [Tooltip("Settings for the behavior of giblets generation")]
        public GibletSettings gibletSettings;
        void OnDestruction()
        {
            var n = Random.Range(gibletSettings.minimumFragments, gibletSettings.maximumFragments);

            var center = transform.position;

            for (int i = 0; i < n; i++)
            {
                var gib = Instantiate(gibletPrefab);

                var direction = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized;

                gib.transform.position = center + direction * 0.25f;
                gib.transform.up = direction;

                var rigidBody = gib.GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    rigidBody.AddForce(direction * gibletSettings.gibletForce, ForceMode.Impulse);
                }
            }
        }
    }
}