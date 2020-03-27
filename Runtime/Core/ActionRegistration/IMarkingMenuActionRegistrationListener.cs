using StansAssets.MarkingMenuB;

namespace StansAssets.MarkingMenuB
{
    public interface IMarkingMenuActionRegistrationListener
    {
        bool Enabled { get; }
        void Register(IMarkingMenu menu);
    }
}
