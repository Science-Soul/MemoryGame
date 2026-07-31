using UnityEngine;

public class BonusCard : MonoBehaviour
{
    public bool IsUnlocked = false;


    public void UnlockCard()
    {
        IsUnlocked = true;
    }
}
