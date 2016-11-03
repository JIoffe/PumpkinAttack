using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Interactive
{
    /// <summary>
    /// Represents a behavior of jumping at the player
    /// </summary>
    public class JumpingMonster : MonoBehaviour
    {
        [Tooltip("The time in seconds between hops or jumps")]
        public float jumpInterval = 1.0f;

        [Tooltip("The strength of each hop or jump")]
        public float jumpStrength = 20f;

        [Tooltip("The layer mask that the environment belongs to")]
        public int environmentLayerMask = 31;

        private Transform playerTransform;
        private Rigidbody rigidBody;
        private bool isGrounded = false;
        private float time = 0f;

        //If this object is falling for too long then it probably found its way out of bounds
        //and needs to be disposed of
        private float timeFalling = 0f;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            playerTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //We only want to prepare a new leap if we're not already airborne

            if (!isGrounded)
            {
                //Remove from scene if we are falling for too long
                timeFalling += Time.deltaTime;
                if (timeFalling > 10f)
                    Destroy(gameObject);

                return;
            }
            timeFalling = 0f;
            time += Time.deltaTime;

            if (time >= jumpInterval)
            {
                var playerPos = playerTransform.position;
                var directionToPlayer = (playerPos - transform.position).normalized;
                directionToPlayer += Vector3.up;
                directionToPlayer.Normalize();

                //Add an impulse to simulate hopping
                rigidBody.AddForce(directionToPlayer * jumpStrength, ForceMode.Impulse);

                //Look at the player as well
                transform.LookAt(new Vector3(
                    playerPos.x,
                    transform.position.y,
                    playerPos.z
                ));
                time = 0f;
                isGrounded = false;
            }
            //var playerPos = playerTransform.position;
            //transform.LookAt(
            //    new Vector3(playerPos.x, transform.position.y, playerPos.z)
            //);
        }

        //Check if the character has hit the ground; if so, set "grounded" to true
        void OnCollisionEnter(Collision collision)
        {
            if (!CollisionIsWithEnvironment(collision))
                return;

            if (collision.contacts[0].normal.y >= 0.6f)
                isGrounded = true;
        }

        //Floating free! Clear "grounded"
        //consider when character is jumping .. it will exit collision.
        void OnCollisionExit(Collision collision)
        {
            if (!CollisionIsWithEnvironment(collision))
                return;

            isGrounded = false;
        }

        private bool CollisionIsWithEnvironment(Collision collision)
        {
            return collision.gameObject.layer == environmentLayerMask;
        }
    }
}