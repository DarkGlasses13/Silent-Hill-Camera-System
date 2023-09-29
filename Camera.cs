using Cinemachine;
using System;
using UnityEngine;

namespace CameraSystem
{
    [Serializable]
    public class Camera
    {
        [field: SerializeField] public CinemachineVirtualCamera Instance { get; private set; }
        [field: SerializeField] public CameraTrigger Trigger { get; private set; }
        [field: SerializeField] public float SwitchToFreeCameraDelay { get; private set; } = 1;
    }
}