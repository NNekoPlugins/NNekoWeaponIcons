using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using Dalamud.Interface.Windowing;
using System;
using System.Numerics;

namespace NNekoWeaponIcons.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    // We give this window a constant ID using ###.
    // This allows for labels to be dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("Weapon Icons (Armoury Board Overlay)###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(500, 125);
        SizeCondition = ImGuiCond.Always;

        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (configuration.IsConfigWindowMovable)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
    }

    public override void Draw()
    {
        //if (!ImGui.CollapsingHeader("Weapon Icons (Armoury Board Overlay)", ImGuiTreeNodeFlags.DefaultOpen))
        //    return;

        ImGui.PushID("WeaponIconsSettings");

        bool mini = configuration.WeaponIconsMiniMode;
        if (ImGui.Checkbox("Mini mode (bottom-left icons)", ref mini))
        {
            configuration.WeaponIconsMiniMode = mini;
            configuration.Save();
        }
        ImGuiComponents.HelpMarker("Draws smaller icons anchored to the bottom-left of each Armoury slot.");

        bool requireCtrl = configuration.WeaponIconsRequireCtrl;
        if (ImGui.Checkbox("Require Ctrl key", ref requireCtrl))
        {
            configuration.WeaponIconsRequireCtrl = requireCtrl;
            configuration.Save();
        }
        ImGuiComponents.HelpMarker("When enabled, overlay only appears while holding Ctrl.");
    }

    
}
