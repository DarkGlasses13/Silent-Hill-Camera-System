using System;
using UnityEngine;

namespace CameraSystem
{
    public class CameraTrigger : MonoBehaviour
    {
        public event Action<CameraTrigger> OnEnter;
        public event Action<CameraTrigger> OnExit;

        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == _target)
            {
                OnEnter?.Invoke(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == _target)
            {
                OnExit?.Invoke(this);
            }
        }
    }
}