using System.Globalization;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs;

namespace TownOfTransformation;

[BepInAutoPlugin("greenlemon.au.towntftransformation", "Town Of Transformation")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[BepInDependency(TownOfUsPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class TownOfTransformationPlugin : BasePlugin, IMiraPlugin
{
    /// <summary>
    ///     Gets the specified Culture for string manipulations.
    /// </summary>
    public static CultureInfo Culture => TownOfUs.TownOfUsPlugin.Culture;

    /// <inheritdoc />
    public string OptionsTitleText => "Town Of Transformation";

    /// <summary>
    ///     Determines if the current build is a dev build or not. This will change certain visuals as well as always grab news locally to be up to date.
    /// </summary>
    public static bool IsDevBuild => true;

    /// <inheritdoc />
    public ConfigFile GetConfigFile()
    {
        return Config;
    }

    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        ReactorCredits.Register("Town Of Transformation", Version, IsDevBuild, ReactorCredits.AlwaysShow);
        IL2CPPChainloader.Instance.Finished += Modules.ExtensionLocale.SearchInternalLocale; // Initialise AFTER the mods are loaded to ensure maximum parity (no need for the soft dependency either then)

        Harmony.PatchAll();
    }
}
