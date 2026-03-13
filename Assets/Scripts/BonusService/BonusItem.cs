using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BonusItem : MonoBehaviour
{
    [SerializeField] GameObject _collectVFXPrefab;
    [SerializeField] private Image timerImage;
    [SerializeField] private float lifetime = 3f;

    private CancellationTokenSource _cts;
    private BonusCard _myCard;

    public void Init(BonusCard card)
    {
        _myCard = card; // Сообщаем ему бонусную карту при инициализации
        Debug.Log("Бонусная карта " + _myCard.name);
    }

    // Инициализация: бонус сам создает свой токен и связывает его с картой/менеджером
    public void Activate(CancellationToken managerToken, CancellationToken cardToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(managerToken, cardToken);
        RunLifecycle(_cts.Token).Forget(); // Запускаем и забываем (Fire and Forget)
    }

    private async UniTaskVoid RunLifecycle(CancellationToken token)
    {
        try
        {
            float elapsed = 0;
            while (elapsed < lifetime)
            {
                elapsed += Time.deltaTime;
                if (timerImage != null) timerImage.fillAmount = 1f - (elapsed / lifetime);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
            // Если дошли сюда — время вышло
            Debug.Log("Бонус просрочен");
        }
        catch (OperationCanceledException)
        {
            // Если отмена пришла НЕ от удаления объекта — значит, это клик!
            if (!token.IsCancellationRequested) return;
            // Логика клика обрабатывается в методе Collect() ниже
        }
        finally
        {
            if (this != null) Destroy(gameObject);
        }
    }
    [Inject] private SignalBus _signalBus;
    public void Collect()
    {
        // Просто "стреляем" сигналом и сообщаем, какая бонусная карта открыта
        _signalBus.Fire(new BonusCollectedSignal { unlockedCard = _myCard });
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
