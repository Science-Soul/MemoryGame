using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Game/FullDecks")]
public class Deck : ScriptableObject
{
    public List<GameObject> cardPrefabs;
}
