<h1 align="center">
  Comparison of Particle Systems
  <br>
</h1>

<h4 align="center">Standard Unity vs. DOTS</h4>

<h2 align="left">EP</h4>

Implement particle effects using Unitys default particle system component versus DOTS and compare FPS using different parameters.

<h2 align="left">Brief Description</h4>

Particle effects are used in many video games to add "juice" to the game, increasing the visual fidelity. Even if the gameplay would stay the same without particles for most of their use cases, they still improve the enjoyment of a game dramatically by making the visuals much more entertaining.

Particles are just a number of objects that get different, often random, values assigned. For this demo I will limit this to:
- Emission rate per second
- Start position
- Scale
- Rotaion
- Velocity

The amount of particles and their behavior influence the performance of the game.

This prototype is based on multiple tutorials: 
 - <a href="https://unitycodemonkey.com/video.php?v=4ZYn9sR3btg">Getting started with Unity DOTS! (ECS, Job System, Burst, Hybrid Game Objects)</a> by <a href="https://unitycodemonkey.com/index.php">CodeMonkey</a>
 -  <a href="https://www.youtube.com/watch?v=6bVcLSZWqK8">Unity Made Easy - Creating Text Files</a> by <a href="https://www.youtube.com/@firebraingames">Fire Brain Games (Creagines)</a>
 - <a href="https://www.youtube.com/watch?v=0HKSvT2gcuk&t=381s">Learn EVERYTHING About Particles in Unity | Easy Tutorial</a> by <a href="https://www.youtube.com/@sasquatchbgames">Sasquatch B Studios</a>

<h2 align="left">Relevance</h4>
Test

<h2 align="left">Comparing Number of Particles</h4>

The particles are being spawned in a circle in a random distribution around a point. Each particle gets assigned the same speed and direction. Every frame there are more particles bein instantiated. The radius of the spawn circle is 20, the speed is 5 and the direction of movement is upwards.

For the standard Unity implementation, I used the <a href="https://docs.unity3d.com/ScriptReference/ParticleSystem.html">ParticleSystem</a> component from Unity.

For the DOTS implementaion, I created an <a href="https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-isystem.html">ISystem</a> component that spawns Particle <a href="https://docs.unity3d.com/Packages/com.unity.entities@1.4/manual/concepts-entities.html">Entities</a>.

<p align="center">
  <img src="/Chart1.png" width="2879">
</p>

<h2 align="left">Analysis</h4>
On the X axis we have the increasing amount of particles and on the y axis the FPS. You can see that the orange graph, representing the DOTS implementation is always more performant than the blue bar, representing the standard Unity implementation.

The orange graph starts off high at over 250 FPS before falling with increasing particle numbers. The graphs function is an exponential one, meaning that the fall-off is initially more intense, before slowing down. In big O notation this is inverse exponential complexity or O(1/n^x) or exponenial decay.

The blue graph also starts of high but has a very steep initial drop, after which it falls off more slowly, almost linearly. In big O notation this is inverse linear complexity or O(1/n) or linear decay.

In theory, big O notation suggests that the blue graph performs better, since it is more efficient. In practice, the orange graph performs better though, because its initial fall-off is less severe resulting in a higher plateau.

<h2 align="center">Comparing Number of Velocity Changes</h4>

<p align="center">
  <img src="/Chart1.png" width="2879">
</p>

Test

<h2 align="left">Analysis</h4>
Test
