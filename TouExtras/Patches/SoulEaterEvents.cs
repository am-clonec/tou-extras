using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modules;
using TownOfUs.Roles.Neutral;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MiraAPI.Events.Vanilla.Meeting.Voting;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Voting;
using Reactor.Utilities;
using TownOfUs.Events.Modifiers;
using TownOfUs.Extensions;
using TownOfUs.Roles.Crewmate;
using TouExtras.Roles.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Crewmate;
using UnityEngine;


using MiraAPI.Hud;
using MiraAPI.LocalSettings;

using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Buttons.Impostor;
using TouExtras.Options.Roles.Impostor;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;

using TownOfUs.Roles.Impostor;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;


using MiraAPI.Events;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using Reactor.Networking.Attributes;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;

namespace TouExtras.Patches;

public static class SoulEaterEvents
{
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        var source = @event.Source;
        var target = @event.Target;

        if (SoulEaterRole.AutoPlaceFakePlayers && source.IsRole<SoulEaterRole>() && !MeetingHud.Instance)
            // leave behind standing body
        {
            _ = new FakePlayer(target);
        }
    }

    [RegisterEvent]
    public static void ReviveEventHandler(PlayerReviveEvent @event)
    {
        var player = @event.Player;
        
        var fakePlayer = FakePlayer.FakePlayers.FirstOrDefault(x => x.PlayerId == player.PlayerId);
        if (fakePlayer != null)
        {
            FakePlayer.FakePlayers.Remove(fakePlayer);
            fakePlayer.Destroy();
        }
    }
}