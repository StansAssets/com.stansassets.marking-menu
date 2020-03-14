using UnityEngine;

namespace StansAssets.MarkingMenu
{
    [CreateAssetMenu]
    class ActionsDataHolder : ScriptableObject
    {
        static ActionsDataHolder s_Instance;

        public static ActionsDataHolder Instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = Resources.Load<ActionsDataHolder>("Data/Data Holder");
                return s_Instance;
            }
        }

        [SerializeField]
        ActionsData m_ActionsData;

        public ActionsData ActionsData => m_ActionsData;
    }
}
