using System;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.ReactionSystem.Base
{
    [ExecuteAlways]
    [Serializable]
    public abstract class ReactionSubsystemBase : InternalMonoBehaviour
    {
        private const string _PRF_PFX = nameof(ReactionSubsystemBase) + ".";
        
        protected abstract string SubsystemName { get; }

        public ReactionSystem mainSystem;

        [SerializeField] private int _groupIndex;

        protected ReactionSubsystemGroup Group => (mainSystem) ? mainSystem.groups[_groupIndex] : null;

        [SerializeField]
        [FoldoutGroup("Texture")]
        [OnValueChanged(nameof(Initialize))]
        public RenderTextureFormat renderTextureFormat;

        [SerializeField]
        [FoldoutGroup("Texture")]
        [OnValueChanged(nameof(Initialize))]
        public RenderQuality renderTextureQuality = RenderQuality.High_1024;

        [SerializeField]
        [FoldoutGroup("Texture")]
        [OnValueChanged(nameof(Initialize))]
        public FilterMode filterMode;

        [SerializeField]
        [FoldoutGroup("Texture")]
        [OnValueChanged(nameof(Initialize))]
        [ValueDropdown(nameof(depths))]
        public int depth;

        private ValueDropdownList<int> depths = new ValueDropdownList<int>()
        {
            {0},
            {8},
            {16},
            {24},
            {32}
        };

        [InlineProperty, ShowInInspector]
        [PreviewField(ObjectFieldAlignment.Center, Height = 256)]
        [FoldoutGroup("Preview")]
        [ShowIf(nameof(showRenderTexture))]
        public abstract RenderTexture renderTexture { get; }

        protected abstract bool showRenderTexture { get; }

        //public abstract void GetRenderingPosition(out Vector3 minimumPosition, out Vector3 size);

        private void Awake()
        {
            updateLoopInitialized = false;
            Initialize();
        }

        private void OnEnable()
        {
            updateLoopInitialized = false;
            Initialize();
        }

        protected abstract void TeardownSubsystem();

        private void OnDisable()
        {
            TeardownSubsystem();
            updateLoopInitialized = false;
        }

        protected abstract void OnInitialization();

        public void InitializeSubsystem(ReactionSystem system, int groupIndex)
        {
            mainSystem = system;
            _groupIndex = groupIndex;

            Initialize();
        }

        [Button]
        public void Initialize()
        {
            gameObject.name = SubsystemName;

            OnInitialization();
        }
        public void UpdateGroupIndex(int i)
        {
            _groupIndex = i;
        }
        
        private bool updateLoopInitialized;

        protected abstract bool InitializeUpdateLoop();

        protected abstract void DoUpdateLoop();

        private void Update()
        {
            try
            {
                if (!updateLoopInitialized)
                {
                    updateLoopInitialized = InitializeUpdateLoop();
                }

                if (!updateLoopInitialized)
                {
                    return;
                }

                DoUpdateLoop();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

    }
}