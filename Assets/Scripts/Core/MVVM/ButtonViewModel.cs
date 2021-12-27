using System;

namespace Core.MVVM
{
    public abstract class ButtonViewModel<TInput> : ViewModel
    {
        private IClickEventHolder _clickEventHolder;
        private Action<TInput> _eventHandler;
        private TInput _eventArg;

        public void Construct(IClickEventHolder clickEventHolder, TInput eventArg, Action<TInput> eventHandler)
        {
            _eventArg = eventArg;
            _eventHandler = eventHandler;
            _clickEventHolder = clickEventHolder;
            
            Subscribe();
        }

        private bool IsSubscribed { get; set; }

        private void Subscribe()
        {
            if (IsSubscribed)
                return;

            IsSubscribed = true;

            SubscribeBehaviour();
        }
        
        protected void Unsubscribe()
        {
            if (IsSubscribed == false)
                return;

            IsSubscribed = false;
            
            UnsubscribeBehaviour();
        }

        private void OnClick()
        {
            _eventHandler.Invoke(_eventArg);
        }

        private void SubscribeBehaviour()
        {
            _clickEventHolder.Click += OnClick;
        }

        private void UnsubscribeBehaviour()
        {
            _clickEventHolder.Click -= OnClick;
        }
    }
}