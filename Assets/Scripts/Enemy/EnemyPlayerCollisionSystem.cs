using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateAfter(typeof(PlayerStateSystem))]
public partial struct EnemyPlayerCollisionSystem : ISystem {

    private CollisionFilter collisionFilter;

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<GameManager>();

        collisionFilter = new CollisionFilter {
            BelongsTo = (uint) GameObjectLayer.Player,
            CollidesWith = (uint) GameObjectLayer.Player,
        };
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            foreach (
                var (transform, physicsVelocity) in SystemAPI.Query<RefRO<LocalToWorld>, RefRO<PhysicsVelocity>>().WithAll<Enemy>()
            ) {
                var hitPlayer = collisionWorld.SphereCast(
                    transform.ValueRO.Position,
                    transform.ValueRO.Value.Scale().x / 2,
                    -1 * SystemAPI.Time.DeltaTime * physicsVelocity.ValueRO.Linear,
                    1,
                    collisionFilter
                );

                if (hitPlayer) {
                    var gameManagerRW = SystemAPI.GetSingletonRW<GameManager>();
                    gameManagerRW.ValueRW.gameState = GameState.Lost;

                    return;
                }
            }
        }
    }
}
