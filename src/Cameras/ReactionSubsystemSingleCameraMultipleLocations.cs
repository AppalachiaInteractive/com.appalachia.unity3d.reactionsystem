using System;
using System.Collections.Generic;
using Appalachia.ReactionSystem.Base;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.ReactionSystem.Cameras
{
    [Serializable]
    public abstract class ReactionSubsystemSingleCameraMultipleLocations : ReactionSubsystemSingleCamera
    {
        private const string _PRF_PFX = nameof(ReactionSubsystemSingleCameraMultipleLocations) + ".";
        
        [OnValueChanged(nameof(Initialize))]
        public List<ReactionSubsystemCenter> centers = new List<ReactionSubsystemCenter>();

        private static readonly ProfilerMarker _PRF_OnRenderStart = new ProfilerMarker(_PRF_PFX + nameof(OnRenderStart));
        protected override void OnRenderStart()
        {
            using (_PRF_OnRenderStart.Auto())
            {
                var currentCenter = GetCurrentSubsystemCenter();
                cameraComponent.center = currentCenter;
            
                var tempCameraPosition = cameraComponent.GetCameraRootPosition();
                var pos = tempCameraPosition;
                pos += cameraOffset;

                cameraComponent.renderCamera.transform.position = pos;
                cameraComponent.renderCamera.transform.rotation = Quaternion.LookRotation(cameraDirection, Vector3.forward);
            }
        }

        protected abstract ReactionSubsystemCenter GetCurrentSubsystemCenter();
    }
}