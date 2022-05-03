using System.Collections.Generic;
using UnityEngine;

namespace RotatingObjects
{
    public class SpawnObjectPoolController : MonoBehaviour
    {
        [SerializeField] private List<SpawnObject> pool;

        private SpawnConfig spawnConfig;


        public void Init(SpawnConfig spawnConfig)
        {
            this.spawnConfig = spawnConfig;
        }

        public SpawnObject InstantiateSpawnObject(Vector3 position, Transform parent)
        {
            SpawnObject instance;

            if (pool.Count > 0)
            {
                instance = pool[0];
                pool.Remove(instance);
                instance.gameObject.SetActive(true);
                instance.transform.position = position;
                instance.transform.SetParent(parent);
            }
            else
            {
                instance = Instantiate(spawnConfig.spawnObject, position, Quaternion.identity, parent);
            }

            return instance;
        }

        public void Remove(SpawnObject instance)
        {
            instance.gameObject.SetActive(false);
            pool.Add(instance);
        }
    }
}