using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Storage
{
    public class PlayerPrefsStorage : IStorage
    {
        private const string PlayerPrefsID = "PlayerProgress";
        
        private readonly Calculator _calculator;

        public PlayerPrefsStorage(Calculator calculator)
        {
            _calculator = calculator;
        }

        public PlayerProgress Progress { get; private set; }

        public void BeginObserve()
        {
            Subscribe();
        }
        
        public void EndObserve()
        {
            Unsubscribe();
        }

        
        public void Save()
        {
            SaveInternal();
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsID))
                LoadInternal();
            else
                CreateNew();
            
            _calculator.SetSequence(Progress.Sequence.ToList());
        }

        private void Subscribe()
        {
            _calculator.SequenceChanged += OnSequenceChanged;
        }
        
        private void Unsubscribe()
        {
            _calculator.SequenceChanged -= OnSequenceChanged;
        }

        private void OnSequenceChanged(IEnumerable<OperationType> sequence)
        {
            Progress = new PlayerProgress(sequence);
            SaveInternal();
        }

        private void SaveInternal()
        {
            var json = JsonUtility.ToJson(Progress);
            PlayerPrefs.SetString(PlayerPrefsID, json);
        }

        private void LoadInternal()
        {
            var json = PlayerPrefs.GetString(PlayerPrefsID);
            Progress = JsonUtility.FromJson<PlayerProgress>(json);
        }

        private void CreateNew()
        {
            var sequence = _calculator.GetOperationTypesSequence();
            Progress = new PlayerProgress(sequence);
        }

        public void ClearPlayerPrefs()
        {
            if(PlayerPrefs.HasKey(PlayerPrefsID))
                PlayerPrefs.DeleteKey(PlayerPrefsID);
        }
    }
}