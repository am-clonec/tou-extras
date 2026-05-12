using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Options.Roles.Neutral;
using TouExtras.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Networking.Attributes;
using TownOfUs.Events;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Options.Modifiers.Universal;
using UnityEngine;

using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.LocalSettings;
using MiraAPI.Networking;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Patches;
using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Buttons.Impostor;
using TouExtras.Modules;
using TouExtras.Options.Roles.Impostor;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Impostor;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;

using MiraAPI.Keybinds;
using TouExtras.Roles.Impostor;
using TouExtras.Roles.Neutral;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;


using TouExtras.Modifiers;
using Reactor.Networking;
using HarmonyLib;
using Rewired;
using TouExtras;
using TouExtras.Options.Modifiers.NeutImp;

namespace TouExtras.Buttons.Neutral;

public sealed class FortegreenTransformButton : TownOfUsRoleButton<FortegreenRole>
{
    public override string Name => "Transform";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TouExampleColors.Fortegreen;
    public override float Cooldown => CD();
    public override LoadableAsset<Sprite> Sprite => TouNeutAssets.RampageSprite;
    public override float EffectDuration => EffDur();



    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }

    public static float EffDur()
    {
        if (CustomRoleSingleton<FortegreenRole>.Instance.Level < 3)
        {
            return Math.Clamp(OptionGroupSingleton<FortegreenOptions>.Instance.TransformTime * ((OptionGroupSingleton<FortegreenOptions>.Instance.TransformTimeMultiplier * CustomRoleSingleton<FortegreenRole>.Instance.Level) + 1), 5f, 30f);
        } else
        {
            return 999999f;
        }
    }

    public static float CD()
    {
        if (CustomRoleSingleton<FortegreenRole>.Instance.Level < 3)
        {
            return Math.Clamp(OptionGroupSingleton<FortegreenOptions>.Instance.TransformCooldown, 5f, 120f);
        } else
        {
            return 0f;
        }
    }


    public override bool CanUse()
    {
        if (HudManager.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
        {
            return false;
        }

        if (PlayerControl.LocalPlayer.HasModifier<GlitchHackedModifier>() || PlayerControl.LocalPlayer
                .GetModifiers<DisabledModifier>().Any(x => !x.CanUseAbilities))
        {
            return false;
        }

            return (Timer <= 0 && !EffectActive) ||
            (EffectActive && Timer <= EffectDuration - 2f);
    }
    public override void ClickHandler()
    {
        if (!CanUse())
        {
            return;
        }

        OnClick();
    }
    protected override void OnClick()
    {
        if (!Role.Transformed)
        {
            
            Role.Transformed = true;
            Timer = EffectDuration;
            Role.SetVisApp();
            EffectActive = true;
        }
        else
        {
            ResetCooldownAndOrEffect();
        }
    }

    public override void OnEffectEnd()
    {
        Role.Transformed = false;
        Role.ResetVisApp();
        EffectActive = false;
    }
}