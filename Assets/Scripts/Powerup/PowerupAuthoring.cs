using Unity.Entities;
using UnityEngine;

public class PowerupAuthoring : MonoBehaviour {

    class Baker : Baker<PowerupAuthoring> {

        public override void Bake(PowerupAuthoring authoring) {
            var entity = GetEntity(
                authoring,
                TransformUsageFlags.Dynamic
            );

            AddComponent<Powerup>(
                entity
            );
        }
    }
}

public struct Powerup : IComponentData {}
