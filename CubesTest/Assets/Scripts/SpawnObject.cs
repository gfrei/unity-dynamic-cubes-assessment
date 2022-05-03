using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [Header("ModelReferences")]
    [SerializeField] private Renderer modelRenderer;
    [SerializeField] private Transform modelTransform;
    [Header("Lifetime")]
    [SerializeField] private Vector2 lifetimeRange;
    [SerializeField] private float fadeDuration;
    [Header("Size")]
    [SerializeField] private Vector2 xSizeRange;
    [SerializeField] private Vector2 ySizeRange;
    [SerializeField] private Vector2 zSizeRange;
    [Header("Rotation")]
    [SerializeField] private Vector2 xRotationSpeedRange;
    [SerializeField] private Vector2 yRotationSpeedRange;
    [SerializeField] private Vector2 zRotationSpeedRange;
    [Header("Colors")]
    [SerializeField] private Vector2 rColorRange;
    [SerializeField] private Vector2 gColorRange;
    [SerializeField] private Vector2 bColorRange;

    private SphereAreaController controller;
    private Color materialColor;
    private Transform mytransform;
    private Vector3 rotationPerFrame;
    private List<Transform> childObjects;

    private void FixedUpdate()
    {
        mytransform.Rotate(rotationPerFrame);
    }

    public void Init(SphereAreaController controller)
    {
        this.controller = controller;
        mytransform = transform;
        childObjects = new List<Transform>();

        modelTransform.localScale = GetRandomSize();

        materialColor = GetRandomColor();
        modelRenderer.material.color = materialColor;

        rotationPerFrame = GetRandomRotation();

        float lifetime = Random.Range(lifetimeRange.x, lifetimeRange.y);
        StartCoroutine(LifecycleCoroutine(lifetime));
    }

    public void AddChild(Transform childTransform)
    {
        childObjects.Add(childTransform);
        childTransform.SetParent(mytransform);
    }

    private Color GetRandomColor()
    {
        float r = Random.Range(rColorRange.x, rColorRange.y);
        float g = Random.Range(gColorRange.x, gColorRange.y);
        float b = Random.Range(bColorRange.x, bColorRange.y);

        return new Color(r, g, b, 1);
    }

    private Vector3 GetRandomSize()
    {
        float x = Random.Range(xSizeRange.x, xSizeRange.y);
        float y = Random.Range(ySizeRange.x, ySizeRange.y);
        float z = Random.Range(zSizeRange.x, zSizeRange.y);

        return new Vector3(x, y, z);
    }

    private Vector3 GetRandomRotation()
    {
        float x = Random.Range(xRotationSpeedRange.x, xRotationSpeedRange.y);
        float y = Random.Range(yRotationSpeedRange.x, yRotationSpeedRange.y);
        float z = Random.Range(zRotationSpeedRange.x, zRotationSpeedRange.y);

        return new Vector3(x, y, z);
    }

    private IEnumerator LifecycleCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        Color targetColor = new Color(materialColor.r, materialColor.g, materialColor.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            modelRenderer.material.color = Color.Lerp(materialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        RemoveChildren();
        controller.RemoveSpawnObject(this);
    }

    private void RemoveChildren()
    {
        foreach(Transform child in childObjects)
        {
            if (child != null)
            {
                child.SetParent(null);
            }
        }
    }
}
