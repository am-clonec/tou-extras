using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtras.Roles.Impostor;
using TownOfUs.Modules.Localization;

namespace TouExtras.Options.Roles.Impostor;

public sealed class BakerOptions : AbstractOptionGroup<BakerRole>
{
    public override string GroupName => TouLocale.Get("ExampleRoleBaker", "Baker");

    [ModdedNumberOption("ExampleOptionBakerBakeCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BakeCooldown { get; set; } = 30f;

    [ModdedNumberOption("ExampleOptionBakerMuffinTime", 1f, 60f, 1f, MiraNumberSuffixes.Seconds)]
    public float MuffinTime { get; set; } = 15f;
}