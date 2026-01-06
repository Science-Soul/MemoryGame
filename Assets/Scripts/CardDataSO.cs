using UnityEngine;


[CreateAssetMenu(fileName = "NewDeckStyle", menuName = "Game/Deck Style")]
public class CardDataSO : ScriptableObject
{
    public Sprite heartsPip;
    public Sprite diamondsPip;
    public Sprite clubsPip;
    public Sprite spadesPip;

    [Space]
    public Sprite heartsEmpty;
    public Sprite diamondsEmpty;
    public Sprite clubsEmpty;
    public Sprite spadesEmpty;
}
