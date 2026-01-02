using UnityEngine;
using System.Collections.Generic;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
}
