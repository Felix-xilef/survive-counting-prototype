using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial struct PowerFieldLifeSystem : ISystem {

    private NativeArray<bool> destroyPowerField;

    private EntityQuery activePowerFieldQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<GameManager>();
        state.RequireForUpdate<Config>();

        destroyPowerField = new NativeArray<bool>(
            1,
            Allocator.Persistent
        );
        destroyPowerField[0] = false;

        activePowerFieldQuery = SystemAPI.QueryBuilder().WithAll<PowerField>().Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            if (destroyPowerField[0]) {
                destroyPowerField[0] = false;

                var powerField = SystemAPI.GetSingleton<PowerField>();

                if (powerField.state != PowerFieldState.Growing) {
                    DestroyActivePowerField(
                        state.EntityManager
                    );
                }
            }

            var config = SystemAPI.GetSingleton<Config>();

            var job = new PowerFieldLifeJob {
                deltatime = SystemAPI.Time.DeltaTime,

                powerFieldSizeChangeSpeed = config.powerFieldSizeChangeSpeed,
                powerFieldDuration = config.powerFieldDuration,
                powerFieldMinSize = config.powerFieldMinSize,
                powerFieldMaxSize = config.powerFieldMaxSize,

                destroyPowerField = destroyPowerField,
            };
            job.Schedule();

        } else {
            destroyPowerField[0] = false;

            if (!activePowerFieldQuery.IsEmpty) {
                DestroyActivePowerField(
                    state.EntityManager
                );
            }
        }
    }

    [BurstCompile]
    private void DestroyActivePowerField(EntityManager entityManager) {
        entityManager.DestroyEntity(
            activePowerFieldQuery.ToEntityArray(
                Allocator.Temp
            )
        );
    }
}
