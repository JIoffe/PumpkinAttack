using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Effects
{
    /// <summary>
    /// Script that simply instantiates the defined "spawn effect" prefab
    /// </summary>
    public class SpawnEffect : MonoBehaviour
    {
        [Tooltip("The prefab to instantiate when this object is created")]
        public GameObject spawnEffectPrefab;
        // Use this for initialization
        void Start()
        {
            if (spawnEffectPrefab == null)
                return;

            var effect = Instantiate(spawnEffectPrefab);
            effect.transform.position = transform.position;
        }
    }
}