using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "BonusCardDeck", menuName = "Game/BonusCardDeck")]
public class BonusCardDeckSO : ScriptableObject
{
    public List<BonusCard> bonusCards;

    public bool IsComplete
    {
        get
        {
            return bonusCards.All(c => c.IsUnlocked);
        }
        set
        {
            IsComplete = true;
        }
    }
}
