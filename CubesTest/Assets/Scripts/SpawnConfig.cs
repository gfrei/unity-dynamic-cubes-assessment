using UnityEngine;

namespace RotatingObjects
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "ScriptableObjects/SpawnConfig", order = 1)]
    public class SpawnConfig : ScriptableObject
    {
        [Header("Spawn Area config")]
        public float sphereRadius;
        public int objectCount;
        public float objectAsChildChance;
        public SpawnObject spawnObject;
        [Header("Lifetime")]
        public Vector2 lifetimeRange;
        public float fadeDuration;
        [Header("Size")]
        public Vector2 xSizeRange;
        public Vector2 ySizeRange;
        public Vector2 zSizeRange;
        [Header("Rotation speed")]
        public Vector2 xRotationSpeedRange;
        public Vector2 yRotationSpeedRange;
        public Vector2 zRotationSpeedRange;
        [Header("Color")]
        public Vector2 rColorRange;
        public Vector2 gColorRange;
        public Vector2 bColorRange;
    }
}