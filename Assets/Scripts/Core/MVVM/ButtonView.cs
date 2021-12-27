using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVVM
{
    [RequireComponent(typeof(Button))]
    public class ButtonView : MonoBehaviour, IClickEventHolder, ISubscriber<OperationsLockedData>
    {
        public event Action Click;
        
        [SerializeField]
        private Button _button;
        
        private ReactiveProperty<OperationsLockedData> _lockedProperty;
        private Action _handler;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickHandler);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickHandler);
        }

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void ClickHandler()
        {
            Click?.Invoke();
        }

        public void Subscribe(ReactiveProperty<OperationsLockedData> property, Action handler)
        {
            _handler = handler;
            _lockedProperty = property;
            _lockedProperty.Changed += _handler;
        }
        
        public void Unsubscribe()
        {
            _lockedProperty.Changed -= _handler;
            _handler = null;
            _lockedProperty = null;
        }

        public void EnableInteractable()
        {
            _button.interactable = true;
        }
        
        public void DisableInteractable()
        {
            _button.interactable = false;
        }
    }
}