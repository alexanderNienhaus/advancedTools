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
                random = new Unity.Mathematics.Random(pAuthoring.particleSettingsSO.randomSeed),
                rateOverTime = pAuthoring.particleSettingsSO.rateOverTime,
                rateOverTimeRoundRest = 0,
                burst = pAuthoring.particleSettingsSO.burst,
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
    public Unity.Mathematics.Random random;
    public int rateOverTime;
    public float rateOverTimeRoundRest;
    public int burst;
    public float radius;
    public int spawnThisFrame;
}
