using System;

namespace Core.MVVM
{
    public interface IClickEventHolder
    {
        event Action Click;
    }
}