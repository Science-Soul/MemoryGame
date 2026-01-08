using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    [SerializeField] int numberOfCardsToSearch = 2;
    private List<GameObject> shuffledCardDeck;

    //int xDim = 10;
    int yDim = 4;

    private GridLayoutGroup gridLayout;


    private void Awake()
    {
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
            Instantiate(shuffledCardDeck[i], this.gameObject.transform);
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
}
