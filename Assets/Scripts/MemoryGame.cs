using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MemoryGame : MonoBehaviour
{
    public RectTransform container;
    public GameObject cardPrefab;
    public int rows = 4;
    public int columns = 4;
    public float spacing = 10f;
    public float bounceDuration = 0.5f;
    public List<Sprite> cardSprites;
    public Sprite backSprite;

    private List<Sprite> shuffledSprites;
    private List<GameObject> openedCards = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateShuffledSpriteList();
        FillGridWithCards();
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
                GameObject newCard = Instantiate(cardPrefab, container);
                RectTransform cardTransform = newCard.GetComponent<RectTransform>();
                Image cardImage = newCard.GetComponent<Image>();
                Button cardButton = newCard.GetComponent<Button>();

                cardTransform.sizeDelta = new Vector2(cardSize, cardSize);

                Vector2 cardPosition = startOffset + new Vector2(
                    col * (cardSize + spacing),
                    -row * (cardSize + spacing));

                cardTransform.anchoredPosition = cardPosition;
                cardTransform.localScale = Vector3.zero;

                if (cardImage != null && spriteIndex < shuffledSprites.Count)
                {
                    cardImage.sprite = shuffledSprites[spriteIndex];
                    Card cardScript = newCard.AddComponent<Card>();
                    cardScript.originalSprite = shuffledSprites[spriteIndex];
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

                    if (openedCards.Count == 2) {
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

                cardTransform.DOScale (Vector3.zero, bounceDuration).SetEase (Ease.OutCirc)
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

        shuffledSprites = new List<Sprite>();

        for (int i = 0; i < rows * columns / 2; i++)
        {
            shuffledSprites.Add(cardSprites[i]);
            shuffledSprites.Add(cardSprites[i]);
        }

        for (int i = 0; i < shuffledSprites.Count; i++)
        {
            Sprite temp = shuffledSprites[i];
            int randomIndex = Random.Range(0, shuffledSprites.Count);
            shuffledSprites[i] = shuffledSprites[randomIndex];
            shuffledSprites[randomIndex] = temp;
        }
    }
}

public class Card : MonoBehaviour
{
    public Sprite originalSprite;
}
