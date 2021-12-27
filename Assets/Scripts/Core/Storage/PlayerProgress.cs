using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Storage
{
    [Serializable]
    public struct PlayerProgress
    {
        [SerializeField]
        private List<OperationType> _sequence;

        public PlayerProgress(IEnumerable<OperationType> sequence)
        {
            _sequence = sequence.ToList();
        }
        public IEnumerable<OperationType> Sequence => _sequence;
    }
}