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
    private Image back;
    private CardSuit Suit;
    private Rank Rank;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<MemoryGame>();
        face = gameObject.GetComponent<Image>();
        back = gameObject.transform.Find("back").GetComponent<Image>();
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
            Sprite desiredBackSprite = gameManager.GetBackSprite();
            Material desiredMat = gameManager.GetTextMaterialForSuit(Suit);
            TMP_FontAsset desiredFont = gameManager.GetFont();

            if (desiredPipSprite != null 
                && desiredEmptySprite != null 
                && desiredBackSprite != null 
                && desiredMat != null)
            {
                UpdateCardVisuals(desiredPipSprite, desiredEmptySprite, desiredBackSprite, desiredMat, desiredFont);
            }
        }
    }

    public void UpdateCardVisuals(Sprite pipSprite, Sprite emptySprite, Sprite backSprite, Material textMat, TMP_FontAsset font)
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
            text.font = font;
            text.fontMaterial = textMat;
        }

        face.sprite = emptySprite;
        face.SetNativeSize();
        back.sprite = backSprite;
        back.type = Image.Type.Simple;
        back.SetNativeSize();
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
