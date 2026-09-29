using UnityEngine;

namespace NOModFramework
{
    public enum FrameworkThemeMode
    {
        Dark = 0,
        Light = 1
    }

    public sealed class FrameworkTheme
    {
        public FrameworkThemeMode Mode;

        public Color Overlay;
        public Color WindowBackground;
        public Color PanelBackground;
        public Color PanelAltBackground;
        public Color Border;
        public Color Text;
        public Color TextMuted;
        public Color Accent;
        public Color AccentDim;
        public Color SelectedRow;
        public Color SelectedRowText;
        public Color ToggleOn;
        public Color ToggleOff;
        public Color SliderLow;
        public Color SliderMid;
        public Color SliderHigh;

        public float WindowPadding = 10f;
        public float TitleBarHeight = 36f;
        public float LeftPanelWidth = 240f;
        public float RowHeight = 26f;
        public float SectionHeight = 28f;
        public float LabelWidth = 150f;
        public float ValueWidth = 64f;

        public static FrameworkTheme Create(FrameworkThemeMode mode)
        {
            if (mode == FrameworkThemeMode.Light)
                return CreateLight();

            return CreateDark();
        }

        // honestly ugg just leave the theme and i'll find a good unity video

        private static FrameworkTheme CreateDark()
        {
            FrameworkTheme theme = new FrameworkTheme();
            theme.Mode = FrameworkThemeMode.Dark;

            theme.Overlay = new Color(0f, 0f, 0f, 0.45f);
            theme.WindowBackground = new Color(0.10f, 0.11f, 0.13f, 0.98f);
            theme.PanelBackground = new Color(0.15f, 0.16f, 0.19f, 0.98f);
            theme.PanelAltBackground = new Color(0.12f, 0.13f, 0.16f, 0.98f);
            theme.Border = new Color(0.25f, 0.29f, 0.34f, 1f);
            theme.Text = new Color(0.92f, 0.94f, 0.97f, 1f);
            theme.TextMuted = new Color(0.68f, 0.72f, 0.78f, 1f);
            theme.Accent = new Color(0.27f, 0.55f, 0.92f, 1f);
            theme.AccentDim = new Color(0.19f, 0.34f, 0.57f, 1f);
            theme.SelectedRow = new Color(0.21f, 0.32f, 0.52f, 1f);
            theme.SelectedRowText = new Color(1f, 1f, 1f, 1f);
            theme.ToggleOn = new Color(0.19f, 0.62f, 0.32f, 1f);
            theme.ToggleOff = new Color(0.45f, 0.18f, 0.18f, 1f);
            theme.SliderLow = new Color(0.18f, 0.76f, 0.28f, 1f);
            theme.SliderMid = new Color(0.96f, 0.83f, 0.18f, 1f);
            theme.SliderHigh = new Color(0.86f, 0.18f, 0.18f, 1f);

            return theme;
        }

        private static FrameworkTheme CreateLight()
        {
            FrameworkTheme theme = new FrameworkTheme();
            theme.Mode = FrameworkThemeMode.Light;

            theme.Overlay = new Color(0f, 0f, 0f, 0.20f);
            theme.WindowBackground = new Color(0.89f, 0.91f, 0.94f, 0.98f);
            theme.PanelBackground = new Color(0.97f, 0.98f, 0.99f, 0.98f);
            theme.PanelAltBackground = new Color(0.93f, 0.95f, 0.97f, 0.98f);
            theme.Border = new Color(0.67f, 0.71f, 0.76f, 1f);
            theme.Text = new Color(0.12f, 0.14f, 0.17f, 1f);
            theme.TextMuted = new Color(0.35f, 0.39f, 0.45f, 1f);
            theme.Accent = new Color(0.22f, 0.49f, 0.85f, 1f);
            theme.AccentDim = new Color(0.74f, 0.84f, 0.96f, 1f);
            theme.SelectedRow = new Color(0.73f, 0.84f, 0.97f, 1f);
            theme.SelectedRowText = new Color(0.08f, 0.10f, 0.13f, 1f);
            theme.ToggleOn = new Color(0.19f, 0.62f, 0.32f, 1f);
            theme.ToggleOff = new Color(0.74f, 0.30f, 0.30f, 1f);
            theme.SliderLow = new Color(0.18f, 0.76f, 0.28f, 1f);
            theme.SliderMid = new Color(0.96f, 0.83f, 0.18f, 1f);
            theme.SliderHigh = new Color(0.86f, 0.18f, 0.18f, 1f);

            return theme;
        }
    }
}