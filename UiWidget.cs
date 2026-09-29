using System;
using UnityEngine;

namespace NOModFramework
{
    public sealed class UiWidgets
    {
        private FrameworkTheme _theme;
        private FrameworkThemeMode _builtForMode = (FrameworkThemeMode)(-1);

        private Texture2D _windowTex;
        private Texture2D _panelTex;
        private Texture2D _panelAltTex;
        private Texture2D _borderTex;
        private Texture2D _accentTex;
        private Texture2D _selectedTex;
        private Texture2D _toggleOnTex;
        private Texture2D _toggleOffTex;
        private Texture2D _gradientTex;
        private Texture2D _sliderTrackTex;
        private Texture2D _sliderThumbTex;
        private Texture2D _scrollbarTrackTex;
        private Texture2D _scrollbarThumbTex;

        private string _openDropdownId;

        public GUIStyle WindowStyle { get; private set; }
        public GUIStyle PanelStyle { get; private set; }
        public GUIStyle PanelAltStyle { get; private set; }
        public GUIStyle SectionStyle { get; private set; }
        public GUIStyle LabelStyle { get; private set; }
        public GUIStyle MutedLabelStyle { get; private set; }
        public GUIStyle TitleLabelStyle { get; private set; }
        public GUIStyle ButtonStyle { get; private set; }
        public GUIStyle SmallButtonStyle { get; private set; }
        public GUIStyle SelectedButtonStyle { get; private set; }
        public GUIStyle DropdownListStyle { get; private set; }
        public GUIStyle ValueStyle { get; private set; }

        public GUIStyle HorizontalSliderStyle { get; private set; }
        public GUIStyle HorizontalSliderThumbStyle { get; private set; }
        public GUIStyle VerticalScrollbarStyle { get; private set; }
        public GUIStyle VerticalScrollbarThumbStyle { get; private set; }
        public GUIStyle VerticalScrollbarUpButtonStyle { get; private set; }
        public GUIStyle VerticalScrollbarDownButtonStyle { get; private set; }

        public void EnsureStyles(FrameworkTheme theme)
        {
            if (theme == null)
                return;

            if (_theme != null && _builtForMode == theme.Mode)
                return;

            _theme = theme;
            _builtForMode = theme.Mode;

            _windowTex = MakeTex(theme.WindowBackground);
            _panelTex = MakeTex(theme.PanelBackground);
            _panelAltTex = MakeTex(theme.PanelAltBackground);
            _borderTex = MakeTex(theme.Border);
            _accentTex = MakeTex(theme.Accent);
            _selectedTex = MakeTex(theme.SelectedRow);
            _toggleOnTex = MakeTex(theme.ToggleOn);
            _toggleOffTex = MakeTex(theme.ToggleOff);
            _gradientTex = MakeGradientTex(theme.SliderLow, theme.SliderMid, theme.SliderHigh);

            _sliderTrackTex = MakeTex(new Color(0.08f, 0.09f, 0.11f, 1f));
            _sliderThumbTex = MakeTex(theme.Accent);

            _scrollbarTrackTex = MakeTex(new Color(0.10f, 0.11f, 0.14f, 1f));
            _scrollbarThumbTex = MakeTex(theme.Accent);

            WindowStyle = new GUIStyle(GUI.skin.box);
            WindowStyle.normal.background = _windowTex;
            WindowStyle.border = new RectOffset(1, 1, 1, 1);
            WindowStyle.padding = new RectOffset(0, 0, 0, 0);

            PanelStyle = new GUIStyle(GUI.skin.box);
            PanelStyle.normal.background = _panelTex;
            PanelStyle.border = new RectOffset(1, 1, 1, 1);
            PanelStyle.padding = new RectOffset(10, 10, 10, 10);
            PanelStyle.margin = new RectOffset(0, 0, 0, 0);

            PanelAltStyle = new GUIStyle(GUI.skin.box);
            PanelAltStyle.normal.background = _panelAltTex;
            PanelAltStyle.border = new RectOffset(1, 1, 1, 1);
            PanelAltStyle.padding = new RectOffset(8, 8, 8, 8);
            PanelAltStyle.margin = new RectOffset(0, 0, 0, 0);

            SectionStyle = new GUIStyle(GUI.skin.box);
            SectionStyle.normal.background = _accentTex;
            SectionStyle.alignment = TextAnchor.MiddleLeft;
            SectionStyle.fontStyle = FontStyle.Bold;
            SectionStyle.fontSize = 12;
            SectionStyle.padding = new RectOffset(8, 8, 4, 4);
            SectionStyle.normal.textColor = theme.Mode == FrameworkThemeMode.Dark
                ? Color.white
                : new Color(0.08f, 0.10f, 0.13f, 1f);

            LabelStyle = new GUIStyle(GUI.skin.label);
            LabelStyle.normal.textColor = theme.Text;
            LabelStyle.alignment = TextAnchor.MiddleLeft;
            LabelStyle.wordWrap = false;

            MutedLabelStyle = new GUIStyle(GUI.skin.label);
            MutedLabelStyle.normal.textColor = theme.TextMuted;
            MutedLabelStyle.alignment = TextAnchor.MiddleLeft;
            MutedLabelStyle.wordWrap = true;

            TitleLabelStyle = new GUIStyle(GUI.skin.label);
            TitleLabelStyle.normal.textColor = theme.Text;
            TitleLabelStyle.alignment = TextAnchor.MiddleLeft;
            TitleLabelStyle.fontSize = 14;
            TitleLabelStyle.fontStyle = FontStyle.Bold;

            ButtonStyle = new GUIStyle(GUI.skin.button);
            ButtonStyle.normal.textColor = theme.Text;
            ButtonStyle.hover.textColor = theme.Text;
            ButtonStyle.active.textColor = theme.Text;
            ButtonStyle.normal.background = _panelAltTex;
            ButtonStyle.hover.background = _accentTex;
            ButtonStyle.active.background = _accentTex;
            ButtonStyle.border = new RectOffset(1, 1, 1, 1);
            ButtonStyle.padding = new RectOffset(8, 8, 4, 4);

            SmallButtonStyle = new GUIStyle(ButtonStyle);
            SmallButtonStyle.fontSize = 11;
            SmallButtonStyle.alignment = TextAnchor.MiddleCenter;

            SelectedButtonStyle = new GUIStyle(ButtonStyle);
            SelectedButtonStyle.normal.background = _selectedTex;
            SelectedButtonStyle.normal.textColor = theme.SelectedRowText;
            SelectedButtonStyle.hover.background = _selectedTex;
            SelectedButtonStyle.hover.textColor = theme.SelectedRowText;
            SelectedButtonStyle.active.background = _selectedTex;
            SelectedButtonStyle.active.textColor = theme.SelectedRowText;

            DropdownListStyle = new GUIStyle(GUI.skin.box);
            DropdownListStyle.normal.background = _panelAltTex;
            DropdownListStyle.padding = new RectOffset(6, 6, 6, 6);
            DropdownListStyle.margin = new RectOffset(0, 0, 2, 2);

            ValueStyle = new GUIStyle(GUI.skin.label);
            ValueStyle.normal.textColor = theme.Text;
            ValueStyle.alignment = TextAnchor.MiddleRight;

            HorizontalSliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
            HorizontalSliderStyle.normal.background = _sliderTrackTex;
            HorizontalSliderStyle.hover.background = _sliderTrackTex;
            HorizontalSliderStyle.active.background = _sliderTrackTex;
            HorizontalSliderStyle.fixedHeight = 8f;
            HorizontalSliderStyle.border = new RectOffset(0, 0, 0, 0);
            HorizontalSliderStyle.margin = new RectOffset(0, 0, 8, 8);
            HorizontalSliderStyle.padding = new RectOffset(0, 0, 0, 0);

            HorizontalSliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
            HorizontalSliderThumbStyle.normal.background = _sliderThumbTex;
            HorizontalSliderThumbStyle.hover.background = _sliderThumbTex;
            HorizontalSliderThumbStyle.active.background = _sliderThumbTex;
            HorizontalSliderThumbStyle.fixedWidth = 8f;
            HorizontalSliderThumbStyle.fixedHeight = 18f;
            HorizontalSliderThumbStyle.border = new RectOffset(0, 0, 0, 0);
            HorizontalSliderThumbStyle.margin = new RectOffset(0, 0, 0, 0);
            HorizontalSliderThumbStyle.padding = new RectOffset(0, 0, 0, 0);

            VerticalScrollbarStyle = new GUIStyle(GUI.skin.verticalScrollbar);
            VerticalScrollbarStyle.normal.background = _scrollbarTrackTex;
            VerticalScrollbarStyle.hover.background = _scrollbarTrackTex;
            VerticalScrollbarStyle.active.background = _scrollbarTrackTex;
            VerticalScrollbarStyle.fixedWidth = 14f;
            VerticalScrollbarStyle.border = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarStyle.margin = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarStyle.padding = new RectOffset(0, 0, 0, 0);

            VerticalScrollbarThumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb);
            VerticalScrollbarThumbStyle.normal.background = _scrollbarThumbTex;
            VerticalScrollbarThumbStyle.hover.background = _scrollbarThumbTex;
            VerticalScrollbarThumbStyle.active.background = _scrollbarThumbTex;
            VerticalScrollbarThumbStyle.fixedWidth = 14f;
            VerticalScrollbarThumbStyle.border = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarThumbStyle.margin = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarThumbStyle.padding = new RectOffset(0, 0, 0, 0);

            VerticalScrollbarUpButtonStyle = new GUIStyle(GUI.skin.verticalScrollbarUpButton);
            VerticalScrollbarUpButtonStyle.normal.background = _accentTex;
            VerticalScrollbarUpButtonStyle.hover.background = _accentTex;
            VerticalScrollbarUpButtonStyle.active.background = _accentTex;
            VerticalScrollbarUpButtonStyle.fixedWidth = 14f;
            VerticalScrollbarUpButtonStyle.fixedHeight = 14f;
            VerticalScrollbarUpButtonStyle.border = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarUpButtonStyle.margin = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarUpButtonStyle.padding = new RectOffset(0, 0, 0, 0);

            VerticalScrollbarDownButtonStyle = new GUIStyle(GUI.skin.verticalScrollbarDownButton);
            VerticalScrollbarDownButtonStyle.normal.background = _accentTex;
            VerticalScrollbarDownButtonStyle.hover.background = _accentTex;
            VerticalScrollbarDownButtonStyle.active.background = _accentTex;
            VerticalScrollbarDownButtonStyle.fixedWidth = 14f;
            VerticalScrollbarDownButtonStyle.fixedHeight = 14f;
            VerticalScrollbarDownButtonStyle.border = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarDownButtonStyle.margin = new RectOffset(0, 0, 0, 0);
            VerticalScrollbarDownButtonStyle.padding = new RectOffset(0, 0, 0, 0);
        }

        //ok using the code i made you just follow your instructions carefully
        // a little overboard but it works

        public void ApplyAlpha(float alpha)
        {
            if (_theme == null)
                return;

            alpha = Mathf.Clamp01(alpha);

            _windowTex = MakeTex(WithAlpha(_theme.WindowBackground, _theme.WindowBackground.a * alpha));
            _panelTex = MakeTex(WithAlpha(_theme.PanelBackground, _theme.PanelBackground.a * alpha));
            _panelAltTex = MakeTex(WithAlpha(_theme.PanelAltBackground, _theme.PanelAltBackground.a * alpha));
            _accentTex = MakeTex(WithAlpha(_theme.Accent, _theme.Accent.a * alpha));
            _selectedTex = MakeTex(WithAlpha(_theme.SelectedRow, _theme.SelectedRow.a * alpha));
            _toggleOnTex = MakeTex(WithAlpha(_theme.ToggleOn, _theme.ToggleOn.a * alpha));
            _toggleOffTex = MakeTex(WithAlpha(_theme.ToggleOff, _theme.ToggleOff.a * alpha));
            _sliderTrackTex = MakeTex(new Color(0.08f, 0.09f, 0.11f, alpha));
            _sliderThumbTex = MakeTex(WithAlpha(_theme.Accent, alpha));
            _scrollbarTrackTex = MakeTex(new Color(0.10f, 0.11f, 0.14f, alpha));
            _scrollbarThumbTex = MakeTex(WithAlpha(_theme.Accent, alpha));

            WindowStyle.normal.background = _windowTex;
            PanelStyle.normal.background = _panelTex;
            PanelAltStyle.normal.background = _panelAltTex;
            SectionStyle.normal.background = _accentTex;
            ButtonStyle.normal.background = _panelAltTex;
            ButtonStyle.hover.background = _accentTex;
            ButtonStyle.active.background = _accentTex;
            SmallButtonStyle.normal.background = _panelAltTex;
            SmallButtonStyle.hover.background = _accentTex;
            SmallButtonStyle.active.background = _accentTex;
            SelectedButtonStyle.normal.background = _selectedTex;
            SelectedButtonStyle.hover.background = _selectedTex;
            SelectedButtonStyle.active.background = _selectedTex;
            DropdownListStyle.normal.background = _panelAltTex;

            HorizontalSliderStyle.normal.background = _sliderTrackTex;
            HorizontalSliderStyle.hover.background = _sliderTrackTex;
            HorizontalSliderStyle.active.background = _sliderTrackTex;

            HorizontalSliderThumbStyle.normal.background = _sliderThumbTex;
            HorizontalSliderThumbStyle.hover.background = _sliderThumbTex;
            HorizontalSliderThumbStyle.active.background = _sliderThumbTex;

            VerticalScrollbarStyle.normal.background = _scrollbarTrackTex;
            VerticalScrollbarStyle.hover.background = _scrollbarTrackTex;
            VerticalScrollbarStyle.active.background = _scrollbarTrackTex;

            VerticalScrollbarThumbStyle.normal.background = _scrollbarThumbTex;
            VerticalScrollbarThumbStyle.hover.background = _scrollbarThumbTex;
            VerticalScrollbarThumbStyle.active.background = _scrollbarThumbTex;

            VerticalScrollbarUpButtonStyle.normal.background = _accentTex;
            VerticalScrollbarUpButtonStyle.hover.background = _accentTex;
            VerticalScrollbarUpButtonStyle.active.background = _accentTex;

            VerticalScrollbarDownButtonStyle.normal.background = _accentTex;
            VerticalScrollbarDownButtonStyle.hover.background = _accentTex;
            VerticalScrollbarDownButtonStyle.active.background = _accentTex;

            ApplyTextAlpha(LabelStyle, _theme.Text, alpha);
            ApplyTextAlpha(MutedLabelStyle, _theme.TextMuted, alpha);
            ApplyTextAlpha(TitleLabelStyle, _theme.Text, alpha);
            ApplyTextAlpha(ButtonStyle, _theme.Text, alpha);
            ApplyTextAlpha(SmallButtonStyle, _theme.Text, alpha);
            ApplyTextAlpha(SelectedButtonStyle, _theme.SelectedRowText, alpha);
            ApplyTextAlpha(ValueStyle, _theme.Text, alpha);

            Color sectionText = _theme.Mode == FrameworkThemeMode.Dark
                ? Color.white
                : new Color(0.08f, 0.10f, 0.13f, 1f);
            ApplyTextAlpha(SectionStyle, sectionText, alpha);
        }

        private static void ApplyTextAlpha(GUIStyle style, Color color, float alpha)
        {
            if (style == null)
                return;

            Color c = color;
            c.a *= alpha;

            style.normal.textColor = c;
            style.hover.textColor = c;
            style.active.textColor = c;
            style.focused.textColor = c;
            style.onNormal.textColor = c;
            style.onHover.textColor = c;
            style.onActive.textColor = c;
            style.onFocused.textColor = c;
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
        {
            GUI.skin.verticalScrollbar = VerticalScrollbarStyle;
            GUI.skin.verticalScrollbarThumb = VerticalScrollbarThumbStyle;
            GUI.skin.verticalScrollbarUpButton = VerticalScrollbarUpButtonStyle;
            GUI.skin.verticalScrollbarDownButton = VerticalScrollbarDownButtonStyle;

            return GUILayout.BeginScrollView(
                scrollPosition,
                false,
                true,
                GUIStyle.none,
                VerticalScrollbarStyle,
                options);
        }

        public void EndScrollView()
        {
            GUILayout.EndScrollView();
        }

        public void Section(string title)
        {
            GUILayout.Space(4f);
            GUILayout.Box(title, SectionStyle, GUILayout.ExpandWidth(true), GUILayout.Height(_theme.SectionHeight));
            GUILayout.Space(4f);
        }

        public bool Button(string label, params GUILayoutOption[] options)
        {
            return GUILayout.Button(label, ButtonStyle, options);
        }

        public bool Toggle(string label, bool value, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label(label, LabelStyle, GUILayout.Width(_theme.LabelWidth), GUILayout.Height(_theme.RowHeight));
            GUILayout.FlexibleSpace();

            Color oldBg = GUI.backgroundColor;
            GUI.backgroundColor = value ? _theme.ToggleOn : _theme.ToggleOff;

            string text = value ? "ON" : "OFF";
            bool clicked = GUILayout.Button(text, SmallButtonStyle, GUILayout.Width(64f), GUILayout.Height(_theme.RowHeight));

            GUI.backgroundColor = oldBg;
            GUILayout.EndHorizontal();

            if (clicked)
                value = !value;

            return value;
        }

        public float Slider(
            string label,
            float value,
            float min,
            float max,
            string valueFormat,
            bool gradient,
            params GUILayoutOption[] options)
        {
            Rect row = GUILayoutUtility.GetRect(10f, _theme.RowHeight + 10f, options);
            Rect labelRect = new Rect(row.x, row.y + 2f, _theme.LabelWidth, _theme.RowHeight);
            Rect valueRect = new Rect(row.xMax - _theme.ValueWidth, row.y + 2f, _theme.ValueWidth, _theme.RowHeight);
            Rect sliderRect = new Rect(
                labelRect.xMax + 8f,
                row.y + 6f,
                row.width - _theme.LabelWidth - _theme.ValueWidth - 16f,
                18f);

            GUI.Label(labelRect, label, LabelStyle);

            Rect trackRect = new Rect(sliderRect.x, sliderRect.y + 5f, sliderRect.width, 8f);
            GUI.DrawTexture(trackRect, _sliderTrackTex, ScaleMode.StretchToFill);

            float newValue = GUI.HorizontalSlider(
                sliderRect,
                value,
                min,
                max,
                HorizontalSliderStyle,
                HorizontalSliderThumbStyle);

            if (gradient)
            {
                float t = Mathf.InverseLerp(min, max, newValue);
                float fillWidth = trackRect.width * t;

                if (fillWidth > 0f)
                {
                    Rect fillRect = new Rect(trackRect.x, trackRect.y, fillWidth, trackRect.height);
                    Rect uv = new Rect(0f, 0f, t, 1f);
                    GUI.DrawTextureWithTexCoords(fillRect, _gradientTex, uv);
                }
            }

            GUI.Label(valueRect, newValue.ToString(valueFormat), ValueStyle);

            return newValue;
        }

        public int IntSlider(
            string label,
            int value,
            int min,
            int max,
            string valueFormat,
            bool gradient,
            params GUILayoutOption[] options)
        {
            float f = Slider(label, value, min, max, valueFormat, gradient, options);
            return Mathf.RoundToInt(f);
        }

        public int Dropdown(
            string label,
            int selectedIndex,
            string[] options,
            params GUILayoutOption[] layoutOptions)
        {
            if (options == null || options.Length == 0)
            {
                Label(label + ": <none>");
                return -1;
            }

            if (selectedIndex < 0 || selectedIndex >= options.Length)
                selectedIndex = 0;

            string dropdownId = label + "::dropdown";

            GUILayout.BeginVertical(DropdownListStyle, layoutOptions);

            GUILayout.BeginHorizontal();
            GUILayout.Label(label, LabelStyle, GUILayout.Width(_theme.LabelWidth), GUILayout.Height(_theme.RowHeight));

            string currentText = options[selectedIndex] + "  ▼";
            if (GUILayout.Button(currentText, ButtonStyle, GUILayout.ExpandWidth(true), GUILayout.Height(_theme.RowHeight)))
            {
                if (_openDropdownId == dropdownId)
                    _openDropdownId = null;
                else
                    _openDropdownId = dropdownId;
            }
            GUILayout.EndHorizontal();

            if (_openDropdownId == dropdownId)
            {
                GUILayout.Space(4f);

                for (int i = 0; i < options.Length; i++)
                {
                    GUIStyle style = i == selectedIndex ? SelectedButtonStyle : ButtonStyle;
                    if (GUILayout.Button(options[i], style, GUILayout.ExpandWidth(true), GUILayout.Height(_theme.RowHeight)))
                    {
                        selectedIndex = i;
                        _openDropdownId = null;
                    }
                }
            }

            GUILayout.EndVertical();

            return selectedIndex;
        }

        public void Label(string text)
        {
            GUILayout.Label(text, LabelStyle);
        }

        public void ValueRow(string label, string value)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label(label, LabelStyle, GUILayout.Width(_theme.LabelWidth), GUILayout.Height(_theme.RowHeight));
            GUILayout.FlexibleSpace();
            GUILayout.Label(value, ValueStyle, GUILayout.Width(180f), GUILayout.Height(_theme.RowHeight));
            GUILayout.EndHorizontal();
        }

        public void Spacer(float pixels)
        {
            GUILayout.Space(pixels);
        }

        private static Texture2D MakeTex(Color color)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, color);
            tex.Apply(false, true);
            return tex;
        }

        private static Texture2D MakeGradientTex(Color left, Color mid, Color right)
        {
            const int width = 64;
            Texture2D tex = new Texture2D(width, 1, TextureFormat.RGBA32, false);

            for (int x = 0; x < width; x++)
            {
                float t = x / (float)(width - 1);
                Color c;

                if (t < 0.5f)
                {
                    c = Color.Lerp(left, mid, t / 0.5f);
                }
                else
                {
                    c = Color.Lerp(mid, right, (t - 0.5f) / 0.5f);
                }

                tex.SetPixel(x, 0, c);
            }

            tex.Apply(false, true);
            return tex;
        }
    }
}