using UnityEngine;
using UnityEditor;
using System;

namespace StansAssets.MarkingMenu
{
    /// <summary>
    /// Data for menu item.
    /// </summary>
    [CreateAssetMenu]
    internal class ActionsData : ScriptableObject
    {
        [Serializable]
        public struct ActionData
        {
            public string Label;

            /// <summary>
            /// Item angle relative to the menu center (0-360 deg.).
            /// </summary>
            [Range(0, 360)] public float Angle;

            /// <summary>
            /// Distance from menu item to menu
            /// </summary>
            public float Length;

            /// <summary>
            /// Class of action that is being performed on click. Should implement <see cref="IMarkingMenuButton"/>.
            /// </summary>
            public MonoScript Action;
        }

        [SerializeField] private ActionData[] m_Data;

        public ActionData[] Data => m_Data;
        public int Count => Data.Length;

        readonly Type k_ActionBaseType = typeof(IMarkingMenuButton);

        void OnValidate()
        {
            foreach (var data in Data)
            {
                if (!k_ActionBaseType.IsAssignableFrom(data.Action.GetClass()))
                {
                    Debug.LogError(data.Action?.GetClass()?.Name + " should implement 'IMarkingMenuAction' interface!");
                }
            }
        }
    }
}
