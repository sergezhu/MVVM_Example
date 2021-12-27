using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Commands;
using Core.Input;
using Core.MVVM;
using UnityEngine;

namespace Core
{
    public class Calculator
    {
        public const int CountForEachOperation = 3;
        public const int ErrorMessageDuration = 600;

        public event Action<IEnumerable<OperationType>> SequenceChanged;
        
        public readonly ReactiveProperty<DisplayData> DisplayData = new ReactiveProperty<DisplayData>();
        public readonly ReactiveProperty<OperationsLockedData> LockedData = new ReactiveProperty<OperationsLockedData>();
        public readonly ReactiveProperty<bool> HasErrors = new ReactiveProperty<bool>();

        private readonly SimpleKeyboardInput _simpleKeyboardInput;
        private readonly CalcButtonsInput _calcButtonsInput;
        private readonly Context<long> _context;
        private Dictionary<OperationType, ICommand> _commandsLibrary;
        private List<ICommand> _sequence;
        
        private bool _isSubscribed;

        public Calculator(SimpleKeyboardInput simpleKeyboardInput, CalcButtonsInput calcButtonsInput)
        {
            if (simpleKeyboardInput == null)
                throw new NullReferenceException("simpleKeyboardInput is null");
            
            if (calcButtonsInput == null)
                throw new NullReferenceException("calcButtonsInput is null");
            
            _simpleKeyboardInput = simpleKeyboardInput;
            _calcButtonsInput = calcButtonsInput;

            _context = new Context<long>(0, 0);
            
            SetupCommandsLibrary(_context);
            ResetSequence();
            EvaluateContext();
            Subscribe();
        }

        public void ForceReset()
        {
            ResetSequence();
            EvaluateContext();
        }

        public void OnDestroy()
        {
            Unsubscribe();
        }
        
        public IEnumerable<OperationType> GetOperationTypesSequence()
        {
            var operationTypes = _sequence
                .Select(command => _commandsLibrary.First(pair => pair.Value == command).Key)
                .ToList();
            
            return operationTypes;
        }

        public void SetSequence(List<OperationType> operationTypesSequence)
        {
            if (operationTypesSequence == null)
                throw new InvalidOperationException();
            
            ValidateSequence(operationTypesSequence);
            
            _sequence = operationTypesSequence.Select(type => _commandsLibrary[type]).ToList();
            
            EvaluateContext();
        }

        private void SetupCommandsLibrary(Context<long> context)
        {
            _commandsLibrary = new Dictionary<OperationType, ICommand>()
            {
                {OperationType.Num0, new NumberCommand(0, NumberOperationCallback)},
                {OperationType.Num1, new NumberCommand(1, NumberOperationCallback)},
                {OperationType.Num2, new NumberCommand(2, NumberOperationCallback)},
                {OperationType.Num3, new NumberCommand(3, NumberOperationCallback)},
                {OperationType.Num4, new NumberCommand(4, NumberOperationCallback)},
                {OperationType.Num5, new NumberCommand(5, NumberOperationCallback)},
                {OperationType.Num6, new NumberCommand(6, NumberOperationCallback)},
                {OperationType.Num7, new NumberCommand(7, NumberOperationCallback)},
                {OperationType.Num8, new NumberCommand(8, NumberOperationCallback)},
                {OperationType.Num9, new NumberCommand(9, NumberOperationCallback)},
                {OperationType.Add, new OperatorCommand(AddOperationCallback)},
                {OperationType.Subtract, new OperatorCommand(SubtractOperationCallback)},
                {OperationType.Multiply, new OperatorCommand(MultiplyOperationCallback)},
                {OperationType.Divide, new OperatorCommand(DivideOperationCallback)},
                {OperationType.Answer, new AnswerCommand(AnswerOperationCallback)}
            };

            void NumberOperationCallback(long value)
            {
                var newCurrentValue = _context.CurrentValue * 10 + value;

                if (int.MaxValue - newCurrentValue < 0)
                    throw new InvalidOperationException();
                
                _context.CurrentValue = newCurrentValue;
            }
            
            void AddOperationCallback() 
            {
                try
                {
                    _context.StoredValue += _context.CurrentValue;
                    _context.CurrentValue = _context.StoredValue;

                    if (int.MaxValue - _context.StoredValue < 0)
                        throw new InvalidOperationException();
                }
                catch
                {
                    HandleError();
                }
            }

            void SubtractOperationCallback()
            {
                try
                {
                    _context.StoredValue -= _context.CurrentValue;
                    _context.CurrentValue = _context.StoredValue;

                    if (int.MaxValue - _context.StoredValue < 0)
                        throw new InvalidOperationException();
                }
                catch
                {
                    HandleError();
                }
            }

            void MultiplyOperationCallback()
            {
                try
                {
                    _context.StoredValue *= _context.CurrentValue;
                    _context.CurrentValue = _context.StoredValue;

                    if (int.MaxValue - _context.StoredValue < 0)
                        throw new InvalidOperationException();
                }
                catch
                {
                    HandleError();
                }
            }

            void DivideOperationCallback()
            {
                try
                {
                    _context.StoredValue /= _context.CurrentValue;
                    _context.CurrentValue = _context.StoredValue;
                }
                catch
                {
                    HandleError();
                }
            }
            
            void AnswerOperationCallback() 
            {
                _context.StoredValue = 0;
                _context.CurrentValue = _context.StoredValue;
            }
        }

        private void Subscribe()
        {
            if (_isSubscribed)
                return;
            
            _isSubscribed = true;
            
            _simpleKeyboardInput.Operation.Changed += OnOperationRequestFromKeyboard;
            _calcButtonsInput.Operation.Changed += OnOperationRequestFromButtons;
        }
        
        private void Unsubscribe()
        {
            if (_isSubscribed == false)
                return;
            
            _isSubscribed = false;
            
            _simpleKeyboardInput.Operation.Changed -= OnOperationRequestFromKeyboard;
            _calcButtonsInput.Operation.Changed -= OnOperationRequestFromButtons;
        }

        private void OnOperationRequestFromKeyboard()
        {
            OnOperationRequest(_simpleKeyboardInput.Operation.Value);
        }

        private void OnOperationRequestFromButtons()
        {
            OnOperationRequest(_calcButtonsInput.Operation.Value);        
        }
        
        private void ValidateSequence(IEnumerable<OperationType> operationTypesSequence)
        {
            // some validation actions e.g.:
            // - cutting of operators subsequences to 1 element 
            // - cutting of answer commands to 1 element
        }

        private void OnOperationRequest(OperationType op)
        {
            if (HasErrors.Value)
                return;
            
            var lastSequenceElement = _sequence[_sequence.Count - 1];

            if (_commandsLibrary[op] is OperatorCommand && lastSequenceElement is OperatorCommand)
            {
                _sequence.RemoveAt(_sequence.Count - 1);
                SequenceChanged?.Invoke(GetOperationTypesSequence());
            }

            LockedData.Value = GetLockedData();
            
            var canAddToSequence = true;
            canAddToSequence &= !(LockedData.Value.AddLocked && op == OperationType.Add);
            canAddToSequence &= !(LockedData.Value.SubtractLocked && op == OperationType.Subtract);
            canAddToSequence &= !(LockedData.Value.MultiplyLocked && op == OperationType.Multiply);
            canAddToSequence &= !(LockedData.Value.DivideLocked && op == OperationType.Divide);

            if (canAddToSequence)
            {
                _sequence.Add(_commandsLibrary[op]);
                SequenceChanged?.Invoke(GetOperationTypesSequence());
            }
            
            EvaluateContext();
        }

        private OperationsLockedData GetLockedData()
        {
            var lockedData = new OperationsLockedData();
            var operationTypes = GetOperationTypesSequence();

            var enumerable = operationTypes as OperationType[] ?? operationTypes.ToArray();
            lockedData.AddLocked = enumerable.Count(type => OperationType.Add == type) >= CountForEachOperation;
            lockedData.SubtractLocked = enumerable.Count(type => OperationType.Subtract == type) >= CountForEachOperation;
            lockedData.MultiplyLocked = enumerable.Count(type => OperationType.Multiply == type) >= CountForEachOperation;
            lockedData.DivideLocked = enumerable.Count(type => OperationType.Divide == type) >= CountForEachOperation;
            
            return lockedData;
        }

        private void ResetSequence()
        {
            _sequence = new List<ICommand>() { _commandsLibrary[OperationType.Num0] };
            
            SequenceChanged?.Invoke(GetOperationTypesSequence());
        }
        
        private void ResetContext()
        {
            _context.CurrentValue = 0;
            _context.StoredValue = 0;
        }

        private void HandleError()
        {
            HandleErrorsAsync();
        }

        private async void HandleErrorsAsync()
        {
            _sequence.RemoveAt(_sequence.Count - 1);
            Unsubscribe();
            EvaluateContext();
            
            HasErrors.Value = true;

            await Task.Delay(ErrorMessageDuration);

            HasErrors.Value = false;
            Subscribe();
            ResetSequence();
            ResetContext();
        }

        private void EvaluateContext()
        {
            ResetContext();

            var lastOperationType = OperationType.Num0;
            ICommand lastOperatorCommand = null;
            ICommand previousCommand = null;

            var debugSequence = string.Empty;
            
            _sequence.ToList().ForEach(command =>
            {
                var type = _commandsLibrary.First(pair => pair.Value == command).Key;
                debugSequence = $"{debugSequence} [{type}]";

                if (command is OperatorCommand operatorCommand)
                {
                    if (lastOperatorCommand == null)
                        _context.StoredValue = _context.CurrentValue;
                    else if(previousCommand != command) 
                        lastOperatorCommand.Do();

                    lastOperatorCommand = operatorCommand;
                    lastOperationType = type;
                }

                if (command is NumberCommand numberCommand)
                {
                    if (previousCommand is OperatorCommand)
                    {
                        _context.StoredValue = _context.CurrentValue;
                        _context.CurrentValue = 0;
                    }

                    if (previousCommand is AnswerCommand)
                    {
                        _context.StoredValue = 0;
                        _context.CurrentValue = 0;
                    }

                    try
                    {
                        numberCommand.Do();
                    }
                    catch
                    {
                        HandleError();
                    }
                }
                
                if (command is AnswerCommand)
                {
                    if (previousCommand is NumberCommand && lastOperatorCommand != null)
                    {
                        lastOperatorCommand.Do();
                        lastOperatorCommand = null;
                    }
                    
                    lastOperationType = OperationType.Answer;
                }

                previousCommand = command;
            });

            if (HasErrors.Value == false)
            {
                Debug.Log($"---- sequence {debugSequence}");
                DisplayData.Value = new DisplayData(_context.CurrentValue, _context.StoredValue, lastOperationType);
            }
        }
    }
}