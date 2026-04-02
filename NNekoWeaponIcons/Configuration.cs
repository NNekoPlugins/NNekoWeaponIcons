using Dalamud.Configuration;
using System;

namespace NNekoWeaponIcons;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool WeaponIconsEnabled { get; set; } = false;
    public bool WeaponIconsRequireCtrl { get; set; } = false;
    public bool WeaponIconsMiniMode { get; set; } = true;

    // The below exists just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
