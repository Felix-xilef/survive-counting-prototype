using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour {

    class Baker : Baker<EnemyAuthoring> {

        public override void Bake(EnemyAuthoring authoring) {
            var entity = GetEntity(
                authoring,
                TransformUsageFlags.Dynamic
            );

            AddComponent(
                entity,
                new Enemy {
                    isActive = false,
                }
            );
        }
    }
}

public struct Enemy : IComponentData {
    public bool isActive;
}
