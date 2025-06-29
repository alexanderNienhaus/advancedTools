<h1 align="center">
  Performance of Particle Systems
</h1>

<h2 align="left">EP</h4>  

Implement particle effects using Unity's default particle system component versus DOTS and compare FPS using different parameters.

<h2 align="left">Brief Description</h4>

Particle effects are used in many video games to add "juice" to the game, increasing the visual fidelity. Even if the gameplay would stay the same without particles for most of their use cases, they still improve the enjoyment of a game dramatically by making the visuals much more entertaining.

Particles are just several objects that get different, often random, values assigned. For this demo, I will limit this to:
- Emission rate per second
- Burst emission
- Start position
- Scale
- Rotation
- Velocity
- Lifetime

The number of particles and their behaviour influence the performance of the game.

This prototype is based on multiple tutorials: 
 - <a href="https://unitycodemonkey.com/video.php?v=4ZYn9sR3btg">Getting started with Unity DOTS! (ECS, Job System, Burst, Hybrid Game Objects)</a> by <a href="https://unitycodemonkey.com/index.php">CodeMonkey</a>
 -  <a href="https://www.youtube.com/watch?v=6bVcLSZWqK8">Unity Made Easy - Creating Text Files</a> by <a href="https://www.youtube.com/@firebraingames">Fire Brain Games (Creagines)</a>
 - <a href="https://www.youtube.com/watch?v=0HKSvT2gcuk&t=381s">Learn EVERYTHING About Particles in Unity | Easy Tutorial</a> by <a href="https://www.youtube.com/@sasquatchbgames">Sasquatch B Studios</a>

<h2 align="left">Relevance</h4>
Particle effects are widely used in modern video games as a way to increase visual fidelity (Aho, 2018). Particle effects started in movies, the earliest being Star Trek II: The Wrath of Khan from 1982, which started the conversation about the topic (Buse, 2021). One of the earliest usages in video games was Rebel Assault from 1993 (Lee, 2025). Today, particle effects are a staple of visual design in the Unity engine (Nerima, 2024).

- Aho, J. (2018). REWARDING PLAYERS WITH AUDIOVISUAL CUES. 
- Buse, K. (2021). Genesis Effects: Growing Planets in 1980s Computer Graphics. Johns Hopkins University Press.
- Lee, S. (2025). www.numberanalytics.com. Retrieved from Mastering Particle Systems in Game Animation: https://www.numberanalytics.com/blog/   mastering-particle-systems-in-game-animation#:~:text=History%20and%20Evolution%20of%20Particle,dynamics%2C%20and%20soft%20body%20simulations.
- Nerima, W. C. (2024). Video Game Special Effects Simulation Based on Particle System of Unity. 


<h2 align="left">Comparing Number of Particles</h4>

For this chart, I tested two different systems under the same workload. The first system is Unity's standard particle component, the second one is my own, using Unity DOTS. The workload is the number of particles that increases over time. The particles are being spawned in a circle in a random distribution around a point. Each particle gets assigned the same speed and direction. Every frame, more particles are being instantiated. The radius of the spawn circle is 20, the speed is 5, and the direction of movement is upwards.

For the standard Unity implementation, I used the <a href="https://docs.unity3d.com/ScriptReference/ParticleSystem.html">ParticleSystem</a> component from Unity.

For the DOTS implementation, I created an <a href="https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-isystem.html">ISystem</a> component that spawns Particle <a href="https://docs.unity3d.com/Packages/com.unity.entities@1.4/manual/concepts-entities.html">Entities</a>.

<p align="center">
  <img src="/Chart1.png" width="2879">
</p>

<h3 align="left">Analysis</h3>
On the x-axis, we have the increasing number of particles and on the y-axis, the FPS. You can see that the blue graph, representing the DOTS implementation, is always (except for a very small number of particles) more performant than the orange graph, representing the standard Unity implementation.

The blue graph starts off high at over 200 FPS before falling slowly with increasing particle numbers. The curve falls logarithmically. It relates to the complexity O(logn) in big O notation (function: y = -50,6ln(x) + 615,69).

The orange graph starts at a similar height, but it performs a very steep initial drop, after which it falls off more slowly, almost linearly. This relates to the complexity O(1) in big O notation (function: y = 58929x-1,006).

In theory, big O notation suggests that the blue graph has a worse performance behaviour than the orange one, since the logarithmic reduction of its values is more rapid than the almost constant one of the orange graph. But since this is only applicable for sufficiently large numbers, the practical results are different. Because the orange graph has such a steep drop-off right at the beginning, the advantage of its shallower curve for higher values is diminished, making the DOTS variant the more performant one.

<h2 align="left">Comparing Number of Velocity Changes</h4>

For this chart, I tested the same system under different workloads. The system is my particle implementation using Unity DOTS. The changing workloads are an increasing(doubling) number of particles that get instantiated at once, to test the system's resiliency for burst instantiation. The underlying implementation stays the same as before.

<p align="center">
  <img src="/Chart2.png" width="3533">
</p>

<h3 align="left">Analysis</h3>
On the x-axis, we have the time in seconds and on the y-axis, the FPS. The different colours of graphs relate to different amounts of particles being instantiated at once. You can see that all graphs behave more or less the same until right before the one-second mark. At this moment, the burst instantiation happens. We can see that all graphs perform a significant drop in performance, before most stabilise at a lower level than the performance they had before.

When looking at the bright blue graph (8k particles), we can see that it drops almost 250 FPS to a little over 50. Out of all graphs, this is the smallest drop compared to its initial performance, but the biggest one compared to its later performance of around 160 FPS. It is also the quickest to recover, taking around 0.2 seconds.

For the yellow graph, this trend is reversed; it performs a much steeper drop with a lower performance increase afterwards. The dark blue graph doesn't recover at all; it seems that it reaches the maximum number of particles the system can handle. The other graphs follow the trend and fall between the light blue and the yellow ones.

When analysing the burst instantiation resiliency of the system, we can see that the lower the number of instantiated particles, the bigger the performance drop compared to the eventual recovery. This means that the system does not handle mass instantiation as well as expected for a smaller number of particles. But with rising numbers of particles, the performance drop decreases about the eventual recovery, making this system optimal for high particle numbers.


