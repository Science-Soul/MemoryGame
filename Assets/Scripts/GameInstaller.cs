using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] List<BonusCardDeckSO> _bonusCardDecks;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.Bind<BonusPopup>().FromComponentInHierarchy().AsSingle();
        Container.BindInstance(_bonusCardDecks).AsSingle();
        Container.BindInterfacesAndSelfTo<BonusService>().AsSingle();

        Container.BindInterfacesAndSelfTo<CollectionService>().AsSingle();


        Container.DeclareSignal<BonusCollectedSignal>().OptionalSubscriberWithWarning();
    }
}
