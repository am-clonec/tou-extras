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
using TownOfUs.Options.Modifiers.Impostor;
using TownOfUs.Options.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game;
using Epic.OnlineServices.RTC;
using MS.Internal.Xml.XPath;
using Il2CppSystem.Runtime.InteropServices;



namespace TouExtras.Modifiers;

public sealed class BowTieErModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => ModifName();
    public override bool HideOnUi => false;
    public override ModifierFaction FactionType => ModifierFaction.NonCrewVisibility;
    public override string IntroInfo => IntroText();
    public override string LocaleKey => "BowTieEr";
        public override string GetDescription()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Use your ability to give everyone a pink tie!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Use your ability to make everyone a cool astronaut!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Use your ability to make everyone a scary pirate!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Use your ability to start a party and give everyone a party hat!";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
   
    }

    public string GetAdvancedDescription()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Use your ability to give everyone a pink tie, making them all look the same.";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Use your ability to make everyone a cool astronaut, making them all look the same.";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Use your ability to make everyone a scary pirate, making them all look the same.";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Use your ability to start a party and give everyone a party hat, making them all look the same.";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }

    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new(BTButtonName(),
                    BTButtonDesc(),
                    Icon()),
                new(UnBTButtonName(),
                    UnBTButtonDesc(),
                    Icon())
            };
        }
    }

    public static string BTButtonDesc()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Use this ability to give everyone a bow tie and make them look the same for a short time. This can be used to hide the identity of the Impostors or just for fun!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Use this ability to make everyone a cool astronaut and make them look the same for a short time. This can be used to hide the identity of the Impostors or just for fun!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Use this ability to make everyone a scary pirate and make them look the same for a short time. This can be used to hide the identity of the Impostors or just for fun!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Use this ability to start a party and give everyone a party hat, making them look the same for a short time. This can be used to hide the identity of the Impostors or just for fun!";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }

        public static string BTButtonName()
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
        public static string UnBTButtonDesc()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Use this ability to remove the bow ties early.";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Use this ability to remove the astronaut suits early.";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Use this ability to remove the pirate hats early.";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Use this ability to stop the party and remove the party hats early.";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }

    public static string UnBTButtonName()
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
    public static string IntroText()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Use your ability to give everyone a pink tie!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Use your ability to make everyone a cool astronaut!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Use your ability to make everyone a scary pirate!";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Use your ability to start a party and give everyone a party hat!";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }

    public static string ModifName()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Bow Tie Lover";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "Astronaut Lover";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Pirate Lover";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "Party Lover";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }
    
    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<BowTieErOptions>.Instance.ModifierChance;
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<BowTieErOptions>.Instance.ModifierAmount;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && (!role.IsCrewmate());
    }
}