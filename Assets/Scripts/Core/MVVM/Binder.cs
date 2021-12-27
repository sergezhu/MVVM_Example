using Core.Input;
using UnityEngine;

namespace Core.MVVM
{
    public class Binder : MonoBehaviour
    {
        [SerializeField]
        private CalcButtonsInput _calcButtonsInput;

        [Space]
        [SerializeField]
        private DisplayView _displayView;
        [SerializeField]
        private DisplayViewModel _displayViewModel;
        
        [Space]
        [SerializeField]
        private ButtonView _buttonViewNum0;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum0;

        [SerializeField]
        private ButtonView _buttonViewNum1;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum1;

        [SerializeField]
        private ButtonView _buttonViewNum2;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum2;

        [SerializeField]
        private ButtonView _buttonViewNum3;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum3;

        [SerializeField]
        private ButtonView _buttonViewNum4;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum4;

        [SerializeField]
        private ButtonView _buttonViewNum5;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum5;

        [SerializeField]
        private ButtonView _buttonViewNum6;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum6;

        [SerializeField]
        private ButtonView _buttonViewNum7;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum7;

        [SerializeField]
        private ButtonView _buttonViewNum8;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum8;

        [SerializeField]
        private ButtonView _buttonViewNum9;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelNum9;

        [SerializeField]
        private ButtonView _buttonViewAdd;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelAdd;

        [SerializeField]
        private ButtonView _buttonViewSubtract;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelSubtract;

        [SerializeField]
        private ButtonView _buttonViewMultiply;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelMultiply;

        [SerializeField]
        private ButtonView _buttonViewDivide;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelDivide;

        [SerializeField]
        private ButtonView _buttonViewAnswer;
        [SerializeField]
        private KeyButtonViewModel _buttonViewModelAnswer;

        public void Bind()
        {
            _buttonViewModelNum0.Construct(_buttonViewNum0, OperationType.Num0, EventHandler);
            _buttonViewModelNum1.Construct(_buttonViewNum1, OperationType.Num1, EventHandler);
            _buttonViewModelNum2.Construct(_buttonViewNum2, OperationType.Num2, EventHandler);
            _buttonViewModelNum3.Construct(_buttonViewNum3, OperationType.Num3, EventHandler);
            _buttonViewModelNum4.Construct(_buttonViewNum4, OperationType.Num4, EventHandler);
            _buttonViewModelNum5.Construct(_buttonViewNum5, OperationType.Num5, EventHandler);
            _buttonViewModelNum6.Construct(_buttonViewNum6, OperationType.Num6, EventHandler);
            _buttonViewModelNum7.Construct(_buttonViewNum7, OperationType.Num7, EventHandler);
            _buttonViewModelNum8.Construct(_buttonViewNum8, OperationType.Num8, EventHandler);
            _buttonViewModelNum9.Construct(_buttonViewNum9, OperationType.Num9, EventHandler);
            _buttonViewModelAdd.Construct(_buttonViewAdd, OperationType.Add, EventHandler);
            _buttonViewModelSubtract.Construct(_buttonViewSubtract, OperationType.Subtract, EventHandler);
            _buttonViewModelMultiply.Construct(_buttonViewMultiply, OperationType.Multiply, EventHandler);
            _buttonViewModelDivide.Construct(_buttonViewDivide, OperationType.Divide, EventHandler);
            _buttonViewModelAnswer.Construct(_buttonViewAnswer, OperationType.Answer, EventHandler);
            
            _displayView.Subscribe(_displayViewModel.Calculator.DisplayData, DisplayDataEventHandler);
            _displayView.Subscribe(_displayViewModel.Calculator.HasErrors, ErrorsHandler);
            
            _buttonViewAdd.Subscribe(_displayViewModel.Calculator.LockedData, LockedDataEventAddButtonHandler);
            _buttonViewSubtract.Subscribe(_displayViewModel.Calculator.LockedData, LockedDataEventSubtractButtonHandler);
            _buttonViewMultiply.Subscribe(_displayViewModel.Calculator.LockedData, LockedDataEventMultiplyButtonHandler);
            _buttonViewDivide.Subscribe(_displayViewModel.Calculator.LockedData, LockedDataEventDivideButtonHandler);
        }

        private void EventHandler(OperationType type) => 
            _calcButtonsInput.Operation.Value = type;

        private void DisplayDataEventHandler()
        {
            _displayView.UpdateDisplay(_displayViewModel.Calculator.DisplayData.Value);
        }

        private void LockedDataEventAddButtonHandler()
        {
            var lockedData = _displayViewModel.Calculator.LockedData.Value;
            if (lockedData.AddLocked)
                _buttonViewAdd.DisableInteractable();
            else
                _buttonViewAdd.EnableInteractable();
        }

        private void LockedDataEventSubtractButtonHandler()
        {
            var lockedData = _displayViewModel.Calculator.LockedData.Value;
            if (lockedData.SubtractLocked)
                _buttonViewSubtract.DisableInteractable();
            else
                _buttonViewSubtract.EnableInteractable();
        }

        private void LockedDataEventMultiplyButtonHandler()
        {
            var lockedData = _displayViewModel.Calculator.LockedData.Value;
            if (lockedData.MultiplyLocked)
                _buttonViewMultiply.DisableInteractable();
            else
                _buttonViewMultiply.EnableInteractable();
        }

        private void LockedDataEventDivideButtonHandler()
        { 
            var lockedData = _displayViewModel.Calculator.LockedData.Value;
            if (lockedData.DivideLocked)
                _buttonViewDivide.DisableInteractable();
            else
                _buttonViewDivide.EnableInteractable();
        }
        
        private void ErrorsHandler()
        {
            _displayView.UpdateErrorsTitle(_displayViewModel.Calculator.HasErrors.Value);
        }
    }
}