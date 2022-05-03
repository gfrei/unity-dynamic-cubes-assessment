using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RotatingObjects
{
    public class SphereAreaController : MonoBehaviour
    {
        [SerializeField] private SpawnObjectPoolController poolManager;
        [SerializeField] private SpawnConfig spawnConfig;

        private List<SpawnObject> objectsList;


        private void Start()
        {
            objectsList = new List<SpawnObject>();

            poolManager.Init(spawnConfig);

            StartCoroutine(SpawnCoroutine());
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 1, 0.2f);
            Gizmos.DrawSphere(Vector3.zero, spawnConfig.sphereRadius);
        }

        private IEnumerator SpawnCoroutine()
        {
            float frametime = Time.unscaledDeltaTime;
            int i = 0;
            yield return new WaitForEndOfFrame();
            while (i < spawnConfig.objectCount)
            {
                SpawnNewObject();
                i++;

                frametime += Time.unscaledDeltaTime;

                // if (frametime > 0.016f)
                // {
                //     Debug.Log(">> frametime wait for frame: " + frametime);
                //     frametime = 0;
                //     yield return new WaitForEndOfFrame();
                // }
            }
        }

        private void SpawnNewObject()
        {
            SpawnObject instance = poolManager.InstantiateSpawnObject(GetRandomPosition());
            instance.Init(this, spawnConfig);

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
            poolManager.Remove(instance);
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
            Vector3 outCart = new Vector3();

            float a = radius * Mathf.Cos(elevation);
            outCart.x = a * Mathf.Cos(polar);
            outCart.y = radius * Mathf.Sin(elevation);
            outCart.z = a * Mathf.Sin(polar);

            return outCart;
        }
    }
}