using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

//[BurstCompile]
public partial struct PaticleSystem : ISystem
{
    private EntityQuery query;
    private ComponentLookup<Particle> allParticles;

    //[BurstCompile]
    public void OnCreate(ref SystemState pSystemState)
    {
        pSystemState.RequireForUpdate<ParticleSettings>();

        EntityQueryBuilder entityQueryDesc = new(Allocator.Temp);
        entityQueryDesc.WithAll<Particle>();
        query = pSystemState.GetEntityQuery(entityQueryDesc);
        entityQueryDesc.Dispose();

        allParticles = pSystemState.GetComponentLookup<Particle>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState pSystemState)
    {
        allParticles.Update(ref pSystemState);
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(pSystemState.WorldUnmanaged);

        NativeArray<Entity> allParticlesEntities = query.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < allParticlesEntities.Length; i++)
        {
            Entity particleEntity = allParticlesEntities[i];
            //if (!pSystemState.EntityManager.Exists(particleEntity) || allParticles.EntityExists(particleEntity)) continue;

            Particle particle = allParticles[particleEntity];
            if (particle.currentLifetime >= particle.maxLifetime)
            {
                //pSystemState.EntityManager.DestroyEntity(particleEntity);
                pSystemState.World.GetExistingSystemManaged<EventBridge>().PublishOnNumberOfParticlesChanged(-1);
                ecb.DestroyEntity(particleEntity);
                continue;
            }
            allParticles.GetRefRW(particleEntity).ValueRW.currentLifetime += SystemAPI.Time.DeltaTime;
        }
        allParticlesEntities.Dispose();
    }
}
