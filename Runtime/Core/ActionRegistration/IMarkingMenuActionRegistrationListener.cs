using System;
using StansAssets.MarkingMenu;

namespace StansAssets.MarkingMenu
{
    public class CustomActionRegistrationAttribute : Attribute
    {
        public bool Enabled { get; }

        public CustomActionRegistrationAttribute(bool enabled)
        {
            Enabled = enabled;
        }
    }

    public interface IMarkingMenuActionRegistrationListener
    {
        bool Enabled { get; }
        void Register(IMarkingMenu menu);
    }
}
