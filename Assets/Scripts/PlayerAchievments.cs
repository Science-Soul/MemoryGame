using UnityEngine;

public static class PlayerAchievments
{
    private static float exp;
    public static float Exp
    {
        get { return exp; }
        set { exp = value; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        exp = 0;
    }

    public static void ExpAdd()
    {
        Exp++;
    }
}
