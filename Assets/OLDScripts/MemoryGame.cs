using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

public class MemoryGame : MonoBehaviour
{
    public RectTransform container;
    public GameObject cardPrefab;
    public int rows = 4;
    public int columns = 4;
    public float spacing = 10f;
    public float bounceDuration = 0.5f;
    public List<GameObject> cardSprites;
    public Sprite backSprite;

    [Space]
    public GameObject[] cardPrefabs;

    private List<GameObject> shuffledSprites;
    private List<GameObject> openedCards = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCurrentDeckStyle(deckStyleA);
        //CardDisplay card = Instantiate(cardPrefab, container).GetComponent<CardDisplay>();
        //card.InitializeCard("Hearts");

        GenerateDeck();
        //CreateShuffledSpriteList();
        //FillGridWithCards();
    }

    private void FillGridWithCards()
    {
        if (container == null || cardPrefab == null)
        {
            Debug.Log("Не указаны контейнер или префаб карты!");
            return;
        }

        Vector2 containerSize = container.rect.size;
        float totalSpacingX = (columns - 1) * spacing;
        float totalSpacingY = (rows - 1) * spacing;

        float availableWidth = containerSize.x - totalSpacingX;
        float availableHeight = containerSize.y - totalSpacingY;

        float cardWidth = availableWidth / columns;
        float cardHeight = availableWidth / rows;
        float cardSize = Mathf.Min(cardHeight, cardWidth);

        float gridWigth = columns * cardSize + totalSpacingX;
        float gridHeight = rows * cardSize + totalSpacingY;

        Vector2 startOffset = new Vector2(
            -gridWigth / 2 + cardSize / 2,
            gridHeight / 2 - cardSize / 2
            );

        int spriteIndex = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject newCard = Instantiate(cardSprites[row], container);
                RectTransform cardTransform = newCard.GetComponent<RectTransform>();
                //Image cardImage = newCard.GetComponent<Image>();
                Button cardButton = newCard.GetComponent<Button>();

                cardTransform.sizeDelta = new Vector2(cardSize, cardSize);

                Vector2 cardPosition = startOffset + new Vector2(
                    col * (cardSize + spacing),
                    -row * (cardSize + spacing));

                cardTransform.anchoredPosition = cardPosition;
                cardTransform.localScale = Vector3.zero;

                if (spriteIndex < shuffledSprites.Count)
                {
                    newCard = shuffledSprites[spriteIndex];
                    Card cardScript = newCard.AddComponent<Card>();
                    cardScript.originalSprite = shuffledSprites[spriteIndex].GetComponent<Image>().sprite;
                    spriteIndex++;
                }

                cardButton.onClick.AddListener(() => OnCardClicked(newCard));

                cardTransform.DOScale(Vector3.one, bounceDuration)
                    .SetEase(Ease.OutBounce)
                    .SetDelay((row * columns + col) * 0.05f);

                DOVirtual.DelayedCall(5f, () => HideCardWithEffect(newCard));
            }
        }
    }

    private void HideCardWithEffect(GameObject card)
    {
        RectTransform cardTransform = card.GetComponent<RectTransform>();
        Image cardImage = card.GetComponent<Image>();

        cardTransform.DOScale(Vector3.zero, bounceDuration)
            .SetEase(Ease.OutCirc)
            .OnComplete(() => ChangeCardImageAndRestore(cardImage, cardTransform));
    }

    private void ChangeCardImageAndRestore(Image cardImage, RectTransform cardTransform)
    {
        cardImage.sprite = backSprite;
        cardTransform.DOScale(Vector3.one, bounceDuration).SetEase(Ease.OutBounce);
    }

    private void OnCardClicked(GameObject card)
    {
        if (openedCards.Count >= 2) return;

        Image cardImage = card.GetComponent<Image>();
        RectTransform cardTransform = card.GetComponent<RectTransform>();
        Card cardScript = card.GetComponent<Card>();

        if (cardImage.sprite == backSprite)
        {
            cardTransform.DOScale(Vector3.zero, bounceDuration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    cardImage.sprite = cardScript.originalSprite;
                    cardTransform.DOScale(Vector3.one, bounceDuration).SetEase(Ease.OutBounce);
                    openedCards.Add(card);

                    if (openedCards.Count == 2)
                    {
                        StartCoroutine(CheckMatch());
                    }
                });
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        Card firstCard = openedCards[0].GetComponent<Card>();
        Card secondCard = openedCards[1].GetComponent<Card>();

        if (firstCard.originalSprite == secondCard.originalSprite)
        {
            foreach (GameObject card in openedCards)
            {
                RectTransform cardTransform = card.GetComponent<RectTransform>();
                cardTransform.DOScale(Vector3.zero, bounceDuration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(card));
            }
        }
        else
        {
            foreach (GameObject card in openedCards)
            {
                Image cardImage = card.GetComponent<Image>();
                RectTransform cardTransform = card.GetComponent<RectTransform>();

                cardTransform.DOScale(Vector3.zero, bounceDuration).SetEase(Ease.OutCirc)
                    .OnComplete(() =>
                    {
                        cardImage.sprite = backSprite;
                        cardTransform.DOScale(Vector3.one, bounceDuration).SetEase(Ease.OutBounce);
                    });
            }
        }

        openedCards.Clear();
    }

    private void CreateShuffledSpriteList()
    {
        if (cardSprites.Count < (rows * columns) / 2)
        {
            Debug.LogError("Недостаточно спрайтов для создания пар!");
            return;
        }

        shuffledSprites = new List<GameObject>();

        for (int i = 0; i < rows * columns / 2; i++)
        {
            shuffledSprites.Add(cardSprites[i]);
            shuffledSprites.Add(cardSprites[i]);
        }

        for (int i = 0; i < shuffledSprites.Count; i++)
        {
            GameObject temp = shuffledSprites[i];
            int randomIndex = UnityEngine.Random.Range(0, shuffledSprites.Count);
            shuffledSprites[i] = shuffledSprites[randomIndex];
            shuffledSprites[randomIndex] = temp;
        }
    }

    // Здесь начинается мой генератор колод

    public CardDataSO deckStyleA;
    public CardDataSO deckStyleB;

    private CardDataSO currentDeckStyle;

    public void SetCurrentDeckStyle(CardDataSO newStyle)
    {
        currentDeckStyle = newStyle;
        Debug.Log("Switched to new deck style: " + newStyle.name);


    }

    public Sprite GetPipSpriteForSuit(CardSuit suit)
    {
        switch (suit)
        {
            case CardSuit.Hearts: return currentDeckStyle.heartsPip;
            case CardSuit.Diamonds: return currentDeckStyle.diamondsPip;
            case CardSuit.Clubs: return currentDeckStyle.clubsPip;
            case CardSuit.Spades: return currentDeckStyle.spadesPip;
            default: return null;
        }
    }

    public Sprite GetEmptySpriteForSuit(CardSuit suit)
    {
        switch (suit)
        {
            case CardSuit.Hearts: return currentDeckStyle.heartsEmpty;
            case CardSuit.Diamonds: return currentDeckStyle.diamondsEmpty;
            case CardSuit.Clubs: return currentDeckStyle.clubsEmpty;
            case CardSuit.Spades: return currentDeckStyle.spadesEmpty;
            default: return null;
        }
    }

    public Material GetTextMaterialForSuit(CardSuit suit)
    {
        switch (suit)
        {
            case CardSuit.Hearts: return currentDeckStyle.heartsTextMat;
            case CardSuit.Diamonds: return currentDeckStyle.diamondTextMat;
            case CardSuit.Clubs: return currentDeckStyle.clubsTextMat;
            case CardSuit.Spades: return currentDeckStyle.spadesTextMat;
            default: return null;
        }
    }

    public void GenerateDeck()
    {
        SpriteMapInit();
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                GameObject card = Instantiate(cardPrefabs[(int)rank], container);
                card.name = rank + "_" + suit.ToString();
                card.GetComponent<RectTransform>().position = new Vector2((int)suit * 1.5f, ((int)rank) * 2);
                if (card.TryGetComponent<CardDisplay>(out var cardComponent))
                {
                    Debug.Log("Card initialized");
                    cardComponent.InitializeCard(suit, rank);
                }
            }
        }
    }

    public enum CardSuit
    {
        Diamonds,
        Spades,
        Clubs,
        Hearts
    }

    public enum Rank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    private Dictionary<CardSuit, Dictionary<Rank, Sprite>> highRankCardSpritesMap;

    void SpriteMapInit()
    {
        highRankCardSpritesMap = new();

        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            highRankCardSpritesMap[suit] = new Dictionary<Rank, Sprite>();
        }

        foreach (var s in deckStyleA.highRankSpriteStructs)
        {
            if (s.sprite != null)
            {
                highRankCardSpritesMap[s.suit][s.rank] = s.sprite;
                Debug.Log($"{s.sprite} highRankCardSpritesMap[s.suit][s.rank] + {s.rank} {s.suit}");
            }
        }
    }

    public Sprite GetHighRankSprite(CardSuit suit, Rank rank)
    {
        if (highRankCardSpritesMap.ContainsKey(suit) && highRankCardSpritesMap[suit].ContainsKey(rank))
        {
            Debug.Log($"{rank} {suit}");
            Debug.Log(highRankCardSpritesMap[suit][rank]);
            return highRankCardSpritesMap[suit][rank];
        }
        return null;
    }
}

public class Card : MonoBehaviour
{
    public Sprite originalSprite;
}
