using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using static PlayerAchievments;
using Random = UnityEngine.Random;
using Zenject;
using Assets.Scripts;

public class Desk : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] Deck _currentDeck;
    [SerializeField] GameObject[] cardPrefabs;
    [SerializeField][Range(2, 4)] int numberOfCardsToSearch = 2;

    private List<GameObject> shuffledDeck;
    private List<GameObject> allCards;
    private List<GameObject> openedCards;
    private int numberOfSets;
    private int numberOfMatchedCards = 0;
    private int baseBonusTime = 0;

    [SerializeField] DifficultLevels difficultLevel;
    private DifficultLevels currentDifficult;

    private GridLayoutGroup gridLayout;

    private ResourceModel _resourceModel;
    private LevelSettings _levelSettings;

    [Inject]
    public void Construct(ResourceModel resourceModel, LevelSettings levelSettings)
    {
        _resourceModel = resourceModel;
        _levelSettings = levelSettings;
    }

    private void Start()
    {
        GridInit();
        uiManager.UpdateUI();
        uiManager.levelObjectives.Init("Находи по " + numberOfCardsToSearch + " одинаковые карты");
        
    }

    private void GridInit()
    {
        openedCards = new List<GameObject>(numberOfCardsToSearch);
        currentDifficult = difficultLevel;
        this.baseBonusTime = currentDifficult.BaseBonusTime;
        numberOfSets = currentDifficult.NumberOfCardsOnDesk / numberOfCardsToSearch;
        GridLayoutInit();
        GridFill();
        ExpAdded += OnExpAdded;

        CreateBonusAtRandomCard();
    }

    private void OnDestroy()
    {
        ExpAdded -= OnExpAdded;
        DOTween.Kill(this.gameObject);
    }

    private void GridFill()
    {
        allCards = new();
        CreateShuffledDeck();
        gameObject.transform.localScale = currentDifficult.GridScale * Vector3.one;
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            GameObject cardGO = Instantiate(shuffledDeck[i], this.gameObject.GetComponent<RectTransform>());
            allCards.Add(cardGO);
            CardLogic card = cardGO.GetComponent<CardLogic>();
            card.InitAnim();
        }

        Debug.Log($"Всего карт на столе {allCards.Count}");
    }

    private void CreateShuffledDeck()
    {
        List<GameObject> shuffledCardSets = new List<GameObject>();
        shuffledDeck = new List<GameObject>();

        foreach (var card in _currentDeck.cardPrefabs)
        {
            shuffledCardSets.Add(card);
        }

        ShuffleDeck(shuffledCardSets);

        for (int i = 0; i < numberOfSets; i++)
        {
            for (int j = 0; j < numberOfCardsToSearch; j++)
            {
                shuffledDeck.Add(shuffledCardSets[i]);
            }
        }

        ShuffleDeck(shuffledDeck);

        void ShuffleDeck(List<GameObject> deck)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                GameObject temp = deck[i];
                int randomIndex = UnityEngine.Random.Range(0, deck.Count);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }
    }

    private void GridLayoutInit()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = currentDifficult.NumberOfRows;
    }

    public void OnCardClicked(GameObject card)
    {
        if (openedCards.Count < numberOfCardsToSearch)
        {
            card.GetComponent<CardLogic>().TurnOverCard();
            openedCards.Add(card);
            if (openedCards.Count == numberOfCardsToSearch)
            {
                StartCoroutine(CheckMatch(card));
            }
        }
    }

    IEnumerator CheckMatch(GameObject card)
    {
        if (openedCards.All(x => x.name == openedCards[0].name))
        {
            ExpAdd(numberOfCardsToSearch * numberOfCardsToSearch);
            _resourceModel.AddResource(_levelSettings.primaryResource, _levelSettings.rewardAmount);
            Debug.Log("Найдено совпадение из " + numberOfCardsToSearch + " карт");

            foreach (var c in openedCards)
            {
                if (_activeBonuses.TryGetValue(c, out var bonus))
                {
                    if (bonus != null)
                    {
                        yield return new WaitUntil(() => !openedCards.Any(x => DOTween.IsTweening(x.transform)));
                        bonus.Collect();
                    }
                    _activeBonuses.Remove(c);
                }

                allCards.Remove(c);
            }

            Debug.Log("Всего карт " + allCards.Count);
            numberOfMatchedCards += numberOfCardsToSearch;
            if (numberOfMatchedCards == currentDifficult.NumberOfCardsOnDesk)
            {
                uiManager.timer.TimerOff();

                // Ждем завершения твинов
                while (DOTween.PlayingTweens() != null && DOTween.PlayingTweens().Count > 0)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(0.1f);
                int bonus = BASE_TIME_BONUS * uiManager.timer.TimeBonusMultiplier(baseBonusTime);
                AddTimeBonus(bonus);
                ExpAdd(EXP_FOR_LEVEL_COMPLETE);
                uiManager.UpdateExpUI(Exp, PreviousExpForLevelUp, CurrentExpForLevelUp);
                uiManager.UpdateMasteryText(CurrentLevel, CurrentMastery);
                uiManager.winScreen.ShowWinScreen(EXP_FOR_LEVEL_COMPLETE, bonus);
            }
        }
        else
        {
            yield return new WaitForSeconds(1);
            foreach (GameObject c in openedCards)
            {
                c.GetComponent<CardLogic>().TurnOverCard();
            }
        }

        openedCards.Clear();
    }

    void OnExpAdded()
    {
        uiManager.UpdateUI();
    }

    [Inject] private DiContainer _container;
    [Inject] private CollectionService _collectionService;
    [SerializeField] BonusItem bonusPrefab;
    private Dictionary<GameObject, BonusItem> _activeBonuses = new();

    private void CreateBonusAtRandomCard()
    {
        BonusCard nextBonusCard = _collectionService.GetNextLockedCard();
        if (nextBonusCard != null)
        {
            GameObject randomCard = allCards[Random.Range(0, allCards.Count)];
            BonusItem bonusInstance = _container.InstantiatePrefabForComponent<BonusItem>(bonusPrefab, randomCard.transform);
            _activeBonuses[randomCard] = bonusInstance;
            bonusInstance.Init(nextBonusCard);
            bonusInstance.Activate(this.GetCancellationTokenOnDestroy(), randomCard.GetCancellationTokenOnDestroy());
        }
        else
        {
            Debug.LogWarning("Все карты уже открыты, бонуса не будет");
        }
    }
}
