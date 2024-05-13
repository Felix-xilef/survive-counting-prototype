using Unity.Entities;
using UnityEngine;

public class PowerFieldAuthoring : MonoBehaviour {

    class Baker : Baker<PowerFieldAuthoring> {

        public override void Bake(PowerFieldAuthoring authoring) {
            var entity = GetEntity(
                authoring,
                TransformUsageFlags.Dynamic
            );

            AddComponent(
                entity,
                new PowerField {
                    state = PowerFieldState.Growing,
                }
            );
        }
    }
}

public struct PowerField : IComponentData {
    public PowerFieldState state;

    public float durationCounter;
}
