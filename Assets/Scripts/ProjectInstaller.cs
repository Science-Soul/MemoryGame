using Zenject;
using UnityEngine;
namespace Assets.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] GameObject hudPrefab;
        [SerializeField] LevelSettings levelSettings;

        public override void InstallBindings()
        {
            Container.Bind<ISaveStorage>().To<LocalJsonStorage>().AsSingle();

            Container.BindInstance(levelSettings).AsSingle();
            Container.Bind<ResourceModel>().AsSingle().NonLazy();
            Container.Bind<LevelUnlockModel>().AsSingle().NonLazy();
            Container.BindInstance(hudPrefab).AsSingle();
            Container.BindInterfacesTo<HudStarter>().AsSingle();
        }
    }
}