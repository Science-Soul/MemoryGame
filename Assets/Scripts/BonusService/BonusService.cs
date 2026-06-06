using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BonusService : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly BonusPopup _popup;

    public BonusService(SignalBus signalBus, BonusPopup popup)
    {
        _signalBus = signalBus;
        _popup = popup;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);
    }
    public void Dispose() => _signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);

    private void OnBonusCollected(BonusCollectedSignal signal)
    {
        Time.timeScale = 0;
        _popup.ShowWithCard(signal.unlockedCard, _popup.GetCancellationTokenOnDestroy()).Forget();
        Time.timeScale = 1;
    }
}
