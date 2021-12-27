using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Ticker : MonoBehaviour
    {
        public bool _isActive;
        private List<Action> _onUpdateCallbacks;

        public void Run(List<Action> onUpdateCallbacks)
        {
            if (onUpdateCallbacks == null)
                throw new InvalidOperationException();
            
            if (_isActive)
                return;

            _isActive = true;
            _onUpdateCallbacks = onUpdateCallbacks;
        }

        public void Stop()
        {
            _isActive = false;
        }

        private void Update()
        {
            _onUpdateCallbacks.ForEach(callback => callback.Invoke());
        }
    }
}