using System;
using R3;

namespace Assets.Scripts
{
    public class ResourceModel : IDisposable
    {
        private readonly ISaveStorage _storage;
        private readonly CompositeDisposable _disposables = new();

        public BindableReactiveProperty<int> Gold { get; }
        public BindableReactiveProperty<int> Food { get; }
        public BindableReactiveProperty<int> Seasons { get; }
        public BindableReactiveProperty<int> Materials { get; }
        public BindableReactiveProperty<int> Science { get; }
        public BindableReactiveProperty<int> Prediction { get; }
        public BindableReactiveProperty<int> Mana { get; }

        public enum ResourceType
        {
            GOLD,
            FOOD,
            MATERIALS,
            SEASONS,
            SCIENCE,
            PREDICTION,
            MANA,
        }

        public ResourceModel(ISaveStorage storage)
        {
            _storage = storage;
            var data = _storage.Load() ?? new SaveData();

            // Инициализируем значениями из сохранения
            Gold = new BindableReactiveProperty<int>(data.Gold);
            Food = new BindableReactiveProperty<int>(data.Food);
            Materials = new BindableReactiveProperty<int>(data.Materials);
            Seasons = new BindableReactiveProperty<int>(data.Seasons);
            Science = new BindableReactiveProperty<int>(data.Science);
            Prediction = new BindableReactiveProperty<int>(data.Prediction);
            Mana = new BindableReactiveProperty<int>(data.Mana);

            // Авто-сохранение при каждом изменении (пропускаем сохранение при загрузке)
            Gold.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Food.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Materials.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Seasons.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Science.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Prediction.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Mana.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
        }

        private void SaveProgress()
        {
            var data = _storage.Load();
            data.Gold = Gold.Value;
            data.Food = Food.Value;
            data.Materials = Materials.Value;
            data.Seasons = Seasons.Value;
            data.Science = Science.Value;
            data.Prediction = Prediction.Value;
            data.Mana = Mana.Value;
            _storage.Save(data);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void AddResource(ResourceType res, int amount)
        {
            switch (res)
            {
                case ResourceType.GOLD: AddGold(amount); 
                    break;
                case ResourceType.FOOD: AddFood(amount);
                    break;
                case ResourceType.MATERIALS: AddMaterials(amount);
                    break;
                case ResourceType.SEASONS: AddSeason(amount);
                    break;
                case ResourceType.SCIENCE: AddScience(amount);
                    break;
                case ResourceType.PREDICTION: AddPrediction(amount);
                    break;
                case ResourceType.MANA: AddMana(amount);
                    break;
                default: return;
            }
        }
        private void AddGold(int amount) => Gold.Value += amount;
        private void AddFood(int amount) => Food.Value += amount;
        private void AddMaterials(int amount) => Materials.Value += amount;
        private void AddSeason(int amount) => Seasons.Value += amount;
        private void AddScience(int amount) => Science.Value += amount;
        private void AddPrediction(int amount) => Prediction.Value += amount;
        private void AddMana(int amount) => Mana.Value += amount;
    }
}