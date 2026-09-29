namespace NOModFramework
{
    public sealed class ModUiContext
    {
        public FrameworkTheme Theme { get; private set; }
        public UiWidgets Widgets { get; private set; }
        public float Alpha { get; private set; }

        public ModUiContext(FrameworkTheme theme, UiWidgets widgets, float alpha)
        {
            Theme = theme;
            Widgets = widgets;
            Alpha = alpha;
        }

        public void Update(FrameworkTheme theme, UiWidgets widgets, float alpha)
        {
            Theme = theme;
            Widgets = widgets;
            Alpha = alpha;
        }
    }
}