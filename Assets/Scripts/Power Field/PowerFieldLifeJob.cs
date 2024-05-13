using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct PowerFieldLifeJob : IJobEntity {

    [ReadOnly]
    public float deltatime;


    [ReadOnly]
    public float powerFieldSizeChangeSpeed;

    [ReadOnly]
    public float powerFieldDuration;

    [ReadOnly]
    public float powerFieldMinSize;

    [ReadOnly]
    public float powerFieldMaxSize;


    public NativeArray<bool> destroyPowerField;


    public void Execute(
        ref LocalTransform transform,
        ref PowerField powerField
    ) {
        switch (powerField.state) {
            case PowerFieldState.Growing:
                if (transform.Scale < powerFieldMaxSize) {
                    transform.Scale += powerFieldSizeChangeSpeed * deltatime;

                } else {
                    powerField.state = PowerFieldState.Static;
                    powerField.durationCounter = powerFieldDuration;
                }
            break;

            case PowerFieldState.Static:
                powerField.durationCounter -= deltatime;

                if (powerField.durationCounter <= 0) {
                    powerField.state = PowerFieldState.Shrinking;
                }
            break;

            case PowerFieldState.Shrinking:
                if (transform.Scale > powerFieldMinSize) {
                    transform.Scale -= powerFieldSizeChangeSpeed * deltatime;

                } else {
                    destroyPowerField[0] = true;
                }
            break;
        }
    }
}
