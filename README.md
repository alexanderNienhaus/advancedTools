<h1 align="center">
  Performance of Particle systems
</h1>

<h2 align="left">EP</h4>  

Implement particle effects using Unitys default particle system component versus DOTS and compare FPS using different parameters.

<h2 align="left">Brief Description</h4>

Particle effects are used in many video games to add "juice" to the game, increasing the visual fidelity. Even if the gameplay would stay the same without particles for most of their use cases, they still improve the enjoyment of a game dramatically by making the visuals much more entertaining.

Particles are just a number of objects that get different, often random, values assigned. For this demo I will limit this to:
- Emission rate per second
- Burst emission
- Start position
- Scale
- Rotaion
- Velocity
- Lifetime

The amount of particles and their behavior influence the performance of the game.

This prototype is based on multiple tutorials: 
 - <a href="https://unitycodemonkey.com/video.php?v=4ZYn9sR3btg">Getting started with Unity DOTS! (ECS, Job System, Burst, Hybrid Game Objects)</a> by <a href="https://unitycodemonkey.com/index.php">CodeMonkey</a>
 -  <a href="https://www.youtube.com/watch?v=6bVcLSZWqK8">Unity Made Easy - Creating Text Files</a> by <a href="https://www.youtube.com/@firebraingames">Fire Brain Games (Creagines)</a>
 - <a href="https://www.youtube.com/watch?v=0HKSvT2gcuk&t=381s">Learn EVERYTHING About Particles in Unity | Easy Tutorial</a> by <a href="https://www.youtube.com/@sasquatchbgames">Sasquatch B Studios</a>

<h2 align="left">Relevance</h4>
Test

<h2 align="left">Comparing Number of Particles</h4>

For this chart, I tested two different system under the same workload. The first system Unity's stanadard particle component, the second one is my own, using Unity DOTS. The workload is the number of particles, that increases over time. The particles are being spawned in a circle in a random distribution around a point. Each particle gets assigned the same speed and direction. Every frame there are more particles beeing instantiated. The radius of the spawn circle is 20, the speed is 5 and the direction of movement is upwards.

For the standard Unity implementation, I used the <a href="https://docs.unity3d.com/ScriptReference/ParticleSystem.html">ParticleSystem</a> component from Unity.

For the DOTS implementaion, I created an <a href="https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-isystem.html">ISystem</a> component that spawns Particle <a href="https://docs.unity3d.com/Packages/com.unity.entities@1.4/manual/concepts-entities.html">Entities</a>.

<p align="center">
  <img src="/Chart1.png" width="2879">
</p>

<h3 align="left">Analysis</h3>
On the x-axis we have the increasing amount of particles and on the y-axis the FPS. You can see that the blue graph, representing the DOTS implementation is always (except for a very small number of particles) more performant than the orange graph, representing the standard Unity implementation.

The blue graph starts off high at over 200 FPS before falling slowly with increasing particle numbers. The curve falls logartithmically. It relates to the complexity O(logn) in big O notation.

The orange graph starts off at a similar height but it performs a very steep initial drop, after which it falls off more slowly, almost linearly. This relates to the complexity O(1) in big O notation.

In theory, big O notation suggests that the blue graph has a worse performance than the orange one, since the logarithmic reduction of its values is more rapid than the almost constant one of the orange graph. But since this is only applicable for sufficiently large numbers, the practical results are different. Because the orange graph has such a steep drop off right at the beginning, the advantage of its shallower curve for higher values is diminished, making the DOTS variant the more performant one.

<h2 align="left">Comparing Number of Velocity Changes</h4>

For this chart, I tested the same system under different workloads. The system is my particle implementation using Unity DOTS. The changing workloads are an increasing(doubling) number of particles that get instantiated at once, to test the systems resiliance for burst instantiation. The underlying implementation stays the same as before.

<p align="center">
  <img src="/Chart2.png" width="3533">
</p>

<h3 align="left">Analysis</h3>
On the x-axis we have the time in seconds and on the y-axis the FPS. The different colors of graphs relate two different amounts of particles being instantiated at once. You can see that all graphs behave more or less the same until right before the one second mark. At this moment the burst instantiation happens. We can see that all graphs perform a significant drop in performance, before most stabilize at a lower level than the performance they had before.

When looking at the bright blue graph (8k particles), we can see that it drops almost 250 FPS to a little over 50. Out of all graps, this is the smallest dro compared to its initial performance, but the biggest one compared to its later performance of around 160 FPS. It also is the quickest to recover, taking around 0.2 seconds.

For the yellow graph, this trend is reversed, it performs a much steeper drop with a lower performance increase afterwards. The dark blue graph doesnt recover at all, it seems that it reaches the maximum number of particles the system can handle. The other graphs follow the trend and fall between the ligh blue and the yellow one.

When analysing the burst instantiation resiliance of the system, we can see that the lower the number of instantiated particles, the bigger the performance drop compared to the eventual recovery. This means that the system does not handle mass instantiation as well as expected for smaller number of particles. But with rising numbers of particles, the performance drop decreases in relation to the eventual recovery, making this system optimal for high particle numbers.

