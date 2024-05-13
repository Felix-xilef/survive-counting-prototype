using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour {

    class Baker : Baker<PlayerAuthoring> {

        public override void Bake(PlayerAuthoring authoring) {
            var entity = GetEntity(
                authoring,
                TransformUsageFlags.Dynamic
            );

            AddComponent(
                entity,
                new Player {
                    initialTransform = new LocalTransform {
                        Position = authoring.transform.position,
                        Rotation = authoring.transform.rotation,
                        Scale = 1,
                    },

                    jumpCooldownCounter = 0,
                }
            );
        }
    }
}

public struct Player : IComponentData {
    public LocalTransform initialTransform;

    public float jumpCooldownCounter;
}
