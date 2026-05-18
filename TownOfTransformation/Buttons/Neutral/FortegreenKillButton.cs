using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfTransformation.Assets;
using TownOfTransformation.Options.Roles.Neutral;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfTransformation.Buttons.Neutral;

public sealed class FortegreenKillButton : TownOfUsKillRoleButton<FortegreenRole, PlayerControl>, IDiseaseableButton,
    IKillButton
{
    public override string Name => TranslationController.Instance.GetStringWithDefault(StringNames.KillLabel, "Kill");
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TouExampleColors.Fortegreen;
    public override float Cooldown => Math.Clamp(OptionGroupSingleton<FortegreenOptions>.Instance.KillCooldown + MapCooldown, 5f, 120f);
    public override LoadableAsset<Sprite> Sprite => NeutAssets.SentinelKillSprite;
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }
    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    
    public override PlayerControl? GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    public override bool CanUse()
    {
        return base.CanUse() && Role.Transformed;
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            Error("Fortegreen Kill: Target is null");
            return;
        }

        PlayerControl.LocalPlayer.RpcCustomMurder(Target);

        if (Role.Level < 3)
        {
            Role.Level = Role.Level + 1;
            Role.Reload();
        }
    }
}