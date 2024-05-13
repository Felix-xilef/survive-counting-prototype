using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateAfter(typeof(PlayerStateSystem))]
public partial struct PlayerEnemyCollisionSystem : ISystem {

    private CollisionFilter collisionFilter;

    private NativeArray<bool> hasHit;

    [BurstCompile]
    void OnCreate(ref SystemState state) {
        state.RequireForUpdate<GameManager>();

        collisionFilter = new CollisionFilter {
            BelongsTo = (uint) GameObjectLayer.Enemy,
            CollidesWith = (uint) GameObjectLayer.Enemy,
        };

        hasHit = new NativeArray<bool>(
            1,
            Allocator.Persistent
        );
        hasHit[0] = false;
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            foreach (
                var (transform, physicsVelocity) in SystemAPI.Query<RefRO<LocalToWorld>, RefRO<PhysicsVelocity>>().WithAll<Player>()
            ) {
                var hitEnemy = collisionWorld.SphereCast(
                    transform.ValueRO.Position,
                    transform.ValueRO.Value.Scale().x / 2,
                    -1 * SystemAPI.Time.DeltaTime * physicsVelocity.ValueRO.Linear,
                    1,
                    collisionFilter
                );

                if (hitEnemy) {
                    var gameManagerRW = SystemAPI.GetSingletonRW<GameManager>();
                    gameManagerRW.ValueRW.gameState = GameState.Lost;

                    return;
                }
            }
        }
    }
}
