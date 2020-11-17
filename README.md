# Deterministic2DPhysics
Deterministic 2D Physics for C# Console Apps.

This is an early commit to demo how its possible to use Svelto 3.0 in a non-Unity context.

I'd recommend **NOT** using this for anything.

## Dependencies
* Svelto.Common
* Svelto.ECS
* SDL2-CS.Core
* SDL2.dll

## Setup
Clone the repo, I think currently everything required is included.

Run the `FixedMaths.Generator` project, this will create a look up table in `\FixedMaths.Core\ProcessedTableData` which is around 70mb.

Run the `DemoGame` project to see boxes bounding around the screen

## Project Structure
### DemoGame
This contains a super simple simulation setup, will start up SDL2 and bounce some boxes around.

The following key bindings are set:
* Ecs - Closes the program
* ` - Sets simulation speed to 10
* 1 - Sets simulation speed to 1.0
* 2 - Sets simulation speed to 0.5
* 3 - Sets simulation speed to 0.25
* 4 - Sets simulation speed to 0.125
* 5 - Sets simulation speed to 0.01
* 6 - Sets simulation speed to 0.0
* Home - Zooms out
* End - Zooms in
* Arrow keys - pans around

### FixedMaths.Core
This contains a deterministic Q number, and supporting Math helper methods

### FixedMaths.FileSystem
This handles all the filesystem access for the look up tables.

### FixedMaths.Generator
This uses `FixedMaths.FileSystem` to write a look up table to disk.

### Graphics.Core
This contains interfaces which are used in `Thorny.Core`

### Graphics.SDL2Driver
This contains an implementation of `Graphics.Core` using SDL2-CS

### SDL2-CS.Core
This needs removing and adding as a submodule, contains a C# wrapper for SDL2.dll

### Physics.Core
This contains the physics simulation code, its still early in progress, but the following engines have been implemented:
* ApplyVelocityEngine - This applies velocity to the entities
* DetectCollisionsEngine - This preforms an AABB-AABB search for collisions
* ResolveCollisionEngine - This uses the collision data to update the movement direction
* ResolvePenetrationEngine - This uses the penetration data to resolve any penetrations
* ClearPerFrameStateEngine - This clears the collision data
* PositionSyncEngine - This is a proof of concept to sync physics data to an external system

### Thorny.Core
Contains interfaces that `Physics.Core` and `Graphics.SDL2Driver` implements to add that functionality to the "Thorny" engine.
