using Zenject;
using UnityEngine;
namespace Assets.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] GameObject hudPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ISaveStorage>().To<LocalJsonStorage>().AsSingle();

            Container.Bind<ResourceModel>().AsSingle();
            Container.Bind<LevelUnlockService>().AsSingle();
            Container.BindInstance(hudPrefab).WhenInjectedInto<HudStarter>();
            Container.BindInterfacesTo<HudStarter>().AsSingle();
        }
    }
}