using UnityEngine;
using Unity.Entities;

public partial class DOTSBridge : SystemBase
{
    private int particleCount;

    protected override void OnCreate()
    {
        particleCount = 0;
    }

    protected override void OnUpdate()
    {
        EntityQuery query = GetEntityQuery(ComponentType.ReadOnly<Particle>());
        int diff = query.CalculateEntityCount() - particleCount;
        if (diff != 0)
        {
            particleCount += diff;
            EventBus<OnNumberOfParticlesChangedDOTS>.Publish(new OnNumberOfParticlesChangedDOTS(diff));
        }
    }

    public void PublishOnNumberOfParticlesChanged(int pNumberOfParticlesChanged)
    {
        EventBus<OnNumberOfParticlesChangedDOTS>.Publish(new OnNumberOfParticlesChangedDOTS(pNumberOfParticlesChanged));
    }

    public void OnNumberOfParticlesChangedStandard(OnNumberOfParticlesChangedStandard pOnNumberOfParticlesChangedStandard)
    {
        if (SystemAPI.TryGetSingletonRW(out RefRW<ParticleSettings> particleSettings))
        {
            particleSettings.ValueRW.spawnThisFrame += pOnNumberOfParticlesChangedStandard.numberOfParticlesChange;
        }
    }

    protected override void OnStartRunning()
    {
        EventBus<OnNumberOfParticlesChangedStandard>.OnEvent += OnNumberOfParticlesChangedStandard;
    }

    protected override void OnStopRunning()
    {
        EventBus<OnNumberOfParticlesChangedStandard>.OnEvent -= OnNumberOfParticlesChangedStandard;
    }
}
