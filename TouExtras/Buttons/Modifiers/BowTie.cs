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

namespace TouExtras.Buttons.Modifiers;

public sealed class BowTieButton : TownOfUsButton
{


    public override float Cooldown => Math.Clamp(OptionGroupSingleton<BowTieErOptions>.Instance.TieCooldown, 5f, 120f);
    public override float EffectDuration => Math.Clamp(OptionGroupSingleton<BowTieErOptions>.Instance.TieTime, 5f, 30f);

    public override string Name => NormalName();
    public override BaseKeybind Keybind => Keybinds.ModifierAction;
    public override Color TextOutlineColor => TouExampleColors.BowTie;
    public override ButtonLocation Location => ButtonLocation.BottomLeft;
    public override LoadableAsset<Sprite> Sprite => Icon();
    public override int MaxUses => (int)OptionGroupSingleton<BowTieErOptions>.Instance.TieCount;
    public override bool ZeroIsInfinite => true;
    public override bool Enabled(RoleBehaviour? role)
    {
        return PlayerControl.LocalPlayer != null &&
               PlayerControl.LocalPlayer.HasModifier<BowTieErModifier>() &&
               !PlayerControl.LocalPlayer.Data.IsDead;
    }

    public static string NormalName()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Bow Tie";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Astronaut Suit";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Pirate Hat";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Party Time";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }

    public static string UndoName()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Un-Bow Tie";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Return To Earth";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Abandon Ship";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Stop Party";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }

    public static LoadableAsset<Sprite> Icon()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return ModifAssets.BowTie;
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return ModifAssets.Astronaut;
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return ModifAssets.Pirate;
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return ModifAssets.Party;
        } else
        {
            return ModifAssets.BowTie;
        }
    }


    public override void ClickHandler()
    {
        if (!CanUse())
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

        return ((Timer <= 0 && !EffectActive && (!LimitedUses || UsesLeft > 0)) ||
                (EffectActive && Timer <= EffectDuration - 2f));
    }
    


    protected override void OnClick()
    {
        if (!EffectActive)
        {
        Helpers.GetAlivePlayers().ForEach(p => p.AddModifier<BowTieActiveModifier>());
        EffectActive = true;
        Timer = EffectDuration;
        OverrideName(UndoName());
        UsesLeft--;
        if (LimitedUses)
        {
            Button?.SetUsesRemaining(UsesLeft);
        }
        } else
        {
            ResetCooldownAndOrEffect();
        }
        
    }


    public override void OnEffectEnd()
    {
        Helpers.GetAlivePlayers().ForEach(p => p.RemoveModifier<BowTieActiveModifier>());
        EffectActive = false;
        OverrideName(NormalName());
    }

}