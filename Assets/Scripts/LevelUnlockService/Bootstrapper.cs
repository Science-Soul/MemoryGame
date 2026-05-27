using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    //[SerializeField] List<LevelUnlockView> _levelUnlockButtons; 
    [SerializeField] LevelUnlockView _view;
    [Inject] LevelUnlockModel _model;
    void Awake()
    {
        LevelUnlockPresenter _presenter = new LevelUnlockPresenter(_model, _view);
        
        Debug.Log("MVP initialized");
    }
}
