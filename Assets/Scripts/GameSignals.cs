public struct BonusCollectedSignal
{
    public BonusCard unlockedCard;
}

public struct DeckUnlockedSignal
{
    public Deck unlockedDeck;
}

public struct ChangeDeckSignal
{
    public Deck newDeck;
}

public struct StartGameSignal
{
    public DifficultyLevels selectedDifficulty;
}
