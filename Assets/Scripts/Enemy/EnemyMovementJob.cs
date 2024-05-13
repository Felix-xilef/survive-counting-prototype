using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyMovementJob : IJobEntity {

    [ReadOnly]
    public float deltaTime;

    [ReadOnly]
    public float speed;

    [ReadOnly]
    public float maxVelocity;

    [ReadOnly]
    public float activationDistanceSq;

    [ReadOnly]
    public float3 playerPosition;

    public void Execute(
        ref Enemy enemy,
        ref PhysicsVelocity physicsVelocity,
        in LocalToWorld transform
    ) {
        if (enemy.isActive) {
            var newVelocity = physicsVelocity.Linear + (
                speed * deltaTime * math.normalize(playerPosition - transform.Position)
            );

            if (math.any(newVelocity != float3.zero)) {
                newVelocity = math.normalize(newVelocity) * math.clamp(
                    math.length(newVelocity),
                    -maxVelocity,
                    maxVelocity
                );
            }

            physicsVelocity.Linear = newVelocity;

        } else {
            enemy.isActive = math.distancesq(playerPosition, transform.Position) <= activationDistanceSq;
        }
    }
}
