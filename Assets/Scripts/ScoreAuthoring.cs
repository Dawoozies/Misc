using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Main
{
    public class ScoreAuthoring : MonoBehaviour
    {

    }

    public struct ScoreTag : IComponentData
    {
    }
    public class ScoreAuthoringBaker : Baker<ScoreAuthoring>
    {
        public override void Bake(ScoreAuthoring authoring)
        {
            var e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<ScoreTag>(e);
        }
    }
}
