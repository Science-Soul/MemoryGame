using UnityEngine;
using UnityEngine.AddressableAssets;

public class BonusCard : MonoBehaviour
{
    public bool IsUnlocked = false;
    //public bool IsUnlocked { get { return isUnlocked; } }

    public void UnlockCard()
    {
        IsUnlocked = true;
    }
}
