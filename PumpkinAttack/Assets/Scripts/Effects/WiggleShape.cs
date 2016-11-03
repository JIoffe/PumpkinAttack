using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Effects
{
    /// <summary>
    /// Simply eases a blend shape in and out continuously
    /// </summary>
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class WiggleShape : MonoBehaviour
    {
        [Tooltip("The higher this value, the faster the wiggle")]
        public float wiggleSpeed = 2.0f;

        private SkinnedMeshRenderer meshRenderer;

        void Start()
        {
            meshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        void Update()
        {
            float s = Mathf.Abs(Mathf.Sin(Time.time * wiggleSpeed));
            meshRenderer.SetBlendShapeWeight(0, s * 100f);
        }
    }
}