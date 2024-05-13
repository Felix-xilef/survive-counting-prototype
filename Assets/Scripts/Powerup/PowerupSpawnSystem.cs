using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PowerupSpawnSystem : ISystem {

    private Random random;

    private float spawnCooldownCounter;

    private EntityQuery powerupQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<GameManager>();

        var randomSeedNoise = noise.cnoise(
            new float2(.44f, 1.97f)
        );
        random = new Random(
            (uint) (((randomSeedNoise * 100) + 1) * 5)
        );

        spawnCooldownCounter = 0;

        powerupQuery = SystemAPI.QueryBuilder().WithAll<Powerup>().Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();
        var config = SystemAPI.GetSingleton<Config>();

        if (gameManager.gameState == GameState.Playing) {
            spawnCooldownCounter -= SystemAPI.Time.DeltaTime;

            if (spawnCooldownCounter <= 0) {
                var powerup = state.EntityManager.Instantiate(
                    config.powerupPrefab
                );

                var transform = SystemAPI.GetComponentRW<LocalTransform>(
                    powerup
                );

                transform.ValueRW.Position = new float3(
                    random.NextFloat(-config.xBoundary, config.xBoundary),
                    config.ySpawnPoint,
                    random.NextFloat(-config.zBoundary, config.zBoundary)
                );

                spawnCooldownCounter = config.powerupSpawnCooldown;
            }

        } else {
            spawnCooldownCounter = config.powerupSpawnCooldown;

            state.EntityManager.DestroyEntity(
                powerupQuery
            );
        }
    }
}
