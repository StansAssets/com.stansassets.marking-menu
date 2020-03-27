namespace StansAssets.MarkingMenuB {
    interface IMarkingMenuInputController
    {
        void Init(IMarkingMenuInternal menu);
        void Reset();
        void HandleInput();
    }
}
