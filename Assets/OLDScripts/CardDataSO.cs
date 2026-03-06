using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MemoryGame;


[CreateAssetMenu(fileName = "NewDeckStyle", menuName = "Game/Deck Style")]
public class CardDataSO : ScriptableObject
{
    public Sprite back;

    [Space]
    public Sprite crestsPip;
    public Sprite diamondsPip;
    public Sprite heartsPip;
    public Sprite spadesPip;

    [Space]
    public Sprite crestsEmpty;
    public Sprite diamondsEmpty;
    public Sprite heartsEmpty;
    public Sprite spadesEmpty;

    [Space]
    public TMP_FontAsset font;
    public Material crestsTextMat;
    public Material diamondTextMat;
    public Material heartsTextMat;
    public Material spadesTextMat;

    [Serializable]
    public struct HighRankCradSprites
    {
        public CardSuit suit;
        public Rank rank;
        public Sprite sprite;
    }

    public List<HighRankCradSprites> highRankSpriteStructs = new(16);
}
