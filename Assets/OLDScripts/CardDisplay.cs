using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static MemoryGame;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> digits;
    [SerializeField] Image characterImage;
    public List<Image> pipImages;
    private Image face;
    public MemoryGame.CardSuit Suit;
    public MemoryGame.Rank Rank;

    private MemoryGame gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<MemoryGame>();
        face = gameObject.GetComponent<Image>();

        TextMeshProUGUI[] textFields = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in textFields)
        {
            digits.Add(text);
        }
    }
    public void InitializeCard(MemoryGame.CardSuit suit, MemoryGame.Rank rank)
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
            Sprite desiredHighRankFace = gameManager.GetHighRankSprite(Suit, Rank);

            if (desiredPipSprite != null && desiredEmptySprite != null && desiredMat != null)
            {
                UpdateCardVisuals(desiredPipSprite, desiredEmptySprite, desiredMat);
            }
        }
    }

    public void UpdateCardVisuals(Sprite pipSprite, Sprite emptySprite, Material textMat)
    {
        foreach (Image pip in pipImages)
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
            Debug.Log($"{rank} {suit}");
        }
    }
}
