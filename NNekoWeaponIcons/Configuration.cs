using Dalamud.Configuration;

namespace NNekoWeaponIcons;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool WeaponIconsEnabled { get; set; } = true;
    public bool WeaponIconsRequireCtrl { get; set; } = false;
    public bool WeaponIconsMiniMode { get; set; } = true;

    public void Save()
    {
        NNekoWeaponIcons.PluginInterface.SavePluginConfig(this);
    }
}
