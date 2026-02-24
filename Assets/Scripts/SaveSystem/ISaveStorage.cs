public interface ISaveStorage
{
    void Save(SaveData data);
    SaveData Load();
}
