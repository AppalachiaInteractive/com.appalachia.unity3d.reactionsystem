using System.Collections.Generic;
using Appalachia.Core.Behaviours;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ReactionSystem
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ReactionSystem : InternalMonoBehaviour
    {
        private const string _PRF_PFX = nameof(ReactionSystem) + ".";
        public List<ReactionSubsystemGroup> groups;

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                Initialize();
            }
        }

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));
        void Start()
        {
            using (_PRF_Start.Auto())
            {
                Initialize();
            }
        }

        private static readonly ProfilerMarker _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));
        private void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                Initialize();
            }
        }

        private static readonly ProfilerMarker _PRF_Initialize = new ProfilerMarker(_PRF_PFX + nameof(Initialize));
        private void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                if (groups == null)
                {
                    groups = new List<ReactionSubsystemGroup>();
                }

                for (int i = groups.Count - 1; i >= 0; i--)
                {
                    var group = groups[i];

                    if (group == null)
                    {
                        groups.RemoveAt(i);
                        continue;
                    }
                
                    group.Initialize(this, i);
                }
            }
        }
    }
}
