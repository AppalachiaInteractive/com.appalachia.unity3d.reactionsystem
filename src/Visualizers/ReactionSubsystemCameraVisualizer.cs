#if UNITY_EDITOR
using Appalachia.Core.Extensions;
using Appalachia.Editing.Visualizers.Base;
using Appalachia.ReactionSystem.Cameras;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Appalachia.ReactionSystem.Visualizers
{
    public class ReactionSubsystemCameraVisualizer : InstancedIndirectSpatialMapVisualization
    {
        [PropertyOrder(-200)] public ReactionSubsystemCamera subsystem;

        protected override bool ShouldRegenerate =>
            (subsystem != null) && subsystem.AutomaticRender;

        protected override bool CanVisualize =>
            (subsystem != null) && (subsystem.renderTexture != null);

        protected override bool CanGenerate => subsystem != null;

        protected override void PrepareInitialGeneration()
        {
            texture = subsystem.renderTexture.Capture();
        }

        protected override void PrepareSubsequentGenerations()
        {
            texture = subsystem.renderTexture.Capture();
        }

        protected override void GetVisualizationInfo(
            Vector3 position,
            float4 color,
            out float height,
            out Quaternion rotation,
            out Vector3 scale)
        {
            height = position.y;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }
    }
}

#endif
