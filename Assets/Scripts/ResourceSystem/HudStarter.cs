using Zenject;
using UnityEngine;

namespace Assets.Scripts
{
    public class HudStarter : IInitializable
    {
        private readonly DiContainer _container;
        private readonly GameObject _hudPrefab;

        public HudStarter(DiContainer container, [InjectOptional] GameObject hudPrefab)
        {
            _container = container;
            _hudPrefab = hudPrefab;
        }

        public void Initialize()
        {
            if (_hudPrefab == null) return;

            // Создаем префаб ПОСЛЕ завершения всех инсталлеров
            var hudInstance = _container.InstantiatePrefab(_hudPrefab);

            Debug.Log("<color=green>[HudStarter]</color> HUD создан безопасно!");
        }
    }
}