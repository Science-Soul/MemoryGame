using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockPresenter : IDisposable
{
    private LevelUnlockModel _model;
    private LevelUnlockView _view;
    private LevelSettings _level;

    public LevelUnlockPresenter(LevelUnlockModel model, LevelUnlockView view)
    {
        _model = model;
        _view = view;
        _level = _view.GetLevelSettings();
        _view.OnClick += (() => OnBuyLevel(_level));

        UpdateView();
    }

    private void UpdateView()
    {
        
        _view.SetInteractable(_model.IsResourcesEnough(_level));
        _view.SetState(_model.IsUnlocked(_level));
        Debug.Log("Presenter initialized");
    }

    private void OnBuyLevel(LevelSettings level)
    {
        _model.TryUnlockLevel(level);
        UpdateView();
    }

    public void Dispose()
    {
        _view.OnClick -= (() => OnBuyLevel(_level));
    }
}
