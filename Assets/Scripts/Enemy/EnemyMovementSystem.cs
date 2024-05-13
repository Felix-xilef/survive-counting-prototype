using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct EnemyMovementSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<GameManager>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            var config = SystemAPI.GetSingleton<Config>();

            foreach (
                var playerTransform in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<Player>()
            ) {
                var job = new EnemyMovementJob {
                    deltaTime = SystemAPI.Time.DeltaTime,
                    speed = config.enemySpeed,
                    maxVelocity = config.enemyMaxVelocity,
                    activationDistanceSq = config.enemyActivationDistanceSq,
                    playerPosition = playerTransform.ValueRO.Position,
                };
                job.Schedule();
            }
        }
    }
}
