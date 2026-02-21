using System;
using UnityEngine;
using Zenject;

/*public class CollectionService : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly List<CardDeck> _allDecks;

    CollectionService(SignalBus signalBus, List<CardDeck> allDecks)
    {
        _signalBus = signalBus;
        _allDecks = allDecks;
    }

    public void Dispose() => _signalBus.Subscribe<BonusCollectedSignal>();

    public void Initialize() => _signalBus.Unsubscribe<BonusCollectedSignal>();

    private void OnBonusCollected()
    {
        var activeDeck = _allDecks.FirstOrDefault(d => !d.IsComplete);
    }
}
*/