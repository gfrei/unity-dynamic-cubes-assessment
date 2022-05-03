using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RotatingObjects
{
    public class SphereAreaController : MonoBehaviour
    {
        [SerializeField] private SpawnObjectPoolController poolController;
        [SerializeField] private SpawnConfig spawnConfig;

        private List<SpawnObject> objectsList;
        private Transform myTransform;


        private void Start()
        {
            objectsList = new List<SpawnObject>();
            myTransform = transform;

            poolController.Init(spawnConfig);

            for (int i = 0; i < spawnConfig.objectCount; i++)
            {
                SpawnNewObject();
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 1, 0.3f);
            Gizmos.DrawSphere(Vector3.zero, spawnConfig.sphereRadius);
        }

        private void SpawnNewObject()
        {
            SpawnObject instance = poolController.InstantiateSpawnObject(GetRandomPosition(), myTransform);
            instance.Init(this, spawnConfig, myTransform);

            if (Random.Range(0, 1f) <= spawnConfig.objectAsChildChance && objectsList.Count > 0)
            {
                int randomIndex = Random.Range(0, objectsList.Count);
                SpawnObject parent = objectsList[randomIndex];
                parent.AddChild(instance.gameObject.transform);
            }

            objectsList.Add(instance);
        }

        public void RemoveSpawnObject(SpawnObject instance)
        {
            objectsList.Remove(instance);
            poolController.Remove(instance);
            SpawnNewObject();
        }

        private Vector3 GetRandomPosition()
        {
            float polar = Random.Range(0, 360);
            float elevation = Random.Range(-180, 180);
            float radius = Random.Range(0, spawnConfig.sphereRadius);

            return SphericalToCartesian(radius, polar, elevation);
        }

        private Vector3 SphericalToCartesian(float radius, float polar, float elevation)
        {
            float a = radius * Mathf.Cos(elevation);

            float x = a * Mathf.Cos(polar);
            float y = radius * Mathf.Sin(elevation);
            float z = a * Mathf.Sin(polar);

            return new Vector3(x, y, z);
        }
    }
}