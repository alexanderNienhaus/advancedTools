using UnityEngine;
using System;

public class ParticleSystemHelper : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private int particleCount;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem == null) throw new Exception("Missing Component: ParticleSystem");
        particleCount = 0;
        EventBus<OnNumberOfParticlesChangedStandard>.Publish(new OnNumberOfParticlesChangedStandard((int)Mathf.Round(particleSystem.emission.rateOverTime.constant * Time.deltaTime)));
    }

    void Update()
    {
        int diff = particleSystem.particleCount - particleCount;
        if (diff != 0){
            particleCount += diff;
            EventBus<OnNumberOfParticlesChangedStandard>.Publish(new OnNumberOfParticlesChangedStandard(diff));
        }
    }
}
