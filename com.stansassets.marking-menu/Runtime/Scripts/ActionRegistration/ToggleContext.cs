namespace StansAssets.MarkingMenu {
    public class ToggleContext
    {
        public delegate void SetDelegate(bool state);
        public delegate bool GetDelegate();

        public readonly SetDelegate Set;
        public readonly GetDelegate Get;

        public ToggleContext(SetDelegate set, GetDelegate get)
        {
            Set = set;
            Get = get;
        }
    }
}
