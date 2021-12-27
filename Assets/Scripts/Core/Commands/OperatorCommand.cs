using System;

namespace Core.Commands
{
    public class OperatorCommand : ICommand
    {
        private readonly Action _callback;

        public OperatorCommand(Action callback)
        {
            _callback = callback;
        }

        public void Do()
        {
            _callback.Invoke();
        }
    }
}