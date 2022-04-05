## Saints Row IV

# Super Ground Smash Mechanic

## Core Elements

-   Holding the trigger button charges the player’s jump power.
    -   Player model glows during charge state.
-   After releasing the trigger button, the player ascends to the sky.
-   Holding the trigger button again activates aim state where player velocity slows and target crosshair appears on ground.
-   Releasing the trigger button again propels the player to target crosshair.
-   Player smashes into the ground, creating a small explosion.
-   Depending on how high the player jumped, a larger cinematic explosion can occur instead following impact.
-   Nearby environment objects are either blown away or destroyed depending on proximity to impact.

## Implementation Steps

1. Create a test map.
    - I will design a simple city environment using Blender and textures from texture.ninja (may change). I will also design smaller reusable objects to showcase a destructible environment.
2. Create a 3D character controller.
    - I will use the tutorial referenced from the homework assignment to create a basic 3D character controller with movement. I will also add speed dashing functionality to enable dynamic ways of triggering the overall mechanic.
3. Design a charged super jump state.
    - The first state of the overall mechanic. I will create a script that charges the player’s jump power while holding the trigger button, and launching them into the sky after releasing it.
4. Design a ground smash aiming state.
    - The second state of the overall mechanic. Leveraging physics from the character controller, I will slow down the player’s velocity and design a targeting reticle that appears on the ground at the point they are aiming.
5. Design a ground fly-smashing state.
    - The final state of the overall mechanic. I will use physics to propel the player in the direction they aimed at, smashing into the ground.
6. Add explosions that affect the impact environment.
    - I will create different explosion types (small, big; basic spheres for now) that occur depending on the strength of the mechanic’s execution. Using Rigidbody.AddExplosionForce, these explosions will also affect the surrounding environment.
7. Polish with animations, particles and effects.
    - I will add visual feedback for the player’s charging state, a form of motion blur during the fly-smashing state and particle effects for the explosions. I will also incorporate a zoomed-out camera effect for the larger cinematic explosion.
