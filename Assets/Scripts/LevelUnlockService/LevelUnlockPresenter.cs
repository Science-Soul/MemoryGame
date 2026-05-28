using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockPresenter : IDisposable
{
    private LevelUnlockModel _model;
    private LevelUnlockView _view;
    //private List<LevelUnlockView> _levelUnlockButtons;

    public LevelUnlockPresenter(LevelUnlockModel model, LevelUnlockView view)
    {
        _model = model;
        _view = view;

        _view.OnClick += OnBuyLevel;

        UpdateView();
    }

    private void UpdateView()
    {
        //var levelSettings = _view.GetLevelSettings();
        _view.SetInteractable(_model.IsResourcesEnough());
        _view.SetState(_model.IsUnlocked());
        Debug.Log("Presenter initialized");
    }

    private void OnBuyLevel()
    {
        _model.TryUnlockLevel();
        UpdateView();
    }

    public void Dispose()
    {
        _view.OnClick -= OnBuyLevel;
    }
}
