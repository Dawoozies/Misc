using Latios;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Latios.Transforms;
using Latios.Psyshock;
using Latios.Anna;

namespace Main
{
    public partial struct EggCollisionLayer : ICollectionComponent
    {
        public CollisionLayer layer;
        public JobHandle TryDispose(JobHandle inputDeps)
        {
            return layer.IsCreated ? layer.Dispose(inputDeps) : inputDeps;
        }
    }
    public partial struct ScoreCollisionLayer : ICollectionComponent
    {
        public CollisionLayer layer;
        public JobHandle TryDispose(JobHandle inputDeps)
        {
            return layer.IsCreated ? layer.Dispose(inputDeps) : inputDeps;
        }
    }
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct BuildCollisionLayersSystem : ISystem, ISystemNewScene
    {
        LatiosWorldUnmanaged latiosWorld;
        BuildCollisionLayerTypeHandles typeHandles;
        EntityQuery eggQuery;
        EntityQuery scoreQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            latiosWorld = state.GetLatiosWorldUnmanaged();
            eggQuery = state.Fluent().With<Egg>().PatchQueryForBuildingCollisionLayer().Build();
            scoreQuery = state.Fluent().With<ScoreTag>().PatchQueryForBuildingCollisionLayer().Build();
            typeHandles = new BuildCollisionLayerTypeHandles(ref state);
        }
        public void OnNewScene(ref SystemState state)
        {
            latiosWorld.sceneBlackboardEntity.AddOrSetCollectionComponentAndDisposeOld(new EggCollisionLayer());
            latiosWorld.sceneBlackboardEntity.AddOrSetCollectionComponentAndDisposeOld(new ScoreCollisionLayer());
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            typeHandles.Update(ref state);
            var physicsSettings = latiosWorld.GetPhysicsSettings();
            CollisionLayer layer;
            state.Dependency = Physics.BuildCollisionLayer(eggQuery, in typeHandles).WithSettings(physicsSettings.collisionLayerSettings).ScheduleParallel(out layer, Allocator.Persistent, state.Dependency);
            latiosWorld.sceneBlackboardEntity.SetCollectionComponentAndDisposeOld(new EggCollisionLayer
            {
                layer = layer,
            });
            state.Dependency = Physics.BuildCollisionLayer(scoreQuery, in typeHandles).WithSettings(physicsSettings.collisionLayerSettings).ScheduleParallel(out layer, Allocator.Persistent, state.Dependency);
            latiosWorld.sceneBlackboardEntity.SetCollectionComponentAndDisposeOld(new ScoreCollisionLayer
            {
                layer = layer,
            });
        }
    }
}
