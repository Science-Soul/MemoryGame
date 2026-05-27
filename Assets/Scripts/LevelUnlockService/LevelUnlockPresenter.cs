using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockPresenter
{
    private LevelUnlockModel _model;
    private LevelUnlockView _view;
    //private List<LevelUnlockView> _levelUnlockButtons;

    public LevelUnlockPresenter(LevelUnlockModel model, LevelUnlockView view)
    {
        _view = view;
        _model = model;
        UpdateView();
    }

    private void UpdateView()
    {
        var levelSettings = _view.GetLevelSettings();
        _view.SetInteractable(_model.IsResourcesEnough(levelSettings));
        _view.SetState(_model.IsUnlocked(levelSettings));
        Debug.Log("Presenter initialized");
    }
}
