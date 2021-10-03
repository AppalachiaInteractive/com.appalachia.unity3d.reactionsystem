using System;
using System.Diagnostics.CodeAnalysis;
using Appalachia.Core.ReactionSystem.Base;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ReactionSystem.Cameras
{
    [Serializable]
    public struct SubsystemCameraShotSelector : IEquatable<SubsystemCameraShotSelector>
    {
        private const string _PRF_PFX = nameof(SubsystemCameraShotSelector) + ".";
        
        [SerializeField] private bool _initialized;
        public int currentIndex;

        private static readonly ProfilerMarker _PRF_InitiateCheck = new ProfilerMarker(_PRF_PFX + nameof(InitiateCheck));
        public void InitiateCheck(int totalCameras)
        {
            using (_PRF_InitiateCheck.Auto())
            {
                CheckIndex(totalCameras);
            }
        }
        
        private static readonly ProfilerMarker _PRF_ShouldRenderAtCenter = new ProfilerMarker(_PRF_PFX + nameof(ShouldRenderAtCenter));
        
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        public bool ShouldRenderAtCenter(SubsystemCameraShotSelectionMode selectionMode, ReactionSubsystemCenter center, int centerIndex, int totalCameras, int frameInterval)
        {
            using (_PRF_ShouldRenderAtCenter.Auto())
            {
                switch (selectionMode)
                {
                    case SubsystemCameraShotSelectionMode.RoundRobin:
                
                        return centerIndex == currentIndex;
                    
                    case SubsystemCameraShotSelectionMode.EveryXFrames:
                
                        if (frameInterval == 0) throw new NotSupportedException("Set frame interval!");
                        return (Time.frameCount + centerIndex) % frameInterval == 0;
                
                    default:
                        throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
                }
            }
        }

        private static readonly ProfilerMarker _PRF_ShouldRenderCamera = new ProfilerMarker(_PRF_PFX + nameof(ShouldRenderCamera));
        
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        public bool ShouldRenderCamera(SubsystemCameraShotSelectionMode selectionMode, SubsystemCameraComponent camera, int cameraIndex, int totalCameras, int frameInterval)
        {
            using (_PRF_ShouldRenderCamera.Auto())
            {
                switch (selectionMode)
                {
                    case SubsystemCameraShotSelectionMode.RoundRobin:
                
                        return cameraIndex == currentIndex;
                    
                    case SubsystemCameraShotSelectionMode.EveryXFrames:
                
                        if (frameInterval == 0) throw new NotSupportedException("Set frame interval!");
                        return (Time.frameCount + cameraIndex) % frameInterval == 0;
                
                    default:
                        throw new ArgumentOutOfRangeException(nameof(selectionMode), selectionMode, null);
                }
            }
        }

        private static readonly ProfilerMarker _PRF_CheckIndex = new ProfilerMarker(_PRF_PFX + nameof(CheckIndex));
        private void CheckIndex(int count)
        {
            using (_PRF_CheckIndex.Auto())
            {
                if (_initialized)
                {
                    currentIndex += 1;
                }
                else
                {
                    _initialized = true;
                }

                if (currentIndex >= count || currentIndex < 0)
                {
                    currentIndex = 0;
                }
            }
        }
        
#region IEquatable

        public bool Equals(SubsystemCameraShotSelector other)
        {
            return _initialized == other._initialized && currentIndex == other.currentIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is SubsystemCameraShotSelector other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_initialized.GetHashCode() * 397) ^ currentIndex;
            }
        }

        public static bool operator ==(SubsystemCameraShotSelector left, SubsystemCameraShotSelector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SubsystemCameraShotSelector left, SubsystemCameraShotSelector right)
        {
            return !left.Equals(right);
        }

#endregion
    }
}