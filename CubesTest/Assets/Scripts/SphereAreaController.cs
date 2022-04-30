using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAreaController : MonoBehaviour
{
    [SerializeField] private float sphereRadius;
    [SerializeField] private int objectCount;
    [SerializeField] private SpawnObject spawnObject;

    private List<SpawnObject> objectsList;


    private void Start()
    {
        objectsList = new List<SpawnObject>();

        StartCoroutine(SpawnCoroutine());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.2f);
        Gizmos.DrawSphere(Vector3.zero, sphereRadius);
    }

    private IEnumerator SpawnCoroutine()
    {
        float frametime = Time.unscaledDeltaTime;
        int i = 0;
        yield return new WaitForEndOfFrame();
        while (i < objectCount)
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
        SpawnObject instance = Instantiate(spawnObject, GetRandomPosition(), Quaternion.identity);
        objectsList.Add(instance);
        instance.SetAreaController(this);
    }

    public void RemoveSpawnObject(SpawnObject instance)
    {
        objectsList.Remove(instance);
        SpawnNewObject();
    }

    private Vector3 GetRandomPosition()
    {
        float polar = Random.Range(0, 360);
        float elevation = Random.Range(-180, 180);
        float radius = Random.Range(0, sphereRadius);

        return SphericalToCartesian(radius, polar, elevation);
    }

    private Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        Vector3 outCart = new Vector3();

        float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);

        return outCart;
    }
}
