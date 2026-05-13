using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtras.Roles.Neutral;
using TownOfUs.Modules.Localization;

namespace TouExtras.Options.Roles.Neutral;

public sealed class FortegreenOptions : AbstractOptionGroup<FortegreenRole>
{
    public override string GroupName => TouLocale.Get("ExampleRoleFortegreen", "Fortegreen");

    [ModdedNumberOption("ExampleOptionFortegreenKillCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 25f;

    [ModdedNumberOption("ExampleOptionFortegreenRunCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RunCooldown { get; set; } = 45f;
    [ModdedNumberOption("ExampleOptionFortegreenTransformCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TransformCooldown { get; set; } = 45f;
    [ModdedNumberOption("ExampleOptionFortegreenTransformTime", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TransformTime { get; set; } = 15f;
    [ModdedNumberOption("ExampleOptionFortegreenDisguiseCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float DisguiseCooldown { get; set; } = 30f;
    [ModdedNumberOption("ExampleOptionFortegreenDisguiseTime", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float DisguiseTime { get; set; } = 15f;
    [ModdedNumberOption("ExampleOptionFortegreenDisguiseTimeMulti", 1f, 3f, 0.5f, MiraNumberSuffixes.Multiplier)]
    public float DisguiseMultiplier { get; set; } = 1.5f;
    [ModdedNumberOption("ExampleOptionFortegreenTransformTimeMultiplier", 1f, 3f, 0.5f, MiraNumberSuffixes.Multiplier)]
    public float TransformTimeMultiplier { get; set; } = 2f;

    [ModdedNumberOption("ExampleOptionFortegreenGetsImpostorVisionAtLvl", 1f, 3f, 1f, MiraNumberSuffixes.None, "Always")]
    public float ImpostorVision { get; set; } = 1f;


    [ModdedNumberOption("ExampleOptionFortegreenCanVentAtLvl", 1f, 3f, 1f, MiraNumberSuffixes.None, "Always")]
    public float CanVentAtLvl { get; set; } = 2f;
}