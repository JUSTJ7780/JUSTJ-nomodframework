using System;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NOModFramework
{
    internal static class NativeMenuButtons
    {
        private const string ModsButtonName = "NO_ModFramework_ModsButton";

        internal static bool TryInjectMainMenuButton(MainMenu mainMenu)
        {
            if (mainMenu == null)
            {
                return false;
            }

            Button source = Traverse.Create(mainMenu).Field("missionsButton").GetValue<Button>();
            Button workshopButton = FindButton(mainMenu.transform, "WorkshopButton");
            return TryCloneButton(source, workshopButton);
        }

        internal static bool TryInjectPauseMenuButton(GameplayUI gameplayUi)
        {
            if (gameplayUi == null || gameplayUi.menuCanvas == null)
            {
                return false;
            }

            Button source = FindButton(gameplayUi.menuCanvas.transform, "ResumeButton");
            return TryCloneButton(source, null);
        }

        private static bool TryCloneButton(Button source, Button placementAnchor)
        {
            if (source == null || source.transform.parent == null)
            {
                return false;
            }

            Transform parent = source.transform.parent;
            Transform existing = parent.Find(ModsButtonName);
            GameObject clone;

            if (existing != null)
            {
                clone = existing.gameObject;
            }
            else
            {
                clone = UnityEngine.Object.Instantiate(source.gameObject, parent, false);
                clone.name = ModsButtonName;
            }

            Button button = clone.GetComponent<Button>();
            if (button == null)
            {
                UnityEngine.Object.Destroy(clone);
                return false;
            }

            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(OpenModsMenu);
            SetLabel(clone, "Mods");
            PositionAfterAnchor(clone.transform, parent, placementAnchor);
            return true;
        }

        private static void PositionAfterAnchor(Transform button, Transform parent, Button placementAnchor)
        {
            if (placementAnchor != null && placementAnchor.transform.parent == parent)
            {
                button.SetSiblingIndex(placementAnchor.transform.GetSiblingIndex() + 1);
                return;
            }

            button.SetSiblingIndex(parent.childCount - 1);
        }

        private static Button FindButton(Transform root, string name)
        {
            Button[] buttons = root.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null && string.Equals(buttons[i].gameObject.name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return buttons[i];
                }
            }

            return null;
        }

        private static void SetLabel(GameObject buttonObject, string text)
        {
            TMP_Text[] labels = buttonObject.GetComponentsInChildren<TMP_Text>(true);
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].text = text;
            }
        }

        private static void OpenModsMenu()
        {
            FrameworkPlugin.OpenFrameworkMenu();
        }
    }
}
