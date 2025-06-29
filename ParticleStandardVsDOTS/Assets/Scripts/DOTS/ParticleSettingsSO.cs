using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Particle")]
public class ParticleSettingsSO : ScriptableObject
{
    [Header("Particle System")]
    public GameObject prefab;
    public float startDelay;
    public float duration;
    public float maxLifetime;
    public float startSpeed;
    public int maxParticles;
    public uint randomSeed;

    [Header("Emission")]
    public int rateOverTime;
    public int burst;

    [Header("Shape")]
    public float radius;
}
