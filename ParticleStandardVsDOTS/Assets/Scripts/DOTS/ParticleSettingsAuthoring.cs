using UnityEngine;
using Unity.Entities;
using Unity.Burst;

public class ParticleSettingsAuthoring : MonoBehaviour
{
    [SerializeField] private ParticleSettingsSO particleSettingsSO;

    private class Baker : Baker<ParticleSettingsAuthoring>
    {
        public override void Bake(ParticleSettingsAuthoring pAuthoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ParticleSettings
            {
                active = false,
                activeTime = 0,
                prefab = GetEntity(pAuthoring.particleSettingsSO.prefab, TransformUsageFlags.Dynamic),
                duration = pAuthoring.particleSettingsSO.duration,
                startDelay = pAuthoring.particleSettingsSO.startDelay,
                currentDelay = 0,
                maxLifetime = pAuthoring.particleSettingsSO.maxLifetime,
                startSpeed = pAuthoring.particleSettingsSO.startSpeed,
                currentParticles = 0,
                maxParticles = pAuthoring.particleSettingsSO.maxParticles,
                randomSeed = pAuthoring.particleSettingsSO.randomSeed,
                rateOverTime = pAuthoring.particleSettingsSO.rateOverTime,
                rateOverTimeRoundRest = 0,
                //angle = pAuthoring.particleSettingsSO.angle,
                radius = pAuthoring.particleSettingsSO.radius,
            });
        }
    }
}

[BurstCompile]
public struct ParticleSettings : IComponentData
{
    public bool active;
    public float activeTime;
    public Entity prefab;
    public float duration;
    public float startDelay;
    public float currentDelay;
    public float maxLifetime;
    public float startSpeed;
    public int currentParticles;
    public int maxParticles;
    public uint randomSeed; //todo
    public int rateOverTime;
    public float rateOverTimeRoundRest;
    //public float angle;
    public float radius;
    public int spawnThisFrame;
}
