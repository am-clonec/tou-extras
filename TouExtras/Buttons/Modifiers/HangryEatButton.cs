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

namespace TownOfUs.Buttons.Modifiers;

public sealed class HangryEatButton : TownOfUsButton
{
    [HideFromIl2Cpp] public Muffin? Muffie { get; set; }
    public override float Cooldown => 0f;
    public override int MaxUses => 1;
    public override string Name => TouLocale.GetParsed("TouModifierHangryEatButton", "Eat");
    public override BaseKeybind Keybind => Keybinds.ModifierAction;
    public override Color TextOutlineColor => TownOfUsColors.ButtonBarry;
    public override ButtonLocation Location => ButtonLocation.BottomLeft;
    public override LoadableAsset<Sprite> Sprite => TouAssets.BarryButtonSprite;
    
    public override bool Enabled(RoleBehaviour? role)
    {
        return PlayerControl.LocalPlayer != null &&
               PlayerControl.LocalPlayer.HasModifier<HangryModifier>() &&
               !PlayerControl.LocalPlayer.Data.IsDead;
    }

    public override bool CanUse()
    {
        
        return Vector3.Distance(PlayerControl.LocalPlayer.transform.position, ExtrasGlobalVars.MuffinPos) < 0.3f;
    }

    protected override void OnClick()
    {
        Muffie?.Destroy();
        PlayerControl.LocalPlayer.RemoveModifier<HangryModifier>();
    }
}