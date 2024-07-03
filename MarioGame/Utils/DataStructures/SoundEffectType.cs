namespace SuperMarioBros.Utils.DataStructures;

public enum SoundEffectType
{
    PlayerJump, // when the player jumps
    BlockDestroyed, // destroy a block when you are big
    BlockPowerUpCollided, // collide with a block that has a power up
    NonBreakableBlockCollided, // when you are big but cannot destroy the block or when you are small and cannot destroy it
    CoinCollected, // when the player collects a coin
    PlayerLostPowerUpBecauseHit, // when the player is hit and loses a power up
    PlayerLostLife, // when the player loses a life
    EnemyDestroyed, //normal destroy
    PowerUpCollected, // when the player collects a power up
    PlayerFireball, // when the player shoots a fireball
    PlayerFireballCollided, // when the player's fireball collides with an enemy
    EnemyDestroyedByStar // when the player's star power up destroys an enemy

    // add more types of sound effects here
}
