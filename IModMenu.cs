using UnityEngine;

namespace NOModFramework
{
    public interface IModMenu
    {
        string Id { get; }
        string DisplayName { get; }
        Texture2D Icon { get; }
        int SortOrder { get; }

        void OnMenuOpen();
        void OnMenuClose();
        void Draw(ModUiContext ui);
    }
}