using Cinemachine;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace CameraSystem
{
    public class CameraSystemController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _freeCamera;
        [SerializeField] private Camera[] _cameras;

        [field: SerializeField] public Transform Target { get; private set; }
        public Camera CurrentCamera { get; private set; }
        public bool IsAllTriggersNotActive => _cameras.All(camera => camera.Trigger.IsActive == false);

        private void OnEnable()
        {
            foreach (Camera camera in _cameras)
            {
                camera.Trigger.SetTarget(Target);
                camera.Trigger.OnEnter += OnEnter;
                camera.Trigger.OnExit += OnExit;
            }
        }

        private void OnEnter(CameraTrigger trigger)
        {
            Camera camera = _cameras.SingleOrDefault(camera => camera.Trigger == trigger);

            if (camera != null)
            {
                StopAllCoroutines();
                CurrentCamera = camera;
                camera.Instance.MoveToTopOfPrioritySubqueue();
            }
        }

        private void OnExit(CameraTrigger trigger)
        {
            Camera exitCamera = _cameras.SingleOrDefault(camera => camera.Trigger == trigger);
            Camera stayCamera = _cameras.SingleOrDefault(camera => camera.Trigger.IsActive);

            if (stayCamera != null)
            {
                OnEnter(stayCamera.Trigger);
                return;
            }

            if (CurrentCamera != exitCamera)
                return;

            if (exitCamera.SwitchToFreeCameraDelay < 0)
                return;

            StartCoroutine(SwitchToFreeCameraRoutine(exitCamera.SwitchToFreeCameraDelay));
        }

        private IEnumerator SwitchToFreeCameraRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (IsAllTriggersNotActive)
            {
                _freeCamera.MoveToTopOfPrioritySubqueue();
                CurrentCamera = null;
            }
        }

        private void OnDisable()
        {
            foreach (Camera camera in _cameras)
            {
                camera.Trigger.OnEnter -= OnEnter;
                camera.Trigger.OnExit -= OnExit;
            }
        }
    }
}