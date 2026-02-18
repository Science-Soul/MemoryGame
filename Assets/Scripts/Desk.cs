using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using static PlayerAchievments;
using Random = UnityEngine.Random;
using System.Threading;
using Unity.VisualScripting;

public class Desk : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
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

    private void Start()
    {
        openedCards = new List<GameObject>(numberOfCardsToSearch);
        currentDifficult = difficultLevel;
        this.baseBonusTime = currentDifficult.BaseBonusTime;
        numberOfSets = currentDifficult.NumberOfCardsOnDesk / numberOfCardsToSearch;
        uiManager.UpdateUI();
        uiManager.levelObjectives.Init("Находи по " + numberOfCardsToSearch + " одинаковые карты");
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

        foreach (var card in cardPrefabs)
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
            Debug.Log("Найдено совпадение из " + numberOfCardsToSearch + " карт");

            foreach (var c in openedCards)
            {
                if (_activeBonuses.TryGetValue(c, out var bonus))
                {
                        if (bonus != null) bonus.Collect(); // Бонус сам все сделает
                        _activeBonuses.Remove(c);
                }
            }

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

    [SerializeField] BonusItem bonusPrefab;
    [SerializeField] float bonusDurationInSec = 5f;
    private Dictionary<GameObject, BonusItem> _activeBonuses = new();
    

    private void CreateBonusAtRandomCard()
    {
        GameObject randomCard = allCards[Random.Range(0, allCards.Count)];
        BonusItem bonusInstance = Instantiate(bonusPrefab, randomCard.transform);
        _activeBonuses[randomCard] = bonusInstance;
        bonusInstance.Activate(this.GetCancellationTokenOnDestroy(), randomCard.GetCancellationTokenOnDestroy());
    }

    /*private async UniTask RunBonusLogic(GameObject card, BonusItem bonus)
    {
        var manualCts = new CancellationTokenSource();
        _activeBonusesTokens[card] = manualCts;

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            manualCts.Token,
            card.GetCancellationTokenOnDestroy(),
            this.GetCancellationTokenOnDestroy()
        );

        try
        {
            // Просто запускаем логику ВНУТРИ уже созданного бонуса
            await bonus.StartBonusLifecycle(linkedCts.Token);

            Debug.Log("Время бонуса вышло");
        }
        catch (OperationCanceledException)
        {
            // 2. Если менеджер УЖЕ уничтожен (смена сцены) — ВЫХОДИМ НЕМЕДЛЕННО
            // Мы не трогаем переменные, не пишем в консоль, просто исчезаем.
            if (this == null || this.GetCancellationTokenOnDestroy().IsCancellationRequested)
                return;

            // 3. Если менеджер жив, проверяем: был ли это клик?
            if (card != null && manualCts.IsCancellationRequested)
            {
                Debug.Log("<color=yellow>БОНУС СОБРАН!</color>");
                // Награда...
            }
        }
        finally
        {
            // Сначала отменяем связанный источник, чтобы остановить все вложенные задачи
            linkedCts.Cancel();

            // Удаляем из словаря ПЕРЕД тем, как уничтожать токен
            if (this != null && card != null)
            {
                _activeBonusesTokens.Remove(card);
            }

            // 5. Уничтожаем источники только если менеджер еще существует
            // и делаем это максимально осторожно
            linkedCts.Dispose();
            manualCts.Dispose();
        }
    }*/
}
