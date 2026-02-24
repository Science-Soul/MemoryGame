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

    public void Initialize()
    {
        Debug.Log("[BonusService] Подписываюсь на сигнал...");
        _signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);
    }
    public void Dispose() => _signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);

    private void OnBonusCollected(BonusCollectedSignal signal)
    {
        Debug.Log($"[BonusService] Сигнал ПОЛУЧЕН! Карта: {(signal.unlockedCard != null ? signal.unlockedCard.name : null)}");
        Time.timeScale = 0;
        _popup.ShowWithCard(signal.unlockedCard, _popup.GetCancellationTokenOnDestroy()).Forget();
        Debug.Log("Показана карта " + signal.unlockedCard);
        //await _popup.ShowAndRotate(_popup.GetCancellationTokenOnDestroy());
        Time.timeScale = 1;
    }
}
