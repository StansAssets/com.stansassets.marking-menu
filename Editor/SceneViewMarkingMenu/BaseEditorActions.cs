using JetBrains.Annotations;
using UnityEngine;

namespace StansAssets.MarkingMenu
{
    [CustomActionRegistration(true)] [UsedImplicitly]
    public class BaseEditorActions
    {
        public static void Register(IMarkingMenu menu)
        {
            menu.Register("Action1", () => Debug.Log("Action1 executed!"));
            menu.Register("Action2", () => Debug.Log("Action2 executed!"));
            menu.Register("Action3", () => Debug.Log("Action3 executed!"));
            menu.Register("Action4", () => Debug.Log("Action4 executed!"));
        }
    }
}
