namespace StansAssets.MarkingMenu
{
    /// <summary>
    /// Base interface for toggleable menu actions
    /// </summary>
    public interface IMarkingMenuToggle : IMarkingMenuButton
    {
        /// <summary>
        /// Current status (on/off) of toggle
        /// </summary>
        bool IsToggle { get; }
    }
}
