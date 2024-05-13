using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

[BurstCompile]
public partial struct PlayerMovementJob : IJobEntity {

    [ReadOnly]
    public float verticalInput;

    [ReadOnly]
    public float horizontalInput;

    [ReadOnly]
    public bool jumpInput;


    [ReadOnly]
    public float3 cameraForward;

    [ReadOnly]
    public float3 cameraRight;


    [ReadOnly]
    public float deltaTime;


    [ReadOnly]
    public float speed;

    [ReadOnly]
    public float maxVelocity;

    [ReadOnly]
    public float jumpForce;

    [ReadOnly]
    public float jumpCooldown;


    public void Execute(
        ref PhysicsVelocity physicsVelocity,
        ref Player player
    ) {
        var newVelocity = physicsVelocity.Linear + (
            speed * deltaTime * (
                (verticalInput * cameraForward) +
                (horizontalInput * cameraRight)
            )
        );

        if (math.any(newVelocity != float3.zero)) {
            newVelocity = math.normalize(newVelocity) * math.clamp(
                math.length(newVelocity),
                -maxVelocity,
                maxVelocity
            );
        }

        physicsVelocity.Linear = newVelocity;

        if (player.jumpCooldownCounter > 0) {
            player.jumpCooldownCounter -= deltaTime;
        }

        if (jumpInput && player.jumpCooldownCounter <= 0) {
            physicsVelocity.Linear.y += jumpForce;

            player.jumpCooldownCounter = jumpCooldown;
        }
    }
}
