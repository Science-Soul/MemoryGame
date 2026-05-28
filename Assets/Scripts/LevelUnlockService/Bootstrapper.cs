using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] LevelSettings _level;
    [SerializeField] LevelUnlockView _view;
    private LevelUnlockPresenter _presenter;
    [Inject] LevelUnlockModel _model;

    public void Awake()
    {
        _view.Initialize(_level);
        _presenter = new LevelUnlockPresenter(_model, _view);
        
        Debug.Log("MVP initialized");
    }

    private void OnDestroy()
    {
        _presenter.Dispose();
    }
}
