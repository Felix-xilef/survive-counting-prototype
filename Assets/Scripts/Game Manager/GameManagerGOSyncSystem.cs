using Unity.Entities;

public partial class GameManagerGOSyncSystem : SystemBase {

    private GameState gameState;

    protected override void OnCreate() {
        RequireForUpdate<GameManager>();
    }

    protected override void OnUpdate() {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        var newGameState = CheckNewGameState(
            gameManager
        );

        if (gameState != newGameState) {
            gameState = newGameState;
            GameManagerGO.Instance.gameState = newGameState;

            var gameManagerRW = SystemAPI.GetSingletonRW<GameManager>();
            gameManagerRW.ValueRW.gameState = newGameState;

            if (newGameState == GameState.Playing) {
                var config = SystemAPI.GetSingleton<Config>();
                GameManagerGO.Instance.Timer = config.gameDurationInSeconds;
            }

        } else if (gameState == GameState.Playing) {
            var newTime = GameManagerGO.Instance.Timer - SystemAPI.Time.DeltaTime;

            if (newTime <= 0) {
                GameManagerGO.Instance.Timer = 0;

                gameState = GameState.Won;

                GameManagerGO.Instance.gameState = gameState;

                var gameManagerRW = SystemAPI.GetSingletonRW<GameManager>();
                gameManagerRW.ValueRW.gameState = gameState;

            } else {
                GameManagerGO.Instance.Timer = newTime;
            }
        }

        if (GameManagerGO.Instance.Counter != gameManager.counter) {
            GameManagerGO.Instance.Counter = gameManager.counter;
        }
    }

    private GameState CheckNewGameState(GameManager gameManager) {
        return gameState != gameManager.gameState ?
            gameManager.gameState :
        gameState != GameManagerGO.Instance.gameState ?
            GameManagerGO.Instance.gameState :
        gameState;
    }
}
