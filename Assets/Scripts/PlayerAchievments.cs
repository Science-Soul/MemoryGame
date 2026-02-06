using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAchievments
{
    public const int BASE_EXP = 100;
    public const int BASE_TIME_BONUS = 10;
    public const float EXP_MULTIPLIER = 1.05f;
    public const int EXP_FOR_LEVEL_COMPLETE = 100;
    public const int MASTERY_DELTA = 10;
    public const int MAX_LEVEL = 100;

    private static int currentLevel = 1;
    public static int CurrentLevel
    {
        get { return currentLevel; }
    }
    private static int currentExpForLevelUp = BASE_EXP;
    public static int CurrentExpForLevelUp { get { return currentExpForLevelUp; } }
    private static int previousExpForLevelUp = 0;
    public static int PreviousExpForLevelUp { get { return previousExpForLevelUp; } }

    private static string currentMastery;
    public static string CurrentMastery { get { return currentMastery; } }

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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        exp = PlayerPrefs.GetFloat("exp_saved", 0);
        currentExpForLevelUp = PlayerPrefs.GetInt("currentExpForLevelUp_saved", BASE_EXP);
        currentLevel = PlayerPrefs.GetInt("level_saved", 1);
        currentMastery = PlayerPrefs.GetString("mastery_saved", "1");
    }

    public static event Action ExpAdded;
    public static void ExpAdd(int expAdditional)
    {
        Exp += expAdditional;
        PlayerPrefs.SetFloat("exp_saved", exp);
        Debug.Log("Current exp: " + exp);
        LevelUp();
        ExpAdded?.Invoke();
    }

    private static void LevelUp()
    {
        if (exp >= currentExpForLevelUp && currentLevel < MAX_LEVEL)
        {
            currentLevel++;
            PlayerPrefs.SetInt("level_saved", currentLevel);
            Debug.Log("Сохранено значение уровня " +  currentLevel);
            Debug.Log("Новый уровень: " + currentLevel);

            MasteryUp();

            // Увеличиваем количество опыта, необходимого для следующего уровня
            previousExpForLevelUp = currentExpForLevelUp;
            currentExpForLevelUp += (int)(BASE_EXP * Mathf.Pow(EXP_MULTIPLIER, currentLevel));
            PlayerPrefs.SetInt("previousExpForLevelUp_saved", previousExpForLevelUp);
            PlayerPrefs.SetInt("currentExpForLevelUp_saved", currentExpForLevelUp);
            Debug.Log("До следующего уровня: " + (currentExpForLevelUp - exp));

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
            PlayerPrefs.SetString("mastery_saved", currentMastery);
            Debug.Log("Новый ранг: " + currentMastery);
        }
    }

    public static void AddTimeBonus(int expBonus)
    {
        exp += expBonus;
        Debug.Log("Бонус за время: " + expBonus);
        Debug.Log("До следующего уровня: " + (currentExpForLevelUp - exp));
    }
}
