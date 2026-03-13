using UnityEngine;

[CreateAssetMenu(fileName = "DifficultLevel", menuName = "Game/DifficultLevel")]
public class DifficultyLevels : ScriptableObject
{
    [SerializeField] int numberOfCardsOnDesk = 12;
    public int NumberOfCardsOnDesk
    {
        get { return numberOfCardsOnDesk; }
    }

    [SerializeField] int numberOfRows = 3;
    public int NumberOfRows
    {
        get { return numberOfRows; }
    }

    [SerializeField] float gridScale = 1;
    public float GridScale
    {
        get { return gridScale; }
    }

    [SerializeField] int baseBonusTime = 10;
    public int BaseBonusTime
    {
        get { return baseBonusTime; }
    }

    [SerializeField] int numberOfCardsToSearch = 2;
    public int NumberOfCardsToSearch
    {
        get { return numberOfCardsToSearch; }
    }
}
