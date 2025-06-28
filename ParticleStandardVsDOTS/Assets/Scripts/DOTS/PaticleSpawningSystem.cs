using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct PaticleSpawningSystem : ISystem
{
    private EntityQuery query;
    private ComponentLookup<ParticleSettings> allParticleSettings;
    private ComponentLookup<LocalTransform> allLocalTransforms;
    private Random r;

    [BurstCompile]
    public void OnCreate(ref SystemState pSystemState)
    {
        pSystemState.RequireForUpdate<ParticleSettings>();

        EntityQueryBuilder entityQueryDesc = new(Allocator.Temp);
        entityQueryDesc.WithAll<ParticleSettings>();
        query = pSystemState.GetEntityQuery(entityQueryDesc);
        entityQueryDesc.Dispose();

        allParticleSettings = pSystemState.GetComponentLookup<ParticleSettings>();
        allLocalTransforms = pSystemState.GetComponentLookup<LocalTransform>(true);

        r = new (1);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState pSystemState)
    {
        allParticleSettings.Update(ref pSystemState);
        allLocalTransforms.Update(ref pSystemState);


        SpawnParticles(ref pSystemState);
    }

    [BurstCompile]
    private void SpawnParticles(ref SystemState pSystemState)
    {
        allParticleSettings.Update(ref pSystemState);
        allLocalTransforms.Update(ref pSystemState);
        NativeArray<Entity> allParticleSettingEntities = query.ToEntityArray(Allocator.Temp);

        //NativeArray<ParticleSettings> allParticleSettings = query.ToComponentDataArray<ParticleSettings>(Allocator.Temp);
        //NativeArray<LocalTransform> allLocalTransforms = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
        for (int i = 0; i < allParticleSettingEntities.Length; i++)
        {
            Entity particleSettingsEntity = allParticleSettingEntities[i];
            if (allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.currentDelay < allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.startDelay - allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.currentDelay)
            {
                allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.currentDelay += SystemAPI.Time.DeltaTime;
            } else
            {
                allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.active = true;
            }

            if (!allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.active) continue;

            if (allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.activeTime >= allParticleSettings.GetRefRW(particleSettingsEntity).ValueRO.duration
                || allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.currentParticles > allParticleSettings.GetRefRW(particleSettingsEntity).ValueRO.maxParticles)
            {
                allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.active = false;
                continue;
            }
            allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.activeTime += SystemAPI.Time.DeltaTime;

            float rate = allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.rateOverTime * SystemAPI.Time.DeltaTime;
            float amountToSpawnExact = allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.rateOverTimeRoundRest + rate;
            int amountToSpawn = (int)math.floor(amountToSpawnExact);
            if (amountToSpawn > (int)math.floor(rate)) allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.rateOverTimeRoundRest--;
            allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.rateOverTimeRoundRest += amountToSpawnExact - amountToSpawn;
            amountToSpawn = (int)math.ceil(rate);

            //int amountToSpawn = allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.spawnThisFrame;

            if (amountToSpawn >= 1)
            {
                //pSystemState.World.GetExistingSystemManaged<EventBridge>().PublishOnNumberOfParticlesChanged(amountToSpawn);
                NativeArray<Entity> instiatedEntities = pSystemState.EntityManager.Instantiate(allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.prefab, amountToSpawn, Allocator.Temp);
                allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.currentParticles += amountToSpawn;

                LocalTransform localTransformParticleSettings = allLocalTransforms[particleSettingsEntity];
                //uint randomSeed = particleSettings.randomSeed;

                for (int j = 0; j < amountToSpawn; j++)
                {
                    Entity particleEntity = instiatedEntities[j];

                    pSystemState.EntityManager.SetComponentData(particleEntity,
                        new Particle
                        {
                            currentLifetime = 0,
                            maxLifetime = allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.maxLifetime,
                        }
                    );

                    float2 randomDirectionUnit = r.NextFloat2Direction();
                    float radius = allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.radius;
                    //float randomRadiusBot = radius - (r.NextFloat(0, radius) + r.NextFloat(0, radius) - radius);
                    //float randomRadiusBot = r.NextFloat(0, radius) + r.NextFloat(0, radius);
                    //if (randomRadiusBot > radius)
                    //{
                    //    randomRadiusBot = radius - (r.NextFloat(0, radius) + r.NextFloat(0, radius) - radius);
                    //}
                    float2 randomBotVector = randomDirectionUnit * r.NextFloat(0, radius);
                    float3 randomBotPoint = localTransformParticleSettings.Position + new float3(randomBotVector.x, 0, randomBotVector.y);
                    float3 direction = new(0, 1, 0);

                    pSystemState.EntityManager.SetComponentData(particleEntity,
                        new LocalTransform
                        {
                            Position = randomBotPoint,
                            Rotation = quaternion.identity,
                            Scale = 1,
                        }
                    );

                    float3 velocity = direction * allParticleSettings.GetRefRO(particleSettingsEntity).ValueRO.startSpeed;
                    pSystemState.EntityManager.SetComponentData(particleEntity,
                        new PhysicsVelocity
                        {
                            Linear = velocity,
                        }
                    );
                }
                allParticleSettings.GetRefRW(particleSettingsEntity).ValueRW.spawnThisFrame = 0;
                instiatedEntities.Dispose();
            }            
        }
        //allParticleSettings.Dispose();
        //allLocalTransforms.Dispose();
        allParticleSettingEntities.Dispose();
    }
}



//float3 randomDirectionUnit3D = new (randomDirectionUnit.x, 0, randomDirectionUnit.y);


//float r1 = r.NextFloat(0, particleSettings.radius);
//float r2 = r.NextFloat(0, particleSettings.radius);
//float r3 = r1 + r2;

//float diff = r3 - particleSettings.radius;
//float randomRadiusBot = diff > 0 ? r3 - 2 * diff : r3;
//float randomRadiusBot = 1 + (particleSettings.radius - 1) * math.pow(r.NextFloat(), 2);
//float randomRadiusBot = math.abs(r1 - r2);




//float angleBottom = particleSettings.angle; //180 - 90 - angle
//if (angleBottom > 89.9999f) angleBottom = 89.9999f;
//if (angleBottom < 0.0001f) angleBottom = 0.0001f;
//angleBottom = 90 - angleBottom;
//float distanceExtendedBottom = (particleSettings.radius / math.sin(angleBottom)) * math.sin(particleSettings.angle);
//float radiusTop = ((distanceExtendedBottom + 1) / math.sin(90 - angleBottom)) * math.sin(angleBottom);


//float randomRadiusTop = (randomRadiusBot / particleSettings.radius) * radiusTop;
//float2 randomTopVector = randomDirectionUnit * randomRadiusTop;
//float3 randomTopPoint = localTransformParticleSettings.Position + new float3(randomTopVector.x, 1, randomTopVector.y);

//float3 direction = math.normalize(randomTopPoint - randomBotPoint);






//float rotationAngle = 0;
//weiter draussen (ratio 1) = direction bleibt gleich (rotation angle 0)
//weiter drinnen (ratio 0) = direction 90 grad aufgerichtet (rotation angle 90)


//mehr angle richtung 90 = direction bleibt gleich (rotation angle 0)
//mehr angle richtung 0 = direction 90 grad aufgerichtet (rotation angle 90)

//float ratio = randomRadiusBot / particleSettings.radius;
//rotationAngle = (90 - particleSettings.angle) * ratio; //angle 0 klappt nicht
//rotationAngle = (90 - (particleSettings.angle * ratio)); //angle 90 klappt nicht
//rotationAngle = (90 - (particleSettings.angle * (ratio / (ratio * (particleSettings.angle / 90))))); //angle 90 klappt nicht


//rotationAngle = (90 - (particleSettings.angle * (ratio / (ratio * (particleSettings.angle / 90))))) * (ratio);


//suche  zugehörigen punkt auf großem kreis darüber -> richtungsvektor


//distanceToMiddle - angle
//0                 - up/0
//max               - specifiedAngle




//particleSettings.angle     rotationAngle   ratio
//0                          90              1
//90                         0               0

//float ratio = randomRadiusBot / particleSettings.radius;
//float rotationAngle = 90 - ratio * particleSettings.angle;
//float rotationAngle = 90 * (particleSettings.angle);
///rotationAngle = math.clamp(0, rotationAngle, 90);



//quaternion rotation = quaternion.AxisAngle(math.cross(randomDirectionUnit3D, new (0, 1, 0)), math.TORADIANS * rotationAngle);
//float3 direction = math.mul(rotation, randomDirectionUnit3D);