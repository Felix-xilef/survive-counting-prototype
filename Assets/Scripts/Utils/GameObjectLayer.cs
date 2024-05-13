public enum GameObjectLayer {
    All = ~0,
    Player = 1 << 3,
    Powerup = 1 << 6,
    Enemy = 1 << 7,
}
