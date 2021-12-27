using System;
using System.Collections.Generic;
using Core.MVVM;
using UnityEngine;

namespace Core.Input
{
    public class SimpleKeyboardInput
    {
        public readonly ReactiveProperty<OperationType> Operation = new ReactiveProperty<OperationType>();
    
        private readonly Dictionary<KeyCode, Action> _inputKeyActions;

        public SimpleKeyboardInput()
        {
            _inputKeyActions = new Dictionary<KeyCode, Action>()
            {
                {KeyCode.Alpha0, () => { Operation.Value = OperationType.Num0; }},
                {KeyCode.Keypad0, () => { Operation.Value = OperationType.Num0; }},

                {KeyCode.Alpha1, () => { Operation.Value = OperationType.Num1; }},
                {KeyCode.Keypad1, () => { Operation.Value = OperationType.Num1; }},

                {KeyCode.Alpha2, () => { Operation.Value = OperationType.Num2; }},
                {KeyCode.Keypad2, () => { Operation.Value = OperationType.Num2; }},

                {KeyCode.Alpha3, () => { Operation.Value = OperationType.Num3; }},
                {KeyCode.Keypad3, () => { Operation.Value = OperationType.Num3; }},

                {KeyCode.Alpha4, () => { Operation.Value = OperationType.Num4; }},
                {KeyCode.Keypad4, () => { Operation.Value = OperationType.Num4; }},

                {KeyCode.Alpha5, () => { Operation.Value = OperationType.Num5; }},
                {KeyCode.Keypad5, () => { Operation.Value = OperationType.Num5; }},

                {KeyCode.Alpha6, () => { Operation.Value = OperationType.Num6; }},
                {KeyCode.Keypad6, () => { Operation.Value = OperationType.Num6; }},

                {KeyCode.Alpha7, () => { Operation.Value = OperationType.Num7; }},
                {KeyCode.Keypad7, () => { Operation.Value = OperationType.Num7; }},

                {KeyCode.Alpha8, () => { Operation.Value = OperationType.Num8; }},
                {KeyCode.Keypad8, () => { Operation.Value = OperationType.Num8; }},

                {KeyCode.Alpha9, () => { Operation.Value = OperationType.Num9; }},
                {KeyCode.Keypad9, () => { Operation.Value = OperationType.Num9; }},

                {KeyCode.Plus, () => { Operation.Value = OperationType.Add; }},
                {KeyCode.KeypadPlus, () => { Operation.Value = OperationType.Add; }},

                {KeyCode.Minus, () => { Operation.Value = OperationType.Subtract; }},
                {KeyCode.KeypadMinus, () => { Operation.Value = OperationType.Subtract; }},

                {KeyCode.KeypadMultiply, () => { Operation.Value = OperationType.Multiply; }},

                {KeyCode.KeypadDivide, () => { Operation.Value = OperationType.Divide; }},
                
                {KeyCode.Equals, () => { Operation.Value = OperationType.Answer; }},
                {KeyCode.KeypadEquals, () => { Operation.Value = OperationType.Answer; }},
                {KeyCode.KeypadEnter, () => { Operation.Value = OperationType.Answer; }},
            };
        }

        public void DoUpdate()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                foreach (var inputKeyAction in _inputKeyActions)
                {
                    if(UnityEngine.Input.GetKeyDown(inputKeyAction.Key))
                        inputKeyAction.Value.Invoke();
                }
            }
        }
    }
}