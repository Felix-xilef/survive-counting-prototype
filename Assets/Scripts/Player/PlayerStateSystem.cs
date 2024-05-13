using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct PlayerStateSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<GameManager>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        var entityCommandBuffer = new EntityCommandBuffer(
            Allocator.Domain
        );

        if (gameManager.gameState == GameState.Playing) {
            var job = new PlayerAddPhysicsJob {
                entityCommandBuffer = entityCommandBuffer,
            };
            job.Schedule();

        } else {
            var job = new PlayerRemovePhysicsJob {
                entityCommandBuffer = entityCommandBuffer,
            };
            job.Schedule();
        }

        state.CompleteDependency();

        entityCommandBuffer.Playback(
            state.EntityManager
        );
    }
}
