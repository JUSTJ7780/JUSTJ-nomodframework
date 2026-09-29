using System;
using System.Collections.Generic;
using UnityEngine;

namespace NOModFramework
{
    public static class ModMenuAPI
    {
        private static readonly List<IModMenu> Menus = new List<IModMenu>();
        private static int _registryVersion = 0;

        internal static int RegistryVersion
        {
            get { return _registryVersion; }
        }

        internal static IList<IModMenu> GetMenusUnsafe()
        {
            return Menus;
        }

        // logging please ugg

        public static bool Register(IModMenu menu)
        {
            if (menu == null)
            {
                UnityEngine.Debug.LogWarning("[NO Mod Framework] Tried to register a null menu.");
                return false;
            }

            if (string.IsNullOrEmpty(menu.Id))
            {
                UnityEngine.Debug.LogWarning("[NO Mod Framework] Tried to register a menu with a null or empty Id.");
                return false;
            }

            if (string.IsNullOrEmpty(menu.DisplayName))
            {
                UnityEngine.Debug.LogWarning("[NO Mod Framework] Tried to register a menu with a null or empty DisplayName.");
                return false;
            }

            for (int i = 0; i < Menus.Count; i++)
            {
                if (string.Equals(Menus[i].Id, menu.Id, StringComparison.OrdinalIgnoreCase))
                {
                    UnityEngine.Debug.LogWarning("[NO Mod Framework] Duplicate menu Id rejected: " + menu.Id);
                    return false;
                }
            }

            Menus.Add(menu);
            SortMenus();
            _registryVersion++;

            return true;
        }

        public static bool Unregister(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            for (int i = 0; i < Menus.Count; i++)
            {
                if (string.Equals(Menus[i].Id, id, StringComparison.OrdinalIgnoreCase))
                {
                    Menus.RemoveAt(i);
                    _registryVersion++;
                    return true;
                }
            }

            return false;
        }

        public static bool IsRegistered(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            for (int i = 0; i < Menus.Count; i++)
            {
                if (string.Equals(Menus[i].Id, id, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static bool Open()
        {
            return FrameworkPlugin.OpenFrameworkMenu();
        }

        public static bool Close()
        {
            return FrameworkPlugin.CloseFrameworkMenu();
        }
        public static bool IsOpen
        {
            get { return FrameworkPlugin.IsFrameworkMenuOpen; }
        }        private static void SortMenus()
        {
            Menus.Sort(CompareMenus);
        }

        private static int CompareMenus(IModMenu a, IModMenu b)
        {
            if (a == null && b == null)
                return 0;

            if (a == null)
                return 1;

            if (b == null)
                return -1;

            int sort = a.SortOrder.CompareTo(b.SortOrder);
            if (sort != 0)
                return sort;

            return string.Compare(a.DisplayName, b.DisplayName, StringComparison.OrdinalIgnoreCase);
        }
    }
}