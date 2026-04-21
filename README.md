**NO UI Framework**

A lightweight in-game UI framework for Nuclear Option BepInEx mods.

NO UI Framework gives modders a shared menu system where individual mods can register their own settings pages instead of each mod building a separate UI. It provides a clean, consistent interface for toggles, sliders, dropdowns, buttons, value rows, sections, scrollable panels, and themed menu styling.

Features:

- Central mod menu registry
- Simple `IModMenu` API for adding new mod pages
- Built-in UI widgets for common settings controls
- Theme support
- Scrollable menu layout
- Supports multiple registered mods
- Sortable menu entries
- Designed for BepInEx Nuclear Option mods

Example use:

```csharp
ModMenuAPI.Register(new MyModMenu());
```

Mods implement `IModMenu` and draw their UI through `ModUiContext`:

```csharp
public void Draw(ModUiContext ui)
{
    ui.Widgets.Section("Settings");
    enabled = ui.Widgets.Toggle("Enabled", enabled);
}
```

Built to make Nuclear Option mod configuration cleaner, easier to maintain, and consistent across mods.
