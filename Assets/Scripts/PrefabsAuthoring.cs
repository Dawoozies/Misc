using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Main
{
    public class PrefabsAuthoring : MonoBehaviour
    {
        public GameObject eggPrefab;
    }
    public struct Prefabs :IComponentData
    {
        public Entity eggPrefab;
    }
    public class PrefabsAuthoringBaker : Baker<PrefabsAuthoring>
    {
        public override void Bake(PrefabsAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.None);
            AddComponent(e, new Prefabs
            {
                eggPrefab = GetEntity(authoring.eggPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}