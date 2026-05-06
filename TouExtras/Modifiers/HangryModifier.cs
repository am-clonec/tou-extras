using MiraAPI.Modifiers;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using TownOfUs.Interfaces;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using UnityEngine;


using MiraAPI.Hud;

using Reactor.Networking.Attributes;
using TownOfUs.Events;
using TownOfUs.Modifiers.Game.Universal;



namespace TouExtras.Modifiers;

public sealed class HangryModifier : BaseModifier
{
    public override string ModifierName => "Hangry";
    public override bool HideOnUi => true;

    public override bool? CanVent()
    {
        return true;
    }

}