using Latios;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Latios.Transforms;

namespace Main
{
    public partial class EggSpawnerSubSystem : SubSystem
    {
        protected override void OnUpdate()
        {
            var sbe = latiosWorld.sceneBlackboardEntity;
            var prefabs = sbe.GetComponentData<Prefabs>();
            var icb = latiosWorld.syncPoint.CreateInstantiateCommandBuffer<WorldTransform>().AsParallelWriter();
            var dt = SystemAPI.Time.DeltaTime;
            var elapsedTime = (float)SystemAPI.Time.ElapsedTime;
            new EggSpawnerJob
            {
                icb = icb,
                eggPrefab = prefabs.eggPrefab,
                elapsedTime = elapsedTime,
            }.ScheduleParallel();
        }
        [BurstCompile]
        partial struct EggSpawnerJob : IJobEntity
        {
            public InstantiateCommandBuffer<WorldTransform>.ParallelWriter icb;
            public Entity eggPrefab;
            public float elapsedTime;
            public void Execute(ref EggSpawner eggSpawner, [ChunkIndexInQuery] int chunkIndexInQuery, WorldTransform transform)
            {
                while(elapsedTime > eggSpawner.spawnNextTime)
                {
                    icb.Add(eggPrefab, transform, chunkIndexInQuery);
                    eggSpawner.spawnNextTime += eggSpawner.spawnTime;
                }
            }
        }
    }
}
