using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerMovementSystem))]
public partial struct PowerFieldPositionSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<PowerField>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach (
            var transform in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<Player>()
        ) {
            var job = new PowerFieldPositionJob {
                playerPosition = transform.ValueRO.Position,
            };
            job.Schedule();
        }
    }
}
