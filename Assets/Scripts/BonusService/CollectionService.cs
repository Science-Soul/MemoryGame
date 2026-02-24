using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CollectionService : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly List<BonusCardDeckSO> _allDecks;
    private readonly ISaveStorage _saveStorage;
    private SaveData _currentSave;

    CollectionService(SignalBus signalBus, List<BonusCardDeckSO> allDecks, ISaveStorage saveStorage)
    {
        _saveStorage = saveStorage;
        _signalBus = signalBus;
        _allDecks = allDecks;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);

        _currentSave = _saveStorage.Load();
        foreach (var deck in _allDecks)
        {
            foreach (var card in deck.bonusCards)
            {
                card.IsUnlocked = _currentSave.UnlockedCardNames.Contains(card.name); // Имена карт из разных колод должны быть уникальными!!!
            }
        }
    }

    public void Dispose() => _signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);


    private void OnBonusCollected(BonusCollectedSignal signal)
    {
        var activeDeck = _allDecks.FirstOrDefault(d => !d.IsComplete);

        if (activeDeck != null)
        {
            var cardToUnlock = signal.unlockedCard;
            cardToUnlock.UnlockCard();
            Debug.Log($"<color=cyan>[Колода: {activeDeck.name}]</color> Открыта карта: {cardToUnlock.name}");
            if (!_currentSave.UnlockedCardNames.Contains(cardToUnlock.name))
            {
                _currentSave.UnlockedCardNames.Add(cardToUnlock.name);

                // Сохраняем обновленный список в JSON
                _saveStorage.Save(_currentSave);
            }

            // Проверяем, не закрыли ли мы всю колоду этим ходом
            if (activeDeck.IsComplete)
            {
                Debug.Log($"<color=gold>ПОЗДРАВЛЯЕМ! Колода {activeDeck.name} полностью собрана!</color>");
            }
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
