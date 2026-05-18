using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfTransformation.Roles.Impostor;
using TownOfUs.Modules.Localization;

namespace TownOfTransformation.Options.Roles.Impostor;

public sealed class SkibidiToiletOptions : AbstractOptionGroup<SkibidiToiletRole>
{
    public override string GroupName => TouLocale.Get("ExampleRoleBaker", "Baker");

    [ModdedNumberOption("Skibidi Toilet Poop Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PoopCooldown { get; set; } = 30f;

    [ModdedNumberOption("Skibidi Toilet Poop Duration", 0f, 120f, 5f, MiraNumberSuffixes.Seconds, "Forever")]
    public float PoopDuration { get; set; } = 45f;
    [ModdedNumberOption("Skibidi Toilet Poop Range", 0f, 2f, 0.1f, MiraNumberSuffixes.Seconds, "Anywhere")]
    public float PoopRange { get; set; } = 1f;
}