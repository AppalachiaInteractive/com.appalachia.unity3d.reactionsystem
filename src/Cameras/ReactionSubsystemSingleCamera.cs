using System;
using Appalachia.Core.Editing.Attributes;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ReactionSystem.Cameras
{
    [Serializable]
    public abstract class ReactionSubsystemSingleCamera : ReactionSubsystemCamera
    {
        private const string _PRF_PFX = nameof(ReactionSubsystemSingleCamera) + ".";
        
        [FoldoutGroup("Camera")]
        [SmartLabel, OnValueChanged(nameof(Initialize))]
        public SubsystemCameraComponent cameraComponent;
        
        public override RenderTexture renderTexture => showRenderTexture ? cameraComponent.renderCamera.targetTexture : null;

        protected override bool showRenderTexture => cameraComponent.showRenderTexture;

        private static readonly ProfilerMarker _PRF_OnInitialization = new ProfilerMarker(_PRF_PFX + nameof(OnInitialization));
        protected override void OnInitialization()
        {
            using (_PRF_OnInitialization.Auto())
            {
                OnBeforeInitialization();

                EnsureSubsystemCenterIsPrepared(cameraComponent, this);
            
                if (cameraComponent.center == null)
                {
                    Debug.LogError("Must assign system center.");
                
                    return;
                }
            
                if (string.IsNullOrWhiteSpace(SubsystemName))
                {
                    Debug.LogError("Must define system name.");
                }
            
                gameObject.name = SubsystemName;
            
                if (!cameraComponent.renderCamera)
                {
                    cameraComponent.renderCamera = SubsystemCameraComponent.CreateCamera(this, SubsystemName);
                }

                if (cameraComponent.renderCamera)
                {
                    cameraComponent.renderCamera.enabled = true;
                }

                OnInitializationStart();

                SubsystemCameraComponent.UpdateCamera(this, cameraComponent.renderCamera);

                OnInitializationComplete();
            }
        }

        private static readonly ProfilerMarker _PRF_InitializeUpdateLoop = new ProfilerMarker(_PRF_PFX + nameof(InitializeUpdateLoop));
        protected override bool InitializeUpdateLoop()
        {
            using (_PRF_InitializeUpdateLoop.Auto())
            {
                var successful = false;
           
                cameraComponent.centerPresent = cameraComponent.center != null;
                cameraComponent.renderCameraPresent = cameraComponent.renderCamera != null;

                if (cameraComponent.centerPresent && cameraComponent.renderCameraPresent)
                {
                    successful = true;

                    cameraComponent.hasReplacementShader = replacementShader != null;
                }
            
                return successful;
            }
        }

        private static readonly ProfilerMarker _PRF_DoUpdateLoop = new ProfilerMarker(_PRF_PFX + nameof(DoUpdateLoop));
        protected override void DoUpdateLoop()
        {
            using (_PRF_DoUpdateLoop.Auto())
            {
                if (AutomaticRender)
                {
                    OnRenderStart();
                
                    cameraComponent.renderCamera.enabled = true;

                    if (cameraComponent.hasReplacementShader && !cameraComponent.shaderReplaced)
                    {
                        cameraComponent.renderCamera.SetReplacementShader(replacementShader, null);
                        cameraComponent.shaderReplaced = true;
                    }
                
                    OnRenderComplete();
                }
                else
                {
                    cameraComponent.renderCamera.enabled = false;
                
                    if (!IsManualRenderingRequired(cameraComponent))
                    {
                        return;
                    }
                
                    OnRenderStart();
                
                    if (cameraComponent.hasReplacementShader)
                    {
                        cameraComponent.renderCamera.RenderWithShader(replacementShader, replacementShaderTag);
                    }
                    else
                    {
                        cameraComponent.renderCamera.Render();
                    }
                
                    OnRenderComplete();
                }
            }
        }      
        
        protected abstract void OnBeforeInitialization();
        
        protected abstract void OnInitializationStart();
        
        protected abstract void OnInitializationComplete();
        
        protected abstract void OnRenderStart();
        
        protected abstract void OnRenderComplete();
        
        protected override void TeardownSubsystem()
        {
            if (cameraComponent.renderCamera)
            {
                cameraComponent.renderCamera.enabled = false;
                
                var rt = cameraComponent.renderCamera.targetTexture;
                cameraComponent.renderCamera.targetTexture = null;

                if (rt != null)
                {
                    DestroyImmediate(rt);
                }
            }
        }

    }
}