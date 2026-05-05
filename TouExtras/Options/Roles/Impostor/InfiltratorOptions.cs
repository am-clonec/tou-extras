using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtras.Roles.Impostor;
using TownOfUs.Modules.Localization;

namespace TouExtras.Options.Roles.Impostor;

public sealed class InfiltratorOptions : AbstractOptionGroup<InfiltratorRole>
{
    public override string GroupName => TouLocale.Get("ExampleRoleInfiltrator", "Infiltrator");

    [ModdedNumberOption("ExampleOptionInfiltratorTransportCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TransportCooldown { get; set; } = 30f;

    [ModdedToggleOption("ExampleOptionInfiltratorCanVent")]
    public bool CanVent { get; set; } = true;
}