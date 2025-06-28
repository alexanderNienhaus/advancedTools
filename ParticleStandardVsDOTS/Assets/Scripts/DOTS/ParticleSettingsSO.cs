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

    [Header("Shape")]
    //[Range(0, 90)] public float angle;
    public float radius;
}
