using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtras.Modifiers;
using TouExtras.Roles.Impostor;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;

namespace TouExtras.Options.Modifiers.NeutImp;

public sealed class BowTieErOptions : AbstractOptionGroup<BowTieErModifier>
{
    public override string GroupName => TouLocale.Get("BowTieErModifier", "Bow Tieer");
    public ModdedEnumOption BowType { get; set; } = new("TouExtrasOptionBowType",
        (int)BType.PinkTie, typeof(BType),
        [
            "TouExtrasOptionBowTypePinkTie", "TouExtrasOptionBowTypeAstronaut",
            "TouExtrasOptionBowTypePirate", "TouExtrasOptionBowTypeParty"
        ]);

    [ModdedNumberOption("Bow Tie Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TieCooldown { get; set; } = 30f;

    [ModdedNumberOption("Bow Tie Duration", 1f, 60f, 1f, MiraNumberSuffixes.Seconds)]
    public float TieTime { get; set; } = 15f;

    [ModdedNumberOption("Bow Tie Uses Per Round", 0f, 10f, 1f, MiraNumberSuffixes.None, "0", true)]
    public float TieCount { get; set; } = 0f;

    public ModdedNumberOption ModifierAmount { get; } =
    new("Bow Tieer Amount", 1f, 0f, 15f, 1f, MiraNumberSuffixes.None);

    public ModdedNumberOption ModifierChance { get; } =
    new("Bow Tieer Chance", 10f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<BowTieErOptions>.Instance.ModifierAmount > 0
        };

}

public enum BType
{
    PinkTie,
    Astronaut,
    Pirate,
    Party
}