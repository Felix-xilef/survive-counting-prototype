using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct PowerFieldEnemyCollisionSystem : ISystem {

    private CollisionFilter collisionFilter;

    private NativeList<Entity> enemiesHit;

    private GameState lastGameState;

    [BurstCompile]
    void OnCreate(ref SystemState state) {
        state.RequireForUpdate<GameManager>();

        collisionFilter = new CollisionFilter {
            BelongsTo = (uint) GameObjectLayer.Enemy,
            CollidesWith = (uint) GameObjectLayer.Enemy,
        };

        enemiesHit = new NativeList<Entity>(
            Allocator.Persistent
        );
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            if (lastGameState != GameState.Playing) {
                enemiesHit.Clear();
                SystemAPI.GetSingletonRW<GameManager>().ValueRW.counter = 0;

            } else if (enemiesHit.Length > gameManager.counter) {
                state.EntityManager.DestroyEntity(
                    enemiesHit.AsArray().Slice(gameManager.counter)
                );

                SystemAPI.GetSingletonRW<GameManager>().ValueRW.counter = enemiesHit.Length;
            }

            var job = new PowerFieldEnemyCollisionJob {
                collisionFilter = collisionFilter,
                collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
                enemiesHit = enemiesHit,
            };
            job.Schedule();
        }

        if (lastGameState != gameManager.gameState) {
            lastGameState = gameManager.gameState;
        }
    }
}
