using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "BonusCardDeck", menuName = "Game/BonusCardDeck")]
public class BonusCardDeckSO : ScriptableObject
{
    public List<BonusCard> bonusCards;

    public bool IsComplete => bonusCards.All(c => c.IsUnlocked);
}
