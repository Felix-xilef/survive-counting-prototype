using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateAfter(typeof(PlayerEnemyCollisionSystem))]
[UpdateAfter(typeof(EnemyPlayerCollisionSystem))]
public partial struct PlayerPowerupCollisionSystem : ISystem {

    private CollisionFilter collisionFilter;

    private NativeList<Entity> powerupsHit;

    [BurstCompile]
    void OnCreate(ref SystemState state) {
        state.RequireForUpdate<GameManager>();

        collisionFilter = new CollisionFilter {
            BelongsTo = (uint) GameObjectLayer.Powerup,
            CollidesWith = (uint) GameObjectLayer.Powerup,
        };

        powerupsHit = new NativeList<Entity>(
            Allocator.Persistent
        );
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            if (powerupsHit.Length > 0) {
                state.EntityManager.DestroyEntity(
                    powerupsHit.AsArray()
                );
                powerupsHit.Clear();

                var powerFieldIsActive = SystemAPI.TryGetSingletonRW(
                    out RefRW<PowerField> powerField
                );

                if (powerFieldIsActive) {
                    powerField.ValueRW.state = PowerFieldState.Growing;

                } else {
                    var config = SystemAPI.GetSingleton<Config>();

                    state.EntityManager.Instantiate(
                        config.powerFieldPrefab
                    );
                }
            }

            var job = new PlayerPowerupCollisionJob {
                collisionFilter = collisionFilter,
                collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
                powerupsHit = powerupsHit,
            };
            job.Schedule();
        }
    }
}
