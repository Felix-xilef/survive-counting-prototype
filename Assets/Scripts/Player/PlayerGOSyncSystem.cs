using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerMovementSystem))]
public partial class PlayerGOSyncSystem : SystemBase {

    protected override void OnCreate() {
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate() {
        foreach (
            var transform in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<Player>()
        ) {
            PlayerGO.Instance.transform.SetPositionAndRotation(
                transform.ValueRO.Position,
                transform.ValueRO.Rotation
            );
        }
    }
}
