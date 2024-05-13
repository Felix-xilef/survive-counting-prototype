using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
[WithAll(typeof(Player))]
[WithNone(typeof(PhysicsVelocity))]
public partial struct PlayerAddPhysicsJob : IJobEntity {

    public EntityCommandBuffer entityCommandBuffer;

    public void Execute(in Entity entity) {
        entityCommandBuffer.AddComponent<PhysicsVelocity>(
            entity
        );
    }
}
