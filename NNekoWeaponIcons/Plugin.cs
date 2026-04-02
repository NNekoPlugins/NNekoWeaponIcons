using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using NNekoWeaponIcons.Windows;

namespace NNekoWeaponIcons;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IPlayerState PlayerState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IKeyState KeyState { get; private set; } = null!;
    [PluginService] internal static IGameGui GameGui { get; private set; } = null!;


    private const string CommandName = "/nnwi";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("NNekoWeaponIcons");
    private ConfigWindow ConfigWindow { get; init; }
    private WeaponIcons? weaponIcons;

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Toggle the plugin Config."
        });

        // Tell the UI system that we want our windows to be drawn through the window system
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        // This adds a button to the plugin installer entry of this plugin which allows
        // toggling the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;

        Enable();

        Log.Information($"==={PluginInterface.Manifest.Name} has loaded.===");
    }

    public void Dispose()
    {
        // Unregister all actions to not leak anything during disposal of plugin
        Disable();
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;

        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        ConfigWindow.Toggle();
    }

    public void ToggleConfigUi() => ConfigWindow.Toggle();
    public void ToggleMainUi() => ToggleConfigUi();

    public void Initialize(Configuration config)
    {
        if (config.WeaponIconsEnabled)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
    public void Enable()
    {
        weaponIcons ??= new WeaponIcons(GameGui, KeyState, DataManager, TextureProvider, Log, Configuration);


        if (Configuration.WeaponIconsEnabled)
        {
            PluginInterface.UiBuilder.Draw += weaponIcons.Draw;
        }
        //PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        Log.Information("[WeaponIcons] Enabled.");
    }
    public void Disable()
    {
        //PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;


        if (weaponIcons != null)
        {
            PluginInterface.UiBuilder.Draw -= weaponIcons.Draw;
            weaponIcons.Dispose();
            weaponIcons = null;
        }

        Log.Information("[WeaponIcons] Disabled.");
    }
}
