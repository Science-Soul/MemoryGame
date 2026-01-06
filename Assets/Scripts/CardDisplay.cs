using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class CardDisplay : MonoBehaviour
{
    public List<Image> pipImages;
    public string Suit;

    private MemoryGame gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<MemoryGame>();
    }
    public void InitializeCard(string suit)
    {
        Suit = suit;
        SetVisuals();
    }

    public void SetVisuals()
    {
        if (gameManager != null)
        {
            Sprite desiredSprite = gameManager.GetPipSpriteForSuit(Suit);

            if (desiredSprite != null)
            {
                UpdateCardVisuals(desiredSprite);
            }
        }
    }

    public void UpdateCardVisuals(Sprite pipSprite)
    {
        foreach (Image pip in pipImages)
        {
            if (pipSprite != null)
            {
                pip.sprite = pipSprite;
            }
        }
    }
}
