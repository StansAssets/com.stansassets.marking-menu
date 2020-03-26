using System;

namespace StansAssets.MarkingMenuB {
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ActionItemAttribute : Attribute
    {
        public string ActionId { get; private set; }
        public ActionItemAttribute(string actionId)
        {
            ActionId = actionId;
        }
    }
}
