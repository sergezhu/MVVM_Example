using System;

namespace Core.MVVM
{
    public interface ISubscriber<T> where T : struct
    {
        public void Subscribe(ReactiveProperty<T> property, Action handler);
    }
}