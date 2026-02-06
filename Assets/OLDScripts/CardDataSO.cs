using System;
using System.Collections.Generic;
using UnityEngine;
using static MemoryGame;


[CreateAssetMenu(fileName = "NewDeckStyle", menuName = "Game/Deck Style")]
public class CardDataSO : ScriptableObject
{
    public Sprite back;

    [Space]
    public Sprite heartsPip;
    public Sprite diamondsPip;
    public Sprite clubsPip;
    public Sprite spadesPip;

    [Space]
    public Sprite heartsEmpty;
    public Sprite diamondsEmpty;
    public Sprite clubsEmpty;
    public Sprite spadesEmpty;

    [Space]
    public Material heartsTextMat;
    public Material diamondTextMat;
    public Material clubsTextMat;
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
