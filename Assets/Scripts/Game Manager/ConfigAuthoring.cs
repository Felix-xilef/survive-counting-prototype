using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour {

    public float xBoundary;
    public float zBoundary;
    public float ySpawnPoint;

    [Space]

    public float gameDurationInSeconds;


    [Header("Powerup")]
    public GameObject powerupPrefab;
    public float powerupSpawnCooldown;


    [Header("Enemy")]
    public GameObject enemyPrefab;
    public int amountOfEnemies;
    public float enemySpeed;
    public float enemyMaxVelocity;
    public float enemyActivationDistance;


    [Header("Player")]
    public float playerSpeed;
    public float playerMaxVelocity;
    public float playerJumpForce;
    public float playerJumpCooldown;
    public float playerInsideCapsuleRotationSpeed;


    [Header("Power Field")]
    public GameObject powerFieldPrefab;
    public float powerFieldDuration;
    public float powerFieldSizeChangeSpeed;
    public float powerFieldMinSize;
    public float powerFieldMaxSize;


    class Baker : Baker<ConfigAuthoring> {

        public override void Bake(ConfigAuthoring authoring) {
            var entity = GetEntity(
                authoring,
                TransformUsageFlags.None
            );

            AddComponent(
                entity,
                new Config {
                    xBoundary = authoring.xBoundary,
                    zBoundary = authoring.zBoundary,
                    ySpawnPoint = authoring.ySpawnPoint,

                    gameDurationInSeconds = authoring.gameDurationInSeconds,

                    powerupPrefab = GetEntity(
                        authoring.powerupPrefab,
                        TransformUsageFlags.Dynamic
                    ),
                    powerupSpawnCooldown = authoring.powerupSpawnCooldown,

                    enemyPrefab = GetEntity(
                        authoring.enemyPrefab,
                        TransformUsageFlags.Dynamic
                    ),
                    amountOfEnemies = authoring.amountOfEnemies,
                    enemySpeed = authoring.enemySpeed,
                    enemyMaxVelocity = authoring.enemyMaxVelocity,
                    enemyActivationDistanceSq = math.pow(
                        authoring.enemyActivationDistance,
                        2
                    ),

                    playerSpeed = authoring.playerSpeed,
                    playerMaxVelocity = authoring.playerMaxVelocity,
                    playerJumpForce = authoring.playerJumpForce,
                    playerJumpCooldown = authoring.playerJumpCooldown,
                    playerInsideCapsuleRotationSpeed = authoring.playerInsideCapsuleRotationSpeed,

                    powerFieldPrefab = GetEntity(
                        authoring.powerFieldPrefab,
                        TransformUsageFlags.Dynamic
                    ),
                    powerFieldDuration = authoring.powerFieldDuration,
                    powerFieldSizeChangeSpeed = authoring.powerFieldSizeChangeSpeed,
                    powerFieldMinSize = authoring.powerFieldMinSize,
                    powerFieldMaxSize = authoring.powerFieldMaxSize,
                }
            );
        }
    }
}

public struct Config : IComponentData {
    public float xBoundary;
    public float zBoundary;
    public float ySpawnPoint;


    public float gameDurationInSeconds;


    public Entity powerupPrefab;
    public float powerupSpawnCooldown;


    public Entity enemyPrefab;
    public int amountOfEnemies;
    public float enemySpeed;
    public float enemyMaxVelocity;
    public float enemyActivationDistanceSq;


    public float playerSpeed;
    public float playerMaxVelocity;
    public float playerJumpForce;
    public float playerJumpCooldown;
    public float playerInsideCapsuleRotationSpeed;


    public Entity powerFieldPrefab;
    public float powerFieldDuration;
    public float powerFieldSizeChangeSpeed;
    public float powerFieldMinSize;
    public float powerFieldMaxSize;
}
