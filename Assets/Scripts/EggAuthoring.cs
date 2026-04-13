using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Main
{
    public class EggAuthoring : MonoBehaviour
    {
        public float value;
    }
    public struct Egg : IComponentData
    {
        public float value;
    }
    public class EggAuthoringBaker : Baker<EggAuthoring>
    {
        public override void Bake(EggAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new Egg
            {
                value = authoring.value,
            });
        }
    }
}