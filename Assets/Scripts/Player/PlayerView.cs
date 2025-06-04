using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private PlayerCollisions playerCollisions;

    private void OnEnable()
    {
        playerCollisions.PlayerHitEnemyAction += MakeShipInvulnerable;
    }

    private void OnDisable()
    {
        playerCollisions.PlayerHitEnemyAction -= MakeShipInvulnerable;
    }

    private void MakeShipInvulnerable()
    {
        particleSystem.Play();
    }
}