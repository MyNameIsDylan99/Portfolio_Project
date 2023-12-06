# Portfolio Project

This unity project demonstrates many different coding techniques that are widely used in games programming.
This project was part of my games programming studies at SAE Zürich.

## Cellular Automata

The cellular automata scene consists of an editor script that allows the user to create a 3D-Grid
and build blocks (cells) inside that grid. This structure can then be exported as a prefab.
As soon as one of these cells collides with the igniter it starts to burn and the fire spreading
will then be simulated by a cellular automata algorithm.

## Physics Gravity Sphere

This scene demonstrates orbital gravity that is based on newtons law of gravity.
The editor script allows you to spawn in cubes that are effected by gravity, reset the scene, change mass and more.

## AI

This part of the portfolio project shows ai with a simple implementation of a state machine and raycast based
vision detection.
The AI roams around until a player is seen and then starts chasing him.

## Procedural Terrain

This is an editor tool for procedural terrain generation. The generation is based on a perlin noise that is
adjustable with the following parameters:
 
- Editor Preview Level of Detail
- Noise Scale
- Auto Update Inspector
- Octaves
- Persistance
- Lacunarity
- Seed
- Offset
- Use Falloff (Falloff Map wird von der generierten Heightmap subtrahiert)
- Mesh Height Curve
- Mesh Height Multiplier
- Draw Mode
- Normalize Mode
- Regions

It uses multi-threading and multiple lods to ensure optimal perfomance.
This tool is based on Sebastian Lagues Tutorial Series "Procedural Landmass Generation" available on Youtube: https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3 [Accessed on 06.12.23]

## Simulation

In the simulation section I showcase particle systems, visual effects and shaders.

This is the list of particle systems:

- Firefly Particle System
- Snow Particle System
- Rain Particle System

Visual Effects

- Smoke

Shaders:

- Cartoon fire shader
- Ocean shader




