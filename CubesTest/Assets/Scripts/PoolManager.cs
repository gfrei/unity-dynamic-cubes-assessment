using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public SpawnObject instancePrefab;
    public List<SpawnObject> pool;


    public SpawnObject InstantiateSpawnObject(Vector3 position)
    {
        SpawnObject instance;

        if (pool.Count > 0)
        {
            instance = pool[0];
            pool.Remove(instance);
            instance.gameObject.SetActive(true);
            instance.transform.position = position;
        }
        else
        {
            instance = Instantiate(instancePrefab, position, Quaternion.identity);
        }

        return instance;
    }

    public void Remove(SpawnObject instance)
    {
        instance.gameObject.SetActive(false);
        pool.Add(instance);
    }
}
