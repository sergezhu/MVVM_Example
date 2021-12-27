using System;

namespace Core.MVVM
{
    public interface IChangeEventHolder
    {
        event Action Changed;
    }
}