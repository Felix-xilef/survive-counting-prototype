using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(PowerField))]
public partial struct PowerFieldEnemyCollisionJob : IJobEntity {

    [ReadOnly]
    public CollisionFilter collisionFilter;

    public CollisionWorld collisionWorld;

    public NativeList<Entity> enemiesHit;

    public void Execute(in LocalToWorld transform) {
        var hits = new NativeList<ColliderCastHit>(
            Allocator.Domain
        );
        collisionWorld.SphereCastAll(
            transform.Position,
            transform.Value.Scale().x / 2,
            float3.zero,
            1,
            ref hits,
            collisionFilter
        );

        var entities = new NativeHashSet<Entity>(
            hits.Length,
            Allocator.Domain
        );
        foreach (var hit in hits) {
            if (
                entities.Add(hit.Entity) &&
                enemiesHit.IndexOf(hit.Entity) == -1
                
            ) {
                enemiesHit.Add(hit.Entity);
            }
        }
    }
}
