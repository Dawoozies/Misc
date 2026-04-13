using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Main
{
    public class EggSpawnerAuthoring : MonoBehaviour
    {
        public float spawnRate;
    }
    public struct EggSpawner : IComponentData
    {
        public float spawnNextTime;
        public float spawnTime;
    }
    public class EggSpawnerAuthoringBaker : Baker<EggSpawnerAuthoring>
    {
        public override void Bake(EggSpawnerAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new EggSpawner
            {
                spawnNextTime = math.rcp(authoring.spawnRate),
                spawnTime = math.rcp(authoring.spawnRate),
            });
        }
    }
}