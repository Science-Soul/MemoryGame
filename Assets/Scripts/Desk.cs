using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour
{
    [SerializeField] GameObject[] cardPrefabs;
    [SerializeField] int numberOfCardsToSearch = 2;
    private int numberOfSets;
    private List<GameObject> shuffledDeck;
    private List<GameObject> openedCards;

    [SerializeField] DifficultLevels difficultLevel;
    private DifficultLevels currentDifficult;

    private GridLayoutGroup gridLayout;


    private void Awake()
    {
        openedCards = new List<GameObject>(numberOfCardsToSearch);
        currentDifficult = difficultLevel;
        numberOfSets = difficultLevel.NumberOfCardsOnDesk / numberOfCardsToSearch;
        GridLayoutInit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GridFill();
    }

    private void GridFill()
    {
        CreateShuffledDeck();

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
            for (int j = 0; j < numberOfCardsToSearch; j++) {
                shuffledDeck.Add(shuffledCardSets[i]);
            }
        }

        ShuffleDeck(shuffledDeck);

        void ShuffleDeck(List<GameObject> deck)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                GameObject temp = deck[i];
                int randomIndex = Random.Range(0, deck.Count);
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
        gridLayout.constraintCount = difficultLevel.NumberOfRows;
    }

    public void OnCardClicked(GameObject card)
    {
        card.GetComponent<CardLogic>().OpenCard();
        CheckMatch(card);
    }

    private void CheckMatch(GameObject card)
    {
        openedCards.Add(card);
        if (openedCards.Count == 1)
        {
            return;
        }
        else
        {
            if (card.name == openedCards[0].name)
            {
                if (openedCards.Count == numberOfCardsToSearch)
                {
                    Debug.Log("Найдено совпадение из " + numberOfCardsToSearch + " карт");
                    openedCards.Clear();
                }
                else
                {
                    return;
                }
            }
            else
            {
                foreach (GameObject c in openedCards)
                {
                    CardLogic cardLogic = c.GetComponent<CardLogic>();
                    cardLogic.CloseCard();
                }
                openedCards.Clear();
            }
        }
    }
}
