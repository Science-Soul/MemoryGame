using System;
using System.Threading.Tasks;
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

    public void Initialize() => _signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);
    public void Dispose() => _signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);

    private async void OnBonusCollected(BonusCollectedSignal signal)
    {
        Time.timeScale = 0;
        await _popup.ShowAndRotate(_popup.GetCancellationTokenOnDestroy());
        Time.timeScale = 1;
    }
}
