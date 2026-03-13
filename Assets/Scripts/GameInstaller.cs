using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] List<BonusCardDeckSO> _bonusCardDecks;
    [SerializeField] LevelSettings _levelSettings;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindInstance(_levelSettings).AsSingle();

        Container.DeclareSignal<BonusCollectedSignal>().OptionalSubscriberWithWarning();
        Container.DeclareSignal<ChangeDeckSignal>();
        Container.DeclareSignal<DeckUnlockedSignal>();

        Container.BindInstance(GetComponentInChildren<Desk>()).AsSingle().NonLazy();

        Container.Bind<BonusPopup>().FromComponentInHierarchy().AsSingle();
        Container.BindInstance(_bonusCardDecks).AsSingle();
        Container.BindInterfacesAndSelfTo<BonusService>().AsSingle();

        Container.BindInterfacesAndSelfTo<CollectionService>().AsSingle();
        Container.DeclareSignal<StartGameSignal>();
    }
}
