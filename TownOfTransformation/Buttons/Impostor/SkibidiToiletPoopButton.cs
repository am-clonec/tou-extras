using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfTransformation.Assets;
using TownOfTransformation.Options.Roles.Impostor;
using TownOfTransformation.Roles.Impostor;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using MiraAPI.Utilities;
using MiraAPI.Modifiers;
using TownOfTransformation.Modifiers;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;
using TownOfTransformation.Modules;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using TownOfTransformation.Buttons.Impostor;
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

namespace TownOfTransformation.Buttons.Impostor;

public sealed class SkibidiToiletPoopButton : TownOfUsRoleButton<SkibidiToiletRole>
{
    public override string Name => "Poop";
    public override Color TextOutlineColor => TownOfUsColors.Impostor;

    public override float Cooldown =>
        Math.Clamp(OptionGroupSingleton<SkibidiToiletOptions>.Instance.PoopCooldown, 5f, 120f);


    public override LoadableAsset<Sprite> Sprite => ImpAssets.SkibidiToiletPoopSprite;

    public override bool CanUse()
    {
        return base.CanUse() && Role.Transformed && PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, OptionGroupSingleton<SkibidiToiletOptions>.Instance.PoopRange) != null;
    }

    protected override void OnClick()
    {
        var target = PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, OptionGroupSingleton<SkibidiToiletOptions>.Instance.PoopRange);
        target.RpcAddModifier<PoopedModifier>();
    }

        
}
