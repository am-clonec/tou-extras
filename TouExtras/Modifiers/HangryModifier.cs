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
using TouExtras.Options.Modifiers.NeutImp;






namespace TouExtras.Modifiers;

public sealed class HangryModifier() : BaseModifier
{
    [HideFromIl2Cpp] public Muffin? Muffie { get; set; }
    public override string ModifierName => "Hangry";
    public override bool HideOnUi => false;




    public override void OnActivate()
    {
        if (PlayerControl.LocalPlayer == Player)
        {
        Muffie = Muffin.CreateMuffin(ExtrasGlobalVars.MuffinTarget, ExtrasGlobalVars.MuffinPos);
        Helpers.CreateAndShowNotification(
            TouLocale.GetParsed("TouRoleBakerCravingNotif", "You really want a muffin right now..."),
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
        }
        

    }
    public override void OnDeath(DeathReason reason)
    {
        Player.RpcRemoveModifier<HangryModifier>();
    }
    public override void OnDeactivate()
    {
        if (PlayerControl.LocalPlayer == ExtrasGlobalVars.MuffinTarget || PlayerControl.LocalPlayer.PlayerId == ExtrasGlobalVars.MuffinTarget.PlayerId)
        {
        Muffie?.Destroy();
        /*
        if (!ExtrasGlobalVars.MuffinEaten)
        {
        PlayerControl.LocalPlayer.RpcSpecialMurder(
                    PlayerControl.LocalPlayer,
                    teleportMurderer: false,
                    showKillAnim: false,
                    causeOfDeath: "BakerMuffin",
                    resetKillTimer: false);
        }
        */
        }
        
    }

}
