using HarmonyLib;
namespace NOModFramework
{
    [HarmonyPatch(typeof(MainMenu), "Start")]
    internal static class MainMenuStartPatch
    {
        private static void Postfix(MainMenu __instance)
        {
            if (FrameworkPlugin.Instance != null)
            {
                FrameworkPlugin.Instance.QueueMainMenuButton(__instance);
            }
        }
    }
    [HarmonyPatch(typeof(GameplayUI), "PauseGame")]
    internal static class GameplayUiPausePatch
    {
        private static void Postfix(GameplayUI __instance)
        {
            if (FrameworkPlugin.Instance != null)
            {
                FrameworkPlugin.Instance.AddPauseMenuButton(__instance);
            }
        }
    }
}