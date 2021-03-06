using System;
using Appalachia.Editing.Attributes;
using Appalachia.ReactionSystem.Base;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.ReactionSystem.Cameras
{
    [Serializable]
    public abstract class ReactionSubsystemCamera : ReactionSubsystemBase
    {
        private const string _PRF_PFX = nameof(ReactionSubsystemCamera) + ".";

        private static readonly ProfilerMarker _PRF_EnsureSubsystemCenterIsPrepared =
            new(_PRF_PFX + nameof(EnsureSubsystemCenterIsPrepared));

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public Shader replacementShader;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public string replacementShaderTag;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public LayerMask cullingMask;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public Vector3 cameraOffset = new(0f, -1000f, 0f);

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public Vector3 cameraDirection = Vector3.up;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public Color backgroundColor;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public CameraClearFlags clearFlags;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [PropertyRange(1, 4096)]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        [ReadOnly]
        public int orthographicSize = 50;

        [SerializeField]
        [FoldoutGroup("Camera")]
        [SmartLabel]
        [OnValueChanged(nameof(Initialize))]
        public bool hideCamera = true;

        public abstract bool AutomaticRender { get; }

        // ReSharper disable once UnusedParameter.Global
        public abstract bool IsManualRenderingRequired(SubsystemCameraComponent cam);

        protected static void EnsureSubsystemCenterIsPrepared(
            SubsystemCameraComponent cam,
            ReactionSubsystemCamera subsystem)
        {
            using (_PRF_EnsureSubsystemCenterIsPrepared.Auto())
            {
                if (cam.center != null)
                {
                    cam.center.ValidateSubsystems();

                    if (!cam.center.subsystems.Contains(subsystem))
                    {
                        cam.center.subsystems.Add(subsystem);
                    }
                }
                else
                {
                    var centers = FindObjectsOfType<ReactionSubsystemCenter>();

                    for (var i = 0; i < centers.Length; i++)
                    {
                        var c = centers[i];

                        c.ValidateSubsystems();

                        if (c.subsystems.Contains(subsystem))
                        {
                            cam.center = c;
                        }
                    }
                }
            }
        }
    }
}
