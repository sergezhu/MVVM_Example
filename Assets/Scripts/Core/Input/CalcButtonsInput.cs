using Core.MVVM;
using UnityEngine;

namespace Core.Input
{
    public class CalcButtonsInput : MonoBehaviour
    {
        public readonly ReactiveProperty<OperationType> Operation = new ReactiveProperty<OperationType>();
    }
}