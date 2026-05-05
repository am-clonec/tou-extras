using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtras.Roles.Impostor;
using TownOfUs.Modules.Localization;

namespace TouExtras.Options.Roles.Impostor;

public sealed class SoulEaterOptions : AbstractOptionGroup<SoulEaterRole>
{
    public override string GroupName => TouLocale.Get("TouRoleSoulEater", "Soul Eater");

    [ModdedToggleOption("TouOptionSoulEaterCanVent")]
    public bool CanVent { get; set; } = false;
}