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

namespace TouExtras.Buttons.Impostor;

public sealed class BakerBakeButton : TownOfUsRoleButton<BakerRole>
{
    public override string Name => "Bake";
    public override Color TextOutlineColor => new Color32(255, 69, 0, 255); // Red-orange

    public override float Cooldown =>
        Math.Clamp(OptionGroupSingleton<BakerOptions>.Instance.BakeCooldown, 5f, 120f);

    public override float EffectDuration => OptionGroupSingleton<BakerOptions>.Instance.MuffinTime;

    public override LoadableAsset<Sprite> Sprite => TouNeutAssets.ChefCookSprite;

    public override void ClickHandler()
    {
        if (!CanClick())
        {
            return;
        }

        OnClick();
    }

    protected override void OnClick()
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
    }

        private void DoThing(PlayerControl plr)
            {
                if (!plr.IsRole<PestilenceRole>())
                {       
                plr.AddModifier<HangryModifier>();
                
                

                ExtrasGlobalVars.MuffinTarget = plr;
                ExtrasGlobalVars.MuffinPos = PlayerControl.LocalPlayer.transform.position;
                ExtrasGlobalVars.MuffinEaten = false;

                BakerRole.RpcPlaceMuffin(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer.transform.position, plr);

                EffectActive = true;
                Timer = EffectDuration;
                } else {
                plr.RpcSpecialMurder(
                PlayerControl.LocalPlayer,
                teleportMurderer: false,
                showKillAnim: true,
                causeOfDeath: "Pestilence",
                resetKillTimer: false);
                }

            }

    public void ForceEffectEnd()
    {
        Timer = 0f;
        ResetCooldownAndOrEffect();
    }
    public override void OnEffectEnd()
    {
        if (!ExtrasGlobalVars.MuffinEaten)
        {
        PlayerControl.LocalPlayer.RpcSpecialMurder(
                ExtrasGlobalVars.MuffinTarget,
                teleportMurderer: false,
                showKillAnim: true,
                causeOfDeath: "BakerMuffin",
                resetKillTimer: false);
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("TouRoleBakerKilledThroughCravingNotif", "Your craving victim couldn't find your delicious muffin in time... They died."),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        } else {
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("TouRoleBakerTargetSatisfiedCravingNotif", "Your craving victim satisfied their hunger with your delicious muffin! They survived."),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        }
        EffectActive = false;
    }
}
