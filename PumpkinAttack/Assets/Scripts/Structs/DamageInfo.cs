using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack
{
    /// <summary>
    /// Information related to damage dealt to a particular object
    /// </summary>
    [System.Serializable]
    public struct DamageInfo
    {
        public DamageInfo(int amount)
        {
            this.amount = amount;
            this.location = Vector3.zero;
            this.direction = Vector3.zero;
        }

        public DamageInfo(int amount, Vector3 location)
        {
            this.amount = amount;
            this.location = location;
            this.direction = Vector3.zero;
        }

        public DamageInfo(int amount, Vector3 location, Vector3 direction)
        {
            this.amount = amount;
            this.location = location;
            this.direction = direction;
        }

        public int amount;
        public Vector3 location;
        public Vector3 direction;
    }
}