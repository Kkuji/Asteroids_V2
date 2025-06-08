using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<PlayerHitSignal>(OnPlayerHit);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PlayerHitSignal>(OnPlayerHit);
    }

    private void OnPlayerHit()
    {
        particleSystem.Play();
    }
}