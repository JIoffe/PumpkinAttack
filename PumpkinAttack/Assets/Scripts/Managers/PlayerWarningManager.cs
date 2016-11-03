using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;
using JI.Holographic.PumpkinAttack.UI;
using System;

namespace JI.Holographic.PumpkinAttack.Managers
{
    /// <summary>
    /// Managers sending warnings to the player when monsters are out of view
    /// </summary>
    public class PlayerWarningManager : Singleton<PlayerWarningManager>
    {
        public enum PlayerWarningDirection
        {
            Front, Behind, Left, Right
        }
        /// <summary>
        /// This comparer will allow us to sort a list of objects
        /// depending on their closeness to the player view direction
        /// </summary>
        private class ClosenessToPlayerFOVComparer : IComparer
        {
            private Transform playerTransform;
            public ClosenessToPlayerFOVComparer(Transform playerTransform)
            {
                this.playerTransform = playerTransform;
            }
            public int Compare(object a, object b)
            {
                var cmp = OdotP((GameObject)a, playerTransform) - OdotP((GameObject)b, playerTransform);
                if (cmp > 0)
                    return -1;
                else if (cmp < 0)
                    return 1;

                    return 0;
            }
            //Returns the dot product between the player view direction and a ray to the object.
            //The closer this value is to +1, the closer the vectors are to eachother
            private float OdotP(GameObject go, Transform playerTransform)
            {
                var dirToObject = (go.transform.position - playerTransform.position).normalized;

                return Vector3.Dot(dirToObject, playerTransform.forward);
            }
        }
        [Tooltip("How long to wait before warning the player to danger out of FOV")]
        public float warningDelay = 2f;

        [Tooltip("The tag of which to warn the player about")]
        public string targetTag = "Monster";

        private UIManager uiManager;
        private Transform playerTransform;
        private ClosenessToPlayerFOVComparer comparer;

        private float timeCount = 0f;

        void Start()
        {
            uiManager = UIManager.Instance;
            playerTransform = Camera.main.transform;
            comparer = new ClosenessToPlayerFOVComparer(playerTransform);
        }

        void LateUpdate()
        {
            timeCount += Time.deltaTime;
            if(timeCount >= warningDelay)
            {
                ProcessWarning();
                timeCount = 0;
            }
        }

        private void ProcessWarning()
        {
            var potentialHazards = GameObject.FindGameObjectsWithTag(targetTag);

            if (potentialHazards.Length == 0)
                return;

            //Sort hazards by closeness to player view direction
            Array.Sort(potentialHazards, comparer);

            var immediateHazard = potentialHazards[0];

            PlayerWarningDirection direction = GetDirectionOfHazard(immediateHazard);
            WarnForDirection(direction);
        }

        private void WarnForDirection(PlayerWarningDirection direction)
        {
            //Assume the player can see what's in front of them...
            //a potentially dangerous assumption!
            if (direction == PlayerWarningDirection.Front)
                return;

            switch (direction)
            {
                case PlayerWarningDirection.Behind:
                    uiManager.ShowStatusText("Behind you!");
                    break;
                case PlayerWarningDirection.Left:
                    uiManager.ShowStatusText("To your left!");
                    break;
                case PlayerWarningDirection.Right:
                    uiManager.ShowStatusText("To your right!");
                    break;
                default:
                    break;
            }
        }
        private PlayerWarningDirection GetDirectionOfHazard(GameObject go)
        {
            var directionToObject = (go.transform.position - playerTransform.position).normalized;
            float[] directionDotProducts = new float[4];
            directionDotProducts[0] = Vector3.Dot(directionToObject, playerTransform.forward);
            directionDotProducts[1] = Vector3.Dot(directionToObject, -playerTransform.forward);
            directionDotProducts[2] = Vector3.Dot(directionToObject, -playerTransform.right);
            directionDotProducts[3] = Vector3.Dot(directionToObject, playerTransform.right);

            float max = directionDotProducts[0];
            PlayerWarningDirection direction = PlayerWarningDirection.Front;

            for(var i = 1; i < 4; i++)
            {
                if (directionDotProducts[i] > max)
                    direction = (PlayerWarningDirection)i;
            }

            return direction;
        }
    }
}