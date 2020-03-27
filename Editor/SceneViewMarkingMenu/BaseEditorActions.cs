using UnityEngine;

namespace StansAssets.MarkingMenuB
{
    public class BaseEditorActions : IMarkingMenuActionRegistrationListener
    {
        public bool Enabled => true;

        // [ActionItemAttribute()]
        public void Register(IMarkingMenu menu)
        {
            menu.Register("Action1", () => Debug.Log("Action1 executed!"));
            menu.Register("Action2", () => Debug.Log("Action2 executed!"));
            menu.Register("Action3", () => Debug.Log("Action3 executed!"));
            menu.Register("Action4", () => Debug.Log("Action4 executed!"));
        }
    }
}
