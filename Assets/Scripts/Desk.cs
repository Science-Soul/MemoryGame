using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject[] cardPrefabs;
    [SerializeField][Range(2, 4)] int numberOfCardsToSearch = 2;

    private int numberOfSets;
    private List<GameObject> shuffledDeck;
    private List<GameObject> openedCards;
    private int numberOfMatchedCards = 0;

    [SerializeField] DifficultLevels difficultLevel;
    private DifficultLevels currentDifficult;

    private GridLayoutGroup gridLayout;

    private void Start()
    {
        openedCards = new List<GameObject>(numberOfCardsToSearch);
        currentDifficult = difficultLevel;
        numberOfSets = currentDifficult.NumberOfCardsOnDesk / numberOfCardsToSearch;
        uiManager.levelObjectives.Init("Находи по " + numberOfCardsToSearch + " одинаковые карты");
        GridLayoutInit();
        GridFill();
    }

    private void GridFill()
    {
        CreateShuffledDeck();
        gameObject.transform.localScale = currentDifficult.GridScale * Vector3.one;
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            Instantiate(shuffledDeck[i], this.gameObject.GetComponent<RectTransform>());
        }
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
            Debug.Log("Deck shuffled");
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
        PlayerAchievments.ExpAdd();
        uiManager.expText.text = PlayerAchievments.Exp.ToString();

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
            Debug.Log("Найдено совпадение из " + numberOfCardsToSearch + " карт");
            numberOfMatchedCards += numberOfCardsToSearch;
            if (numberOfMatchedCards == currentDifficult.NumberOfCardsOnDesk)
            {
                uiManager.timer.TimerOff();

                // Ждем завершения твинов
                while (DOTween.PlayingTweens() != null && DOTween.PlayingTweens().Count > 0)
                {
                    yield return null;
                }
                Debug.Log("Все твины завершились. Окно победы.");

                yield return new WaitForSeconds(0.1f);
                uiManager.winScreen.ShowWinScreen(uiManager.timer.TimeText.text);
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
}
