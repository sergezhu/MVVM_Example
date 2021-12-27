using System;
using System.Collections.Generic;
using Core.Input;
using Core.MVVM;
using Core.Storage;
using UnityEngine;

namespace Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private bool _clearPlayerPrefsWhenStart;
        
        [Space]
        [SerializeField]
        private Ticker _ticker;
        [SerializeField]
        private Binder _binder;
        [SerializeField]
        private CalcButtonsInput _calcButtonsInput;
        [SerializeField]
        private DisplayViewModel _displayViewModel;
        
        private SimpleKeyboardInput _simpleKeyboardInput;
        private IStorage _storage;
        private Calculator _calculator;

        private void Awake()
        {
            _simpleKeyboardInput = new SimpleKeyboardInput();
            _calculator = new Calculator(_simpleKeyboardInput, _calcButtonsInput);
            _displayViewModel.Construct(_calculator);
            
            _binder.Bind();
            _calculator.ForceReset();

            _storage = new PlayerPrefsStorage(_calculator);
            var playerPrefsStorage = _storage is PlayerPrefsStorage ? (PlayerPrefsStorage) _storage : null;
            
            if(_clearPlayerPrefsWhenStart && playerPrefsStorage != null)
                playerPrefsStorage.ClearPlayerPrefs();
            
            _storage.Load();
            if(playerPrefsStorage != null)
                playerPrefsStorage.BeginObserve();
                
            _ticker.Run(new List<Action>()
            {
                () => {_simpleKeyboardInput.DoUpdate();}
            });
        }
        
        
    }
}