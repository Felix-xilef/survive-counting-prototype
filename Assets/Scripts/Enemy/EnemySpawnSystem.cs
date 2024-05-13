using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemySpawnSystem : ISystem {

    private Random random;

    private EntityQuery activeEnemiesQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<GameManager>();

        var randomSeedNoise = noise.cnoise(
            new float2(.13f, 1.73f)
        );
        random = new Random(
            (uint) (((randomSeedNoise * 100) + 1) * 5)
        );

        activeEnemiesQuery = SystemAPI.QueryBuilder().WithAll<Enemy>().Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            var config = SystemAPI.GetSingleton<Config>();

            var activeEnemies = activeEnemiesQuery.ToEntityArray(
                Allocator.Temp
            );

            var enemiesToSpawn = config.amountOfEnemies - activeEnemies.Length;

            if (enemiesToSpawn > 0) {
                var entities = state.EntityManager.Instantiate(
                    config.enemyPrefab,
                    enemiesToSpawn,
                    Allocator.Temp
                );

                foreach (var item in entities) {
                    var localTransform = SystemAPI.GetComponentRW<LocalTransform>(
                        item
                    );

                    localTransform.ValueRW.Position = new float3(
                        random.NextFloat(-config.xBoundary, config.xBoundary),
                        config.ySpawnPoint,
                        random.NextFloat(-config.zBoundary, config.zBoundary)
                    );
                }
            }

        } else {
            state.EntityManager.DestroyEntity(
                activeEnemiesQuery
            );
        }
    }
}
