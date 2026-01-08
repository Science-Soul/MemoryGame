using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Desk : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    [SerializeField] int numberOfCardsToSearch = 2;
    private List<GameObject> shuffledCardDeck;
    private List<GameObject> openedCards;

    //int xDim = 10;
    int yDim = 4;

    private GridLayoutGroup gridLayout;


    private void Awake()
    {
        openedCards = new List<GameObject>(numberOfCardsToSearch);
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

        for (int i = 0; i < shuffledCardDeck.Count; i++)
        {
            Instantiate(shuffledCardDeck[i], this.gameObject.GetComponent<RectTransform>());
        }
    }

    private void CreateShuffledDeck()
    {
        shuffledCardDeck = new List<GameObject>();

        foreach (var card in cardPrefabs)
        {
            for (int i = 0; i < numberOfCardsToSearch; i++)
            {
                shuffledCardDeck.Add(card);
            }
        }

        for (int i = 0; i < shuffledCardDeck.Count; i++)
        {
            GameObject temp = shuffledCardDeck[i];
            int randomIndex = Random.Range(0, shuffledCardDeck.Count);
            shuffledCardDeck[i] = shuffledCardDeck[randomIndex];
            shuffledCardDeck[randomIndex] = temp;
        }
    }

    private void GridLayoutInit()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = yDim;
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
                    c.GetComponent<CardLogic>().CloseCard();
                }
                openedCards.Clear();
            }
        }
    }
}
