using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtras.Roles.Impostor;
using TownOfUs.Modules.Localization;

namespace TouExtras.Options.Roles.Impostor;

public sealed class EvilSwapperOptions : AbstractOptionGroup<EvilSwapperRole>
{
    public override string GroupName => TouLocale.Get("TouRoleEvilSwapper", "Evil Swapper");

    [ModdedToggleOption("ExampleOptionEvilSwapperCanVent")]
    public bool CanVent { get; set; } = true;

    [ModdedToggleOption("ExampleOptionEvilSwapperCanButton")]
    public bool CanButton { get; set; } = true;
}
