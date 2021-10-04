using Appalachia.Core.Globals.Shading;
using Appalachia.Core.ReactionSystem.Cameras;
using UnityEngine;

namespace Appalachia.Core.ReactionSystem.TouchBend
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class TouchBendCurrentStateMaskCamera : ReactionSubsystemSingleCameraSingleLocation
    {
        private const string _systemName = "TOUCH_BEND_CURRENT_STATE_MASK";

        protected override string SubsystemName => _systemName;

        public override bool AutomaticRender => true;
        public override bool IsManualRenderingRequired(SubsystemCameraComponent cam)
        {
            return false;
        }

        protected override void OnBeforeInitialization()
        {
        }

        protected override void OnInitializationStart()
        {
            if (cullingMask == 0)
            {
                cullingMask = LayerMask.GetMask(LayerMask.LayerToName(29));
            }
            
            cameraComponent.renderCamera.depth = -90;
        }

        protected override void OnInitializationComplete()
        {
        }

        protected override void OnRenderStart()
        {
        }
        
        protected override void OnRenderComplete()
        {
            Shader.SetGlobalTexture(GSC.TOUCHBEND._TOUCHBEND_CURRENT_STATE_MAP_MASK, renderTexture);
        }
    }
}
