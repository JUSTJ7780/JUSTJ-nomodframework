using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NOModFramework
{
    [BepInPlugin("com.JUSTJ7780.nomodframework", "NO Mod Framework", "1.1.0")]
    public class FrameworkPlugin : BaseUnityPlugin
    {
        internal static FrameworkPlugin Instance { get; private set; }
        internal static ManualLogSource FrameworkLogger;

        private ConfigEntry<int> _themeMode;
        private ConfigEntry<bool> _fadeEnabled;
        private ConfigEntry<float> _windowX;
        private ConfigEntry<float> _windowY;
        private ConfigEntry<float> _windowW;
        private ConfigEntry<float> _windowH;
        private ConfigEntry<string> _lastSelectedModId;

        private readonly FadeController _fade = new FadeController();
        private readonly UiWidgets _widgets = new UiWidgets();

        private FrameworkTheme _theme;
        private FrameworkThemeMode _cachedThemeMode = (FrameworkThemeMode)(-1);
        private ModUiContext _uiContext;

        private Rect _windowRect;
        private bool _menuOpen;

        private Vector2 _modListScroll;
        private Vector2 _contentScroll;

        private IModMenu _selectedMenu;
        private int _seenRegistryVersion = -1;
        private Harmony _harmony;

        private void Awake()
        {
            Instance = this;
            FrameworkLogger = Logger;

            _themeMode = Config.Bind("Menu", "Theme Mode", 0, "0 = Dark, 1 = Light");
            _fadeEnabled = Config.Bind("Menu", "Fade Enabled", true, "Enable simple fade in/out.");
            _windowX = Config.Bind("Menu", "Window X", 140f, "");
            _windowY = Config.Bind("Menu", "Window Y", 90f, "");
            _windowW = Config.Bind("Menu", "Window Width", 980f, "");
            _windowH = Config.Bind("Menu", "Window Height", 620f, "");
            _lastSelectedModId = Config.Bind("Menu", "Last Selected Mod Id", string.Empty, "");

            _windowRect = new Rect(_windowX.Value, _windowY.Value, _windowW.Value, _windowH.Value);

            RebuildTheme();
            _uiContext = new ModUiContext(_theme, _widgets, 0f);

            _fade.SetOpen(false, true);
            _harmony = new Harmony("com.JUSTJ7780.nomodframework.native-menu-hooks");
            _harmony.PatchAll(typeof(FrameworkPlugin).Assembly);
            Logger.LogInfo("NO Mod Framework loaded");
        }

        private void Update()
        {
            _fade.Tick(Time.unscaledDeltaTime);

            if (_menuOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                SetMenuOpen(false);
            }

            if (_menuOpen)
            {
                RefreshRegistryState();
            }
        }

        private void OnDestroy()
        {
            SaveWindowRect();
            if (_harmony != null)
            {
                _harmony.UnpatchSelf();
            }
            if (Instance == this)
            {
                Instance = null;
            }
        }
        internal static bool OpenFrameworkMenu()
        {
            if (Instance == null)
            {
                return false;
            }
            Instance.SetMenuOpen(true);
            return true;
        }
        internal static bool CloseFrameworkMenu()
        {
            if (Instance == null)
            {
                return false;
            }
            Instance.SetMenuOpen(false);
            return true;
        }
        internal static bool IsFrameworkMenuOpen
        {
            get { return Instance != null && Instance._menuOpen; }
        }
        internal void QueueMainMenuButton(MainMenu mainMenu)
        {
            StartCoroutine(InjectMainMenuButtonWhenReady(mainMenu));
        }
        internal void AddPauseMenuButton(GameplayUI gameplayUi)
        {
            NativeMenuButtons.TryInjectPauseMenuButton(gameplayUi);
        }
        private IEnumerator InjectMainMenuButtonWhenReady(MainMenu mainMenu)
        {
            const int maxFrames = 600;
            for (int frame = 0; frame < maxFrames; frame++)
            {
                if (mainMenu == null)
                {
                    yield break;
                }
                if (MainMenu.State == MainMenu.LoadingState.Loaded &&
                    NativeMenuButtons.TryInjectMainMenuButton(mainMenu))
                {
                    yield break;
                }
                yield return null;
            }
            Logger.LogWarning("Could not add the Mods button to the main menu.");
        }

        private void OnGUI()
        {
            if (_fade.Alpha <= 0.001f)
                return;

            if (_cachedThemeMode != ReadThemeMode())
            {
                RebuildTheme();
            }

            _widgets.EnsureStyles(_theme);
            _widgets.ApplyAlpha(_fade.Alpha);
            _uiContext.Update(_theme, _widgets, _fade.Alpha);

            _windowRect = ClampWindowRect(_windowRect);

            Color oldColor = GUI.color;
            Color oldBg = GUI.backgroundColor;
            Color oldContent = GUI.contentColor;

            GUI.color = new Color(1f, 1f, 1f, _fade.Alpha);
            GUI.backgroundColor = new Color(1f, 1f, 1f, _fade.Alpha);
            GUI.contentColor = new Color(1f, 1f, 1f, _fade.Alpha);

            DrawOverlay();
            _windowRect = GUI.Window(187211, _windowRect, DrawWindow, string.Empty, _widgets.WindowStyle);

            GUI.backgroundColor = oldBg;
            GUI.contentColor = oldContent;
            GUI.color = oldColor;

            SaveWindowRect();
        }

        private void DrawOverlay()
        {
            Color old = GUI.color;
            GUI.color = new Color(_theme.Overlay.r, _theme.Overlay.g, _theme.Overlay.b, _theme.Overlay.a * _fade.Alpha);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.color = old;
        }


        private void DrawWindow(int windowId)
        {
            float bodyHeight = _windowRect.height - _theme.TitleBarHeight - _theme.WindowPadding * 2f;

            GUILayout.BeginVertical();
            DrawTitleBar();

            GUILayout.Space(_theme.WindowPadding);

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(bodyHeight));

            DrawLeftPanel();
            GUILayout.Space(_theme.WindowPadding);
            DrawRightPanel();

            GUILayout.EndHorizontal();

            GUILayout.Space(_theme.WindowPadding);
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width - 120f, _theme.TitleBarHeight));

            _windowRect = ClampWindowRect(_windowRect);
        }

        private void DrawTitleBar()
        {
            GUILayout.BeginHorizontal(_widgets.PanelAltStyle, GUILayout.Height(_theme.TitleBarHeight));

            GUILayout.Space(8f);
            GUILayout.Label("NO Mod Framework", _widgets.TitleLabelStyle, GUILayout.Height(_theme.TitleBarHeight - 6f));
            GUILayout.FlexibleSpace();

            string themeButtonText = _theme.Mode == FrameworkThemeMode.Dark ? "Light" : "Dark";
            if (GUILayout.Button(themeButtonText, _widgets.SmallButtonStyle, GUILayout.Width(70f), GUILayout.Height(24f)))
            {
                ToggleTheme();
            }

            GUILayout.Space(6f);

            if (GUILayout.Button("X", _widgets.SmallButtonStyle, GUILayout.Width(30f), GUILayout.Height(24f)))
            {
                SetMenuOpen(false);
            }

            GUILayout.Space(6f);
            GUILayout.EndHorizontal();
        }

        private void DrawLeftPanel()
        {
            GUILayout.BeginVertical(_widgets.PanelStyle, GUILayout.Width(_theme.LeftPanelWidth), GUILayout.ExpandHeight(true));

            GUILayout.Label("Mods", _widgets.TitleLabelStyle);
            GUILayout.Space(6f);

            _modListScroll = _widgets.BeginScrollView(_modListScroll);

            IList<IModMenu> menus = ModMenuAPI.GetMenusUnsafe();
            if (menus == null || menus.Count == 0)
            {
                GUILayout.Label("No registered mods.", _widgets.MutedLabelStyle);
            }
            else
            {
                for (int i = 0; i < menus.Count; i++)
                {
                    DrawMenuListRow(menus[i]);
                    GUILayout.Space(4f);
                }
            }

            _widgets.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawMenuListRow(IModMenu menu)
        {
            if (menu == null)
                return;

            bool selected = _selectedMenu != null && string.Equals(_selectedMenu.Id, menu.Id, StringComparison.OrdinalIgnoreCase);
            GUIStyle style = selected ? _widgets.SelectedButtonStyle : _widgets.ButtonStyle;

            Rect rowRect = GUILayoutUtility.GetRect(10f, 40f, GUILayout.ExpandWidth(true));
            if (GUI.Button(rowRect, GUIContent.none, style))
            {
                SelectMenu(menu);
            }

            float iconSize = 22f;
            float x = rowRect.x + 8f;
            float y = rowRect.y + (rowRect.height - iconSize) * 0.5f;

            if (menu.Icon != null)
            {
                GUI.DrawTexture(new Rect(x, y, iconSize, iconSize), menu.Icon, ScaleMode.ScaleToFit, true);
                x += iconSize + 8f;
            }

            Rect textRect = new Rect(x, rowRect.y + 8f, rowRect.width - (x - rowRect.x) - 8f, 24f);
            GUIStyle textStyle = selected ? _widgets.SelectedButtonStyle : _widgets.LabelStyle;
            GUI.Label(textRect, menu.DisplayName, textStyle);
        }

        private void DrawRightPanel()
        {
            GUILayout.BeginVertical(_widgets.PanelStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (_selectedMenu == null)
            {
                GUILayout.Label("No mod selected.", _widgets.MutedLabelStyle);
                GUILayout.EndVertical();
                return;
            }

            GUILayout.Label(_selectedMenu.DisplayName, _widgets.TitleLabelStyle);
            GUILayout.Space(6f);

            _contentScroll = _widgets.BeginScrollView(_contentScroll);

            try
            {
                _selectedMenu.Draw(_uiContext);
            }
            catch (Exception ex)
            {
                GUILayout.Label("Error drawing mod panel.", _widgets.TitleLabelStyle);
                GUILayout.Space(4f);
                GUILayout.Label(ex.ToString(), _widgets.MutedLabelStyle);

                if (FrameworkLogger != null)
                    FrameworkLogger.LogError(ex);
            }

            _widgets.EndScrollView();
            GUILayout.EndVertical();
        }

        private void SetMenuOpen(bool open)
        {
            if (_menuOpen == open)
                return;

            _menuOpen = open;

            if (_menuOpen)
            {
                RefreshRegistryState();
                if (_selectedMenu != null)
                {
                    SafeMenuOpen(_selectedMenu);
                }
            }
            else
            {
                if (_selectedMenu != null)
                {
                    SafeMenuClose(_selectedMenu);
                }
            }

            _fade.SetOpen(_menuOpen, !_fadeEnabled.Value);
        }

        private void SelectMenu(IModMenu menu)
        {
            if (menu == null)
                return;

            if (_selectedMenu != null && string.Equals(_selectedMenu.Id, menu.Id, StringComparison.OrdinalIgnoreCase))
                return;

            if (_menuOpen && _selectedMenu != null)
            {
                SafeMenuClose(_selectedMenu);
            }

            _selectedMenu = menu;
            _lastSelectedModId.Value = menu.Id;
            _contentScroll = Vector2.zero;

            if (_menuOpen && _selectedMenu != null)
            {
                SafeMenuOpen(_selectedMenu);
            }
        }

        private void RefreshRegistryState()
        {
            if (_seenRegistryVersion == ModMenuAPI.RegistryVersion)
                return;

            _seenRegistryVersion = ModMenuAPI.RegistryVersion;

            IList<IModMenu> menus = ModMenuAPI.GetMenusUnsafe();

            if (menus == null || menus.Count == 0)
            {
                _selectedMenu = null;
                _lastSelectedModId.Value = string.Empty;
                return;
            }

            IModMenu found = null;

            if (!string.IsNullOrEmpty(_lastSelectedModId.Value))
            {
                for (int i = 0; i < menus.Count; i++)
                {
                    if (string.Equals(menus[i].Id, _lastSelectedModId.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        found = menus[i];
                        break;
                    }
                }
            }

            if (found == null)
            {
                for (int i = 0; i < menus.Count; i++)
                {
                    if (_selectedMenu != null &&
                        string.Equals(menus[i].Id, _selectedMenu.Id, StringComparison.OrdinalIgnoreCase))
                    {
                        found = menus[i];
                        break;
                    }
                }
            }

            if (found == null)
                found = menus[0];

            SelectMenu(found);
        }

        private void SafeMenuOpen(IModMenu menu)
        {
            try
            {
                menu.OnMenuOpen();
            }
            catch (Exception ex)
            {
                if (FrameworkLogger != null)
                    FrameworkLogger.LogError(ex);
            }
        }

        private void SafeMenuClose(IModMenu menu)
        {
            try
            {
                menu.OnMenuClose();
            }
            catch (Exception ex)
            {
                if (FrameworkLogger != null)
                    FrameworkLogger.LogError(ex);
            }
        }

        private void ToggleTheme()
        {
            FrameworkThemeMode current = ReadThemeMode();
            FrameworkThemeMode next = current == FrameworkThemeMode.Dark
                ? FrameworkThemeMode.Light
                : FrameworkThemeMode.Dark;

            _themeMode.Value = (int)next;
            RebuildTheme();
        }

        private FrameworkThemeMode ReadThemeMode()
        {
            return _themeMode.Value == 1
                ? FrameworkThemeMode.Light
                : FrameworkThemeMode.Dark;
        }

        private void RebuildTheme()
        {
            _cachedThemeMode = ReadThemeMode();
            _theme = FrameworkTheme.Create(_cachedThemeMode);
        }

        private Rect ClampWindowRect(Rect rect)
        {
            float minW = 720f;
            float minH = 440f;

            rect.width = Mathf.Clamp(rect.width, minW, Screen.width - 20f);
            rect.height = Mathf.Clamp(rect.height, minH, Screen.height - 20f);
            rect.x = Mathf.Clamp(rect.x, 10f, Screen.width - rect.width - 10f);
            rect.y = Mathf.Clamp(rect.y, 10f, Screen.height - rect.height - 10f);

            return rect;
        }

        private void SaveWindowRect()
        {
            _windowX.Value = _windowRect.x;
            _windowY.Value = _windowRect.y;
            _windowW.Value = _windowRect.width;
            _windowH.Value = _windowRect.height;
        }
    }
}