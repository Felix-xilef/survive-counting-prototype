using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(PowerField))]
public partial struct PowerFieldPositionJob : IJobEntity {

    [ReadOnly]
    public float3 playerPosition;

    public void Execute(ref LocalTransform transform) {
        transform.Position = playerPosition;
    }
}
