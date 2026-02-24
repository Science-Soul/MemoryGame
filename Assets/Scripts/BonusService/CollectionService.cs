using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CollectionService : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly List<BonusCardDeckSO> _allDecks;

    CollectionService(SignalBus signalBus, List<BonusCardDeckSO> allDecks)
    {
        _signalBus = signalBus;
        _allDecks = allDecks;
    }

    public void Initialize() => _signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);
    public void Dispose() => _signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);


    private void OnBonusCollected()
    {
        var activeDeck = _allDecks.FirstOrDefault(d => !d.IsComplete);

        if (activeDeck != null)
        {
            var cardToUnlock = activeDeck.bonusCards.FirstOrDefault(c => !c.IsUnlocked);
            cardToUnlock.UnlockCard();
            Debug.Log($"<color=cyan>[Колода: {activeDeck.name}]</color> Открыта карта: {cardToUnlock.name}");
            
            // Проверяем, не закрыли ли мы всю колоду этим ходом
            if (activeDeck.IsComplete)
                Debug.Log($"<color=gold>ПОЗДРАВЛЯЕМ! Колода {activeDeck.name} полностью собрана!</color>");
        }
        else
        {
            Debug.Log("Все 3 колоды уже собраны! Вы мастер игры.");
        }
    }

    public BonusCard GetNextLockedCard()
    {
        // Ищем в колодах первую попавшуюся закрытую карту
        var activeDeck = _allDecks.FirstOrDefault(d => !d.IsComplete);

        if (activeDeck == null)
        {
            Debug.Log("<color=orange>[Collection] Все колоды уже собраны!</color>");
            return null;
        }
        else
        {
            return activeDeck.bonusCards.FirstOrDefault(c => !c.IsUnlocked);
        }
    }
}
