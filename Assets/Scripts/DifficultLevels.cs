using UnityEngine;


[CreateAssetMenu(fileName = "DifficultLevel", menuName = "Game/DifficultLevel")]
public class DifficultLevels : ScriptableObject
{
    [SerializeField] int numberOfCardsOnDesk;
    public int NumberOfCardsOnDesk
    {
        get { return numberOfCardsOnDesk; }
    }

    [SerializeField] int numberOfRows;
    public int NumberOfRows
    {
        get { return numberOfRows; }
    }
}
