using System;

namespace Core.Commands
{
    public class NumberCommand : ICommand
    {
        private readonly long _value;
        private readonly Action<long> _action;

        public NumberCommand(long value, Action<long> action)
        {
            _action = action;
            _value = value;
        }

        public void Do()
        {
            NumberCommandAction();
        }
        
        private void NumberCommandAction()
        {
            _action.Invoke(_value);
        }
    }
}