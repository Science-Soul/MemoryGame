using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static MemoryGame;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> digits;
    public List<Image> pipImagesOnCard;
    public Button button;

    private MemoryGame gameManager;
    private Image face;
    private CardSuit Suit;
    private Rank Rank;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<MemoryGame>();
        face = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();

        TextMeshProUGUI[] textFields = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in textFields)
        {
            digits.Add(text);
        }
    }
    public void InitializeCard(CardSuit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
        SetVisuals();
        ReplaceHighRankSprite(suit, rank);
    }

    public void SetVisuals()
    {
        if (gameManager != null)
        {
            Sprite desiredPipSprite = gameManager.GetPipSpriteForSuit(Suit);
            Sprite desiredEmptySprite = gameManager.GetEmptySpriteForSuit(Suit);
            Material desiredMat = gameManager.GetTextMaterialForSuit(Suit);

            if (desiredPipSprite != null && desiredEmptySprite != null && desiredMat != null)
            {
                UpdateCardVisuals(desiredPipSprite, desiredEmptySprite, desiredMat);
            }
        }
    }

    public void UpdateCardVisuals(Sprite pipSprite, Sprite emptySprite, Material textMat)
    {
        foreach (Image pip in pipImagesOnCard)
        {
            if (pipSprite != null)
            {
                pip.sprite = pipSprite;
            }
        }

        foreach (TextMeshProUGUI text in digits)
        {
            text.fontMaterial = textMat;
        }

        face.sprite = emptySprite;
    }

    private void ReplaceHighRankSprite(CardSuit suit, Rank rank)
    {
        Sprite highRankSprite = gameManager.GetHighRankSprite(suit, rank);
        if (highRankSprite != null)
        {
            gameObject.GetComponent<Image>().sprite = highRankSprite;
        }
    }
}
