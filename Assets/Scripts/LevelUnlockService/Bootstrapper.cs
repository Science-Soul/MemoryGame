using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    //[SerializeField] LevelUnlockView _view;
    [SerializeField] List<LevelUnlockView> _views;
    [SerializeField] BonusCardDeckSO _deck;
    private List<LevelUnlockPresenter> _presenters = new();
    //private LevelUnlockPresenter _presenter;
    [Inject] LevelUnlockModel _model;

    public void Awake()
    {
        _model.Initialize(_deck);
        //_presenter = new LevelUnlockPresenter(_model, _view);
        foreach (var view in _views)
        {
            _presenters.Add(new LevelUnlockPresenter(_model, view));
        }
        
        Debug.Log("MVP initialized");
    }

    private void OnDestroy()
    {
        //_presenter.Dispose();
    }
}
