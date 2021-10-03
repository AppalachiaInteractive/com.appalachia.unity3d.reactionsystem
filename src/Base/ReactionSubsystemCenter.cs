using System.Collections.Generic;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Editing;
using Appalachia.Core.Editing.Handle;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.ReactionSystem.Base
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ReactionSubsystemCenter : InternalMonoBehaviour
    {
        private const string _PRF_PFX = nameof(ReactionSubsystemCenter) + ".";
        
        public Vector3 offset;

        public Color gizmoColor = Color.cyan;
        
        [FormerlySerializedAs("systems")] 
        public List<ReactionSubsystemBase> subsystems = new List<ReactionSubsystemBase>();
        
#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_OnDrawGizmosSelected = new ProfilerMarker(_PRF_PFX + nameof(OnDrawGizmosSelected));
        private void OnDrawGizmosSelected()
        {
            using (_PRF_OnDrawGizmosSelected.Auto())
            {
                if (!GizmoCameraChecker.ShouldRenderGizmos())
                {
                    return;
                }
                
                SmartHandles.DrawWireSphere(GetPosition(), 2f, gizmoColor);
            }
        }
#endif
        private static readonly ProfilerMarker _PRF_GetPosition = new ProfilerMarker(_PRF_PFX + nameof(GetPosition));
        public Vector3 GetPosition()
        {
            using (_PRF_GetPosition.Auto())
            {
                return _transform.position + offset;
            }
        }

        private static readonly ProfilerMarker _PRF_ValidateSubsystems = new ProfilerMarker(_PRF_PFX + nameof(ValidateSubsystems));
        public void ValidateSubsystems()
        {
            using (_PRF_ValidateSubsystems.Auto())
            {
                if (subsystems == null)
                {
                    subsystems = new List<ReactionSubsystemBase>();
                }
            
                for (var i = subsystems.Count - 1; i >= 0; i--)
                {
                    var subsystem = subsystems[i];

                    if (subsystem == null)
                    {
                        subsystems.RemoveAt(i);
                    }
                }
            }
        }
    }
}
