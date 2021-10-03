using System;
using Appalachia.Core.ReactionSystem.Base;
using Sirenix.OdinInspector;

namespace Appalachia.Core.ReactionSystem.Cameras
{
    [Serializable]
    public abstract class ReactionSubsystemSingleCameraMultipleLocationsShotSelector : ReactionSubsystemSingleCameraMultipleLocations
    {
        public abstract SubsystemCameraShotSelectionMode selectionMode { get; }
        private bool _selectionModeXFrames => selectionMode == SubsystemCameraShotSelectionMode.EveryXFrames;
        
        [PropertyRange(1, 300)]
        [ShowIf(nameof(_selectionModeXFrames))]
        public int frameShotInterval = 4;
        
        private SubsystemCameraShotSelector shotSelector;
        
        protected override ReactionSubsystemCenter GetCurrentSubsystemCenter()
        {
            if (shotSelector == default)
            {
                shotSelector = new SubsystemCameraShotSelector();
            }
            
            shotSelector.InitiateCheck(centers.Count);
            
            for(var i = 0; i < centers.Count; i++)
            {
                var shouldRender = shotSelector.ShouldRenderAtCenter(selectionMode, centers[i], i, centers.Count, frameShotInterval);

                if (shouldRender)
                {
                    return centers[i];
                }
            }
            
            return null;
        }
    }
}