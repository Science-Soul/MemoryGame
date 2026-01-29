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

    [SerializeField] float gridScale;
    public float GridScale
    {
        get { return gridScale; }
    }
}
