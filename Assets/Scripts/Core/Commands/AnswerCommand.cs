using System;

namespace Core.Commands
{
    public class AnswerCommand : ICommand
    {
        private readonly Action _callback;

        public AnswerCommand(Action callback)
        {
            _callback = callback;
        }

        public void Do()
        {
            _callback.Invoke();
        }
    }
}