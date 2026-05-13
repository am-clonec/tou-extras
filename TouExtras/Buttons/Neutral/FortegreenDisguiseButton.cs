using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Options.Roles.Impostor;
using TouExtras.Roles.Impostor;
using TouExtras.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using MiraAPI.Utilities;
using MiraAPI.Modifiers;
using TouExtras.Modifiers;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;
using TouExtras.Modules;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using TouExtras.Buttons.Impostor;
using TownOfUs;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Impostor;
using TownOfUs.Roles.Neutral;
using HarmonyLib;
using MiraAPI.Modifiers.Types;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Events;
using JetBrains.Annotations;
using Il2CppMono.Security.Authenticode;
using System.Collections;
using TownOfUs.Utilities.Appearances;
using Reactor.Utilities.Extensions;
using TownOfUs.Networking;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Options;
using TownOfUs.Patches;
using TouExtras.Options.Roles.Neutral;
using Rewired;

namespace TouExtras.Buttons.Impostor;

public sealed class FortegreenDisguiseButton : TownOfUsRoleButton<FortegreenRole>
{
    public override string Name => "Disguise";
    public override Color TextOutlineColor => TouExampleColors.Fortegreen; // Red-orange

    public override float Cooldown =>
        Math.Clamp(OptionGroupSingleton<BakerOptions>.Instance.BakeCooldown, 5f, 120f);

    public override float EffectDuration => OptionGroupSingleton<FortegreenOptions>.Instance.DisguiseTime * ((OptionGroupSingleton<FortegreenOptions>.Instance.DisguiseMultiplier * Role.Level) + 1);

    public override LoadableAsset<Sprite> Sprite => TouImpAssets.MorphSprite;

    public PlayerControl target;

    public override void ClickHandler()
    {
        if (!CanClick())
        {
            return;
        }

        OnClick();
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

    protected override void OnClick()
    {
        if (!EffectActive)
        {
        PlayerControl.LocalPlayer.NetTransform.Halt();

        if (Minigame.Instance)
        {
            return;
        }

        var player1Menu = CustomPlayerMenu.Create();
        player1Menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        player1Menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        player1Menu.Begin(
            plr => plr != null && plr != PlayerControl.LocalPlayer && !plr.Data.IsDead && !(plr.IsRole<ImpostorRole>()),
            plr =>
            {
                player1Menu.Close();

                if (plr == null)
                {
                    return;
                }
                DoThing(plr);
                
            }
        );
        foreach (var panel in player1Menu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
        } else
        {
            ResetCooldownAndOrEffect();
        }
    }

        private void DoThing(PlayerControl plr)
            {
    
                plr.AddModifier<FortegreenDisguisedModifier>();
                target = plr;
                EffectActive = true;
                Timer = EffectDuration;
                

            }
    
    public override void OnEffectEnd()
    {
        target.RemoveModifier<FortegreenDisguisedModifier>();
        EffectActive = false;
    }
}