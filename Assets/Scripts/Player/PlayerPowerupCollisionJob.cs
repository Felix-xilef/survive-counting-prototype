using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(Player))]
public partial struct PlayerPowerupCollisionJob : IJobEntity {

    [ReadOnly]
    public CollisionFilter collisionFilter;

    public CollisionWorld collisionWorld;

    public NativeList<Entity> powerupsHit;

    public void Execute(in LocalToWorld transform) {
        var hits = new NativeList<ColliderCastHit>(
            Allocator.Temp
        );

        collisionWorld.SphereCastAll(
            transform.Position,
            transform.Value.Scale().x / 2,
            float3.zero,
            1,
            ref hits,
            collisionFilter
        );

        foreach (var hit in hits) {
            powerupsHit.Add(
                hit.Entity
            );
        }
    }
}
