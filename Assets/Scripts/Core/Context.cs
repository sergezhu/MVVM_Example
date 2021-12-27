using System;

namespace Core
{
    public class Context<T> where T : IComparable
    {
        public Context(T storedValue, T currentValue)
        {
            StoredValue = storedValue;
            CurrentValue = currentValue;
        }

        public T StoredValue { get; set; }
        public T CurrentValue { get; set; }
    }
}