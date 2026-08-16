using System;
using R3;
using UnityEngine;

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


        public enum LevelResourceType
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
            int seconds = 10;
            Gold.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Food.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Materials.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Seasons.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Science.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Prediction.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Mana.Skip(1).Debounce(TimeSpan.FromSeconds(seconds)).Subscribe(val => SaveProgress()).AddTo(_disposables);
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
            PlayerPrefs.Save();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void AddResource(LevelResourceType res, int amount)
        {
            switch (res)
            {
                case LevelResourceType.GOLD:
                    AddGold(amount);
                    break;
                case LevelResourceType.FOOD:
                    AddFood(amount);
                    break;
                case LevelResourceType.MATERIALS:
                    AddMaterials(amount);
                    break;
                case LevelResourceType.SEASONS:
                    AddSeason(amount);
                    break;
                case LevelResourceType.SCIENCE:
                    AddScience(amount);
                    break;
                case LevelResourceType.PREDICTION:
                    AddPrediction(amount);
                    break;
                case LevelResourceType.MANA:
                    AddMana(amount);
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


        public void TrySpendResource(LevelResourceType res, int amount)
        {
            switch (res)
            {
                case LevelResourceType.GOLD:
                    if (Gold.Value >= amount) Gold.Value -= amount;
                    break;

                case LevelResourceType.FOOD:
                    if (Food.Value >= amount) Food.Value -= amount;
                    break;

                case LevelResourceType.MATERIALS:
                    if (Materials.Value >= amount) Materials.Value -= amount;
                    break;

                case LevelResourceType.SEASONS:
                    if (Seasons.Value >= amount) Seasons.Value -= amount;
                    break;

                case LevelResourceType.SCIENCE:
                    if (Science.Value >= amount) Science.Value -= amount;
                    break;

                case LevelResourceType.PREDICTION:
                    if (Prediction.Value >= amount) Prediction.Value -= amount;
                    break;

                case LevelResourceType.MANA:
                    if (Mana.Value >= amount) Mana.Value -= amount;
                    break;

                default: return;
            }
        }

        public int GetResourceAvailable(LevelResourceType type)
        {
            return type switch
            {
                LevelResourceType.GOLD => Gold.Value,
                LevelResourceType.FOOD => Food.Value,
                LevelResourceType.SCIENCE => Science.Value,
                LevelResourceType.PREDICTION => Prediction.Value,
                LevelResourceType.MATERIALS => Materials.Value,
                LevelResourceType.SEASONS => Seasons.Value,
                LevelResourceType.MANA => Mana.Value,
                _ => 0,
            };
        }
    }
}