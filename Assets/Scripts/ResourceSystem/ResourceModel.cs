using System;
using R3;

namespace Assets.Scripts
{
    public class ResourceModel : IDisposable
    {
        private readonly ISaveStorage _storage;
        private readonly CompositeDisposable _disposables = new();

        public BindableReactiveProperty<int> Gold { get; }
        public BindableReactiveProperty<int> Wood { get; }

        public ResourceModel(ISaveStorage storage)
        {
            _storage = storage;
            var data = _storage.Load();

            // Инициализируем значениями из сохранения
            Gold = new BindableReactiveProperty<int>(data.Gold);
            Wood = new BindableReactiveProperty<int>(data.Wood);

            // Авто-сохранение при каждом изменении (пропускаем сохранение при загрузке)
            Gold.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
            Wood.Skip(1).Debounce(TimeSpan.FromSeconds(1)).Subscribe(val => SaveProgress()).AddTo(_disposables);
        }

        private void SaveProgress()
        {
            var data = _storage.Load();
            data.Gold = Gold.Value;
            data.Wood = Wood.Value;
            _storage.Save(data);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void AddGold(int amount) => Gold.Value += amount;
        public void AddWood(int amount) => Wood.Value += amount;
    }
}