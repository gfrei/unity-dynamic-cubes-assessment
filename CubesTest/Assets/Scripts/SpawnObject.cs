using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RotatingObjects
{
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField] private Renderer modelRenderer;
        [SerializeField] private Transform modelTransform;
        private SpawnConfig spawnConfig;
        private SphereAreaController controller;
        private Color materialColor;
        private Transform mytransform;
        private Vector3 rotationPerFrame;
        private List<Transform> childObjects;

        private void FixedUpdate()
        {
            mytransform.Rotate(rotationPerFrame);
        }

        public void Init(SphereAreaController controller, SpawnConfig spawnConfig)
        {
            this.controller = controller;
            this.spawnConfig = spawnConfig;

            mytransform = transform;
            childObjects = new List<Transform>();

            modelTransform.localScale = GetRandomSize();

            materialColor = GetRandomColor();
            modelRenderer.material.color = materialColor;

            rotationPerFrame = GetRandomRotation();

            float lifetime = Random.Range(spawnConfig.lifetimeRange.x, spawnConfig.lifetimeRange.y);
            StartCoroutine(LifecycleCoroutine(lifetime));
        }

        public void AddChild(Transform childTransform)
        {
            childObjects.Add(childTransform);
            childTransform.SetParent(mytransform);
        }

        private Color GetRandomColor()
        {
            float r = Random.Range(spawnConfig.rColorRange.x, spawnConfig.rColorRange.y);
            float g = Random.Range(spawnConfig.gColorRange.x, spawnConfig.gColorRange.y);
            float b = Random.Range(spawnConfig.bColorRange.x, spawnConfig.bColorRange.y);

            return new Color(r, g, b, 1);
        }

        private Vector3 GetRandomSize()
        {
            float x = Random.Range(spawnConfig.xSizeRange.x, spawnConfig.xSizeRange.y);
            float y = Random.Range(spawnConfig.ySizeRange.x, spawnConfig.ySizeRange.y);
            float z = Random.Range(spawnConfig.zSizeRange.x, spawnConfig.zSizeRange.y);

            return new Vector3(x, y, z);
        }

        private Vector3 GetRandomRotation()
        {
            float x = Random.Range(spawnConfig.xRotationSpeedRange.x, spawnConfig.xRotationSpeedRange.y);
            float y = Random.Range(spawnConfig.yRotationSpeedRange.x, spawnConfig.yRotationSpeedRange.y);
            float z = Random.Range(spawnConfig.zRotationSpeedRange.x, spawnConfig.zRotationSpeedRange.y);

            return new Vector3(x, y, z);
        }

        private IEnumerator LifecycleCoroutine(float time)
        {
            yield return new WaitForSeconds(time);

            Color targetColor = new Color(materialColor.r, materialColor.g, materialColor.b, 0f);

            float elapsedTime = 0f;
            while (elapsedTime < spawnConfig.fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                modelRenderer.material.color = Color.Lerp(materialColor, targetColor, elapsedTime / spawnConfig.fadeDuration);
                yield return null;
            }

            RemoveChildren();
            controller.RemoveSpawnObject(this);
        }

        private void RemoveChildren()
        {
            foreach (Transform child in childObjects)
            {
                if (child != null)
                {
                    child.SetParent(null);
                }
            }
        }
    }
}