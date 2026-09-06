using System;
using System.Collections.Generic;
using System.Linq;
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
        _view.OnClick += OnBuyLevelHandler;

        UpdateView();
    }

    private void UpdateView()
    {
        
        _view.SetInteractable(_model.IsResourcesEnough(_level));
        _view.SetState(_model.IsUnlocked(_level));
        _view.SetText(string.Join("\n", _level.resourcesCost.Select(x => $"{x.type}: {x.amount}")) + $"\nКарт из набора {_level.levelType}: {_model.NumberOfCardsInDeckComplete(_level).ToString()}");
    }

    private void OnBuyLevel(LevelSettings level)
    {
        _model.TryUnlockLevel(level);
        UpdateView();
    }

    public void Dispose()
    {
        _view.OnClick -= OnBuyLevelHandler;
    }

    private void OnBuyLevelHandler() => OnBuyLevel(_level);

}
