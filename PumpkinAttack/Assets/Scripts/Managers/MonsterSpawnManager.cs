using UnityEngine;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.Managers
{
    /// <summary>
    /// Spawns monsters around the player. Takes prefabs as input
    /// as well as settings for spawn frequency. This layer chooses
    /// a monster at random with no order or weight. Monsters are spawned at random
    /// around the player, with at least a 40% chance of being in the player's FOV
    /// </summary>
    public class MonsterSpawnManager : MonoBehaviour
    {
        [System.Serializable]
        public class MonsterSpawnSettings
        {
            [Tooltip("Maximum monsters that can exist at once")]
            public int maximumSimultaneousMonsters = 20;

            [Tooltip("The time in seconds inbetween each spawns")]
            public float spawnFrequency = 2f;

            [Tooltip("Minimum distance away to spawn")]
            public float minimumSpawnDistance = 2.5f;

            [Tooltip("Maximum distance away to spawn")]
            public float maxSpawnDistance = 4.5f;

            [Tooltip("The minimum chance that a monster will spawn within the player's FOV")]
            public float minimumChanceToSpawnWithinFOV = 40f;

        }

        [Tooltip("Settings for spawn including frequency and distance")]
        public MonsterSpawnSettings spawnSettings;

        [Tooltip("List of monsters that this system can spawn")]
        public GameObject[] monsterPrefabs;

        private float runtime = 0f;
        private Transform playerTransform;

        void Start()
        {
            playerTransform = Camera.main.transform;
        }
        public void LateUpdate()
        {
            if (monsterPrefabs.Length == 0)
                return;

            if (transform.childCount >= spawnSettings.maximumSimultaneousMonsters)
                return;

            runtime += Time.deltaTime;

            if (runtime < spawnSettings.spawnFrequency)
                return;

            var monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            var monster = Instantiate(monsterPrefab);

            monster.transform.position = GetNewSpawnPosition();
            monster.transform.SetParent(transform);

            runtime = 0;
        }

        private Vector3 GetNewSpawnPosition()
        {
            //Create a ray from the player's perspective and height,
            //randomly cast horizontally
            var origin = playerTransform.position;
            var direction = GetNewSpawnDirection();

            RaycastHit hitInfo;
            if(Physics.SphereCast(origin, 0.5f, direction, out hitInfo, spawnSettings.maxSpawnDistance, ~(1 << 31)))
            {
                return hitInfo.point;
            }

            return origin + direction * spawnSettings.maxSpawnDistance;
        }

        private Vector3 GetNewSpawnDirection()
        {
            if(Random.Range(0f,100f) <= spawnSettings.minimumChanceToSpawnWithinFOV)
            {
                return GetDirectionWithinPlayerFOV();
            }

            return GetRandomDirection();
        }

        private Vector3 GetRandomDirection()
        {
            return new Vector3(
                        Random.Range(-1f, 1f),
                        0f,
                        Random.Range(-1f, 1f)
                    ).normalized;
        }
        private Vector3 GetDirectionWithinPlayerFOV()
        {
            //Assume ~20 degrees for horizontal FOV
            float halfRotation = 10f;
            float pitch = Random.Range(-halfRotation, halfRotation);
            float yaw = Random.Range(-halfRotation, halfRotation);

            Matrix4x4 deviationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(new Vector3(pitch, yaw, 0f)), Vector3.one);

            return deviationMatrix.MultiplyPoint(playerTransform.forward).normalized;
        }
    }
}