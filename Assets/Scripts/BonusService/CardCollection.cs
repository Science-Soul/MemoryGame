using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CardCollection : MonoBehaviour
{
    [Inject] LevelUnlockModel _model;
    [Inject] ISaveStorage _storage;

    //[SerializeField] BonusCardDeckSO deck;
    [SerializeField] List<BonusCard> cards;

    private SaveData _saveData;

    private void Awake()
    {
        _saveData = _storage.Load();
    }

    private void Start()
    {
        foreach (BonusCard card in cards)
        {
            bool needShow = _saveData.UnlockedCardNames.Contains(card.name);
            card.gameObject.SetActive(needShow);
        }
    }

}
