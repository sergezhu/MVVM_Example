using System;

namespace Core.MVVM
{
    public class ReactiveProperty<TValue> : IChangeEventHolder where TValue : struct
    {
        public event Action Changed;
        
        private TValue _value;

        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke();
            }
        }
    }
}