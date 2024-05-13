using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public partial struct PlayerMovementSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<GameManager>();
    }

    public void OnUpdate(ref SystemState state) {
        var gameManager = SystemAPI.GetSingleton<GameManager>();

        if (gameManager.gameState == GameState.Playing) {
            var config = SystemAPI.GetSingleton<Config>();

            var job = new PlayerMovementJob {
                verticalInput = InputManager.Vertical,
                horizontalInput = InputManager.Horizontal,
                jumpInput = InputManager.Jump,

                cameraForward = Camera.main.transform.forward,
                cameraRight = Camera.main.transform.right,

                deltaTime = SystemAPI.Time.DeltaTime,

                speed = config.playerSpeed,
                maxVelocity = config.playerMaxVelocity,
                jumpForce = config.playerJumpForce,
                jumpCooldown = config.playerJumpCooldown,
            };
            job.Schedule();
        }
    }
}
