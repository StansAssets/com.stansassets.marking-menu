namespace StansAssets.MarkingMenu {
    
    public class ToggleMenuContext
    {
        public delegate void SetDelegate(string state);
        public delegate ToggleMenuContextModel GetDelegate();

        public readonly SetDelegate Set;
        public readonly GetDelegate Get;

        public ToggleMenuContext(SetDelegate set, GetDelegate get)
        {
            Set = set;
            Get = get;
        }
    }
}