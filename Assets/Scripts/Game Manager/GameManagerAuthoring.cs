using Unity.Entities;
using UnityEngine;

public class GameManagerAuthoring : MonoBehaviour {

    class Baker : Baker<GameManagerAuthoring> {

        public override void Bake(GameManagerAuthoring authoring) {
            var entity = GetEntity(
                authoring,
                TransformUsageFlags.Dynamic
            );

            AddComponent(
                entity,
                new GameManager {
                    gameState = GameState.OnMenu,
                    counter = 0,
                }
            );
        }
    }
}

public struct GameManager : IComponentData {
    public GameState gameState;
    public int counter;
}
