using System;
using TMPro;
using UnityEngine;

namespace Core.MVVM
{
    public class DisplayView : MonoBehaviour, ISubscriber<DisplayData>, ISubscriber<bool>
    {
        [SerializeField]
        private TMP_Text _numbersValue;
        [SerializeField]
        private TMP_Text _memoryValue;

        [Space]
        [SerializeField]
        private TMP_Text _addSign;
        [SerializeField]
        private TMP_Text _subtractSign;
        [SerializeField]
        private TMP_Text _multiplySign;
        [SerializeField]
        private TMP_Text _divideSign;

        private ReactiveProperty<DisplayData> _resultProperty;
        private ReactiveProperty<bool> _errorProperty;
        private Action _handler;
        private Action _errorHandler;

        public TMP_Text NumbersValue => _numbersValue;
        public TMP_Text MemoryValue => _memoryValue;

        public void Subscribe(ReactiveProperty<DisplayData> resultProperty, Action handler)
        {
            _handler = handler;
            _resultProperty = resultProperty;
            _resultProperty.Changed += _handler;
        }

        public void Subscribe(ReactiveProperty<bool> hasErrorsProperty, Action handler)
        {
            _errorHandler = handler;
            _errorProperty = hasErrorsProperty;
            _errorProperty.Changed += _errorHandler;
        }

        public void UpdateDisplay(DisplayData data)
        {
            NumbersValue.text = data.CurrentNumber.ToString();
            MemoryValue.text = $"M {data.MemoryNumber.ToString()}";
            UpdateOperationSign(data.OperationType);
        }

        public void UpdateErrorsTitle(bool hasErrors)
        {
            if (hasErrors == false) 
                _memoryValue.text = "0";

            _numbersValue.text = hasErrors ? "Invalid Number!" : "0";
            _numbersValue.fontSize = hasErrors ? 90 : 125;
            
            _memoryValue.gameObject.SetActive(hasErrors == false);
        }

        private void UpdateOperationSign(OperationType operationType)
        {
            _addSign.gameObject.SetActive(operationType == OperationType.Add);
            _subtractSign.gameObject.SetActive(operationType == OperationType.Subtract);
            _multiplySign.gameObject.SetActive(operationType == OperationType.Multiply);
            _divideSign.gameObject.SetActive(operationType == OperationType.Divide);
        }
    }
}