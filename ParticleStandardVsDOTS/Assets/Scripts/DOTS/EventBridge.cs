using UnityEngine;
using Unity.Entities;

public partial class EventBridge : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
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
