using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
[WithAll(typeof(PhysicsVelocity))]
public partial struct PlayerRemovePhysicsJob : IJobEntity {

    public EntityCommandBuffer entityCommandBuffer;

    public void Execute(
        in Entity entity,
        in Player player
    ) {
        entityCommandBuffer.RemoveComponent<PhysicsVelocity>(
            entity
        );

        entityCommandBuffer.SetComponent(
            entity,
            player.initialTransform
        );
    }
}
