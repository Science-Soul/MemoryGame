using System.Collections.Generic;
using UnityEngine;

public static class PlayerAchievments
{
    const int BASE_EXP = 100;
    const float EXP_MULTIPLIER = 1.05f;
    const int EXP_FOR_LEVEL_COMPLETE = 100;
    const int MASTERY_DELTA = 10;
    const int MAX_LEVEL = 100;

    private static int currentExpForLevelUp = BASE_EXP;
    private static int currentLevel = 1;
    private static string currentMastery;

    private static readonly string[] PLAYER_MASTERIES = new string[11]
    {
        "Новичок",
        "Ученик",
        "Знаток",
        "Хранитель образов",
        "Архивариус",
        "Магистр",
        "Мастер",
        "Мнемонист",
        "Мудрец",
        "Оракул",
        "Великий"
    };

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
        currentExpForLevelUp = BASE_EXP;
        currentLevel = 1;
        currentMastery = PLAYER_MASTERIES[0];
    }

    public static void ExpAdd()
    {
        Exp += EXP_FOR_LEVEL_COMPLETE * 10000;
        Debug.Log("Current exp: " + exp);
        LevelUp();
    }

    private static void LevelUp()
    {
        if (exp >= currentExpForLevelUp && currentLevel < MAX_LEVEL)
        {
            currentLevel++;
            Debug.Log("Новый уровень: " +  currentLevel);

            MasteryUp();

            // Увеличиваем количество опыта, необходимого для следующего уровня
            currentExpForLevelUp += (int)(BASE_EXP * Mathf.Pow(EXP_MULTIPLIER, currentLevel));
            Debug.Log("До следующего уровня: " + currentExpForLevelUp);

            LevelUp(); // Рекурсивно повышаем уровень, пока очки опыта не уравновесятся
        }
    }

    private static void MasteryUp()
    {
        int masteryIndex = Mathf.FloorToInt(currentLevel / MASTERY_DELTA);

        if (masteryIndex > PLAYER_MASTERIES.Length - 1)
        {
            return;
        }

        if (currentMastery != PLAYER_MASTERIES[masteryIndex])
        {
            currentMastery = PLAYER_MASTERIES[masteryIndex];
            Debug.Log("Новый ранг: " + currentMastery);
        }
    }
}
