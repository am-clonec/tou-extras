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
using TownOfTransformation.Assets;
using TownOfTransformation.Buttons.Impostor;
using TownOfTransformation.Modules;
using TownOfTransformation.Options.Roles.Impostor;
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
using TownOfTransformation.Roles.Impostor;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;


using TownOfTransformation.Modifiers;
using Reactor.Networking;
using HarmonyLib;
using Rewired;
using TownOfTransformation;
using TownOfTransformation.Options.Modifiers.NeutImp;

namespace TownOfTransformation.Buttons.Neutral;

public sealed class SkibidiToiletTransformButton : TownOfUsRoleButton<SkibidiToiletRole>
{
    public override string Name => "Transform";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfUsColors.Impostor;
    public override float Cooldown => 1f;
    public override LoadableAsset<Sprite> Sprite => ImpAssets.SkibidiToiletTransformSprite;





    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
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

            return base.CanUse();
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
            
            Role.Transform();
            OverrideName("Untransform");
            
        }
        else
        {
            Role.Untransform();
            OverrideName("Transform");
        }
    }


}