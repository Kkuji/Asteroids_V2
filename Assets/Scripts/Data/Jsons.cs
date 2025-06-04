[System.Serializable]
public class PlayerConfig
{
    public int maxHealth;
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public float inertiaTime;
    public float inertiaDecreaseSpeed;
    public float inertiaDragFactor;
    public float hitDisableDuration;
    public float hitSpeedDecrease;
    public float minHitPushForce;
    public float invulnerableDuration;
    public float moveAfterHitDuration;
    public float laserAppearDuration;
    public float laserCooldown;
}

[System.Serializable]
public class EnemyConfig
{
    public float asteroidMinSpeed;
    public float asteroidMaxSpeed;
    public float enemyShipMinSpeed;
    public float enemyShipMaxSpeed;
    public float asteroidDeflectionAngle;
    public float bounceCooldown;
    public float defaultEnemiesPositionZ;
    public int scoreAsteroid;
    public int scoreAsteroidSmall;
    public int scoreEnemyShip;
}

[System.Serializable]
public class WorldConfig
{
    public int startAsteroidsAmount;
    public int maxAsteroidsAmount;
    public int startEnemyShipsAmount;
    public int maxEnemyShipsAmount;
    public int starBulletsAmount;
    public int maxBulletsAmount;
    public float bulletSpeed;
    public float asteroidsSpawnMinTimeStep;
    public float asteroidsSpawnMaxTimeStep;
    public float enemyShipSpawnMinTimeStep;
    public float enemyShipSpawnMaxTimeStep;
    public float laserAppearDuration;
    public int maxShots;
    public float shotRechargeTime;
}

[System.Serializable]
public class GameConfig
{
    public PlayerConfig playerConfig = new();
    public EnemyConfig enemyConfig = new();
    public WorldConfig worldConfig = new();
}