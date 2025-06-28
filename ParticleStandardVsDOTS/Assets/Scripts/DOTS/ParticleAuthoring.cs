using UnityEngine;
using Unity.Entities;
using Unity.Burst;

public class ParticleAuthoring : MonoBehaviour
{
    private class Baker : Baker<ParticleAuthoring>
    {
        public override void Bake(ParticleAuthoring pAuthoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Particle
            {
            });
        }
    }
}

[BurstCompile]
public struct Particle : IComponentData
{
    public float currentLifetime;
    public float maxLifetime;
}

